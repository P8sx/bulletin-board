using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public enum ViolationSortBy
    {
        Bulletin = 0,
        Board = 1,
        Comment = 2
    }
    public class HelperService : BaseService, IHelperService
    {
        private readonly IBulletinService _bulletinService;
        private readonly IBoardService _boardService;
        private readonly UserManager<User> _userManager;
        public HelperService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<HelperService> logger, IMemoryCache memoryCache, GlobalService globalService, IBulletinService bulletinService, IBoardService boardService, UserManager<User> userManager) : base(dbFactory, logger, memoryCache, globalService)
        {
            _bulletinService = bulletinService;
            _boardService = boardService;
            _userManager = userManager;
        }
        public async Task AddToDefaultGroupAsync(User user)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var boardId = await dbContext.Boards.Where(b => b.Guid == GlobalService.DefaultBoardGuid).Select(b=>b.Id)
                .FirstOrDefaultAsync();
            await dbContext.BoardUsers.AddAsync(new BoardUser { BoardId = boardId, Role = BoardRole.User, UserId = user.Id });
            await dbContext.SaveChangesAsync();
        }

        public async Task Report(Violation violation)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            await dbContext.Violations.AddAsync(violation);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Violation>> GetViolations(int page = 1, int limit = 20, ViolationSortBy sortBy = ViolationSortBy.Bulletin)
        {
            if (page == 0)
                page = 1;

            if (limit == 0)
                limit = int.MaxValue;

            var skip = (page - 1) * limit;
            
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var querry = dbContext.Violations
                .Include(v => v.Board)
                .Include(v => v.Comment).ThenInclude(c => c!.User)
                .Include(v => v.Bulletin).ThenInclude(b => b!.Images)
                .Include(v => v.Bulletin).ThenInclude(b => b!.User)
                .Include(v => v.User).ThenInclude(u => u!.Image);
            return sortBy switch
            {
                ViolationSortBy.Board => await querry.OrderByDescending(v => v.BoardId)
                    .Skip(skip)
                    .Take(limit)
                    .ToListAsync(),
                ViolationSortBy.Bulletin => await querry.OrderByDescending(v => v.BulletinId)
                    .Skip(skip)
                    .Take(limit)
                    .ToListAsync(),
                ViolationSortBy.Comment => await querry.OrderByDescending(v => v.CommentId)
                    .Skip(skip)
                    .Take(limit)
                    .ToListAsync(),
                _ => await querry.Skip(skip).Take(limit).ToListAsync()
            };
        }

        public async Task<int> GetViolationsCount()
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Violations.CountAsync();
        }

        public async Task<bool> RemoveViolation(Violation violation)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var result = await dbContext.Violations.Where(v => v.Id == violation.Id).FirstOrDefaultAsync();
            if(result == null) return false;
            dbContext.Remove(result);
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteViolation(Violation violation)
        {
            var removeResult = await RemoveViolation(violation);
            if (!removeResult) return false;
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            if (violation.Bulletin != null) return await _bulletinService.RemoveBulletinAsync(violation.Bulletin);
            if (violation.Board != null) return await _boardService.RemoveBoardAsync(violation.Board);
            if (violation.Comment == null) return false;
            var result = await dbContext.Comments.Where(b => b.Id == violation.CommentId).FirstOrDefaultAsync();
            if (result == null) return false;
            dbContext.Remove(result);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BanUser(Ban ban, Violation violation)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            User? banUser = null;
            if (violation.Board != null)
            {
                banUser = await dbContext.BoardUsers.AsNoTracking().Where(b => b.Role == BoardRole.Owner && b.BoardId == violation.BoardId).Select(b => b.User)
                    .FirstOrDefaultAsync();
            }

            if (violation.Comment is {User: { }})
            {
                banUser = await dbContext.Comments.AsNoTracking().Where(c => c.Id == violation.CommentId).Select(c => c.User)
                    .FirstOrDefaultAsync();
            }
            else if(violation.Comment != null)
            {
                banUser = violation.Comment.User;
            }


            if (violation.Bulletin is {User: { }})
            {
                banUser = await dbContext.Bulletins.AsNoTracking().Where(c => c.Id == violation.BulletinId).Select(c => c.User)
                    .FirstOrDefaultAsync();
            }
            else if(violation.Bulletin != null)
            {
                banUser = violation.Bulletin.User;
            }

            if (banUser == null) return false;
            ban.UserId = banUser.Id;
            await dbContext.Bans.AddAsync(ban);
            await dbContext.SaveChangesAsync();
            var usermanagerUser = await _userManager.FindByIdAsync(banUser.Id.ToString());
            var roles = await _userManager.GetRolesAsync(usermanagerUser);
            await _userManager.AddToRoleAsync(usermanagerUser, "Banned");
            await _userManager.RemoveFromRolesAsync(usermanagerUser, roles);
            return true;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Users.Include(z => z.Roles).Select(user => new User()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Joined = user.Joined,
                Roles = user.Roles,
                RolesName = user.Roles.Select(z=>z.Name).ToArray()
            }).ToListAsync();
        }

        public async Task<bool> RemoveUser(User user)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var dbUser = await dbContext.Users.Where(z => z.Id == user.Id).FirstOrDefaultAsync();
            if (dbUser == null) return false;
            dbContext.Users.Remove(dbUser);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
