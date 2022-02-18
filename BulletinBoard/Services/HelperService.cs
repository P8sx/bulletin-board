using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static BulletinBoard.Services.GlobalService;

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
        public HelperService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<HelperService> logger, IMemoryCache memoryCache, GlobalService globalService) : base(dbFactory, logger, memoryCache, globalService)
        {

        }
        public async Task AddToDefaultGroupAsync(User user)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var boardId = await dbContext.Boards.Where(b => b.Guid == GlobalService.DefaultBoardGuid).Select(b=>b.Id)
                .FirstOrDefaultAsync();
            await dbContext.BoardUsers.AddAsync(new BoardUser() { BoardId = boardId, Role = BoardRole.User, UserId = user.Id });
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

        public async Task RejectViolation(Violation violation)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var result = await dbContext.Violations.Where(v => v.Id == violation.Id).FirstOrDefaultAsync();
            if(result == null) return;
            dbContext.Remove(result);
            await dbContext.SaveChangesAsync();
        }
    }
}
