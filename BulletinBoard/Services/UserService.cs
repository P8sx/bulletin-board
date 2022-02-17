using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidatorService _validatorService;

        public User? User { get; private set; }
        private List<BoardUser> _userBoardsRoles = new();
        private List<Board?> _userBoards = new();
        private List<Board?> _userPendingAcceptanceBoards = new();
        private List<Board?> _userPendingInvitationsBoards = new();

        public UserService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<UserService> logger, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor, IValidatorService validatorService, GlobalService globalService) : base(dbFactory, logger, memoryCache,globalService)
        {
            _httpContextAccessor = httpContextAccessor;
            _validatorService = validatorService;
            User = GetUser();
            UpdateUserBoards();
        }


        public void UpdateUserBoards()
        {
            if(User == null || User == default) return;
            using var dbContext = _dbFactory.CreateDbContext();
            _userBoardsRoles = dbContext.BoardUsers
                .Include(gu => gu.User)
                .Include(gu => gu.Board)
                .ThenInclude(gu => gu!.Image)
                .Where(gu => gu.User == User)
                .ToList();
            _userBoards = _userBoardsRoles
                .Where(g => g.Role != BoardRole.Invited)
                .Where(g => g.Role != BoardRole.PendingAcceptance)
                .Select(u => u.Board).Distinct()
                .ToList();
            _userPendingAcceptanceBoards = _userBoardsRoles
                .Where(g => g.Role == BoardRole.PendingAcceptance)
                .Select(u => u.Board).Distinct()
                .ToList();
            _userPendingInvitationsBoards = _userBoardsRoles
                .Where(g => g.Role == BoardRole.Invited)
                .Select(u => u.Board).Distinct()
                .ToList();
        }


        public bool IsAuthenticated
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.Identity != null)
                    return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
                return false;
            }
        }
        private User? GetUser()
        {
            if (_httpContextAccessor.HttpContext?.User.Identity?.Name == null) return null;
            using var dbContext = _dbFactory.CreateDbContext();
            return dbContext.Users.FirstOrDefault(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);
        }

        // User groups
        public List<Board?> GetUserBoards() => _userBoards;
        public List<Board?> GetUserPendingAcceptanceBoards() => _userPendingAcceptanceBoards;
        public List<Board?> GetUserPendingInvitationsBoards() => _userPendingInvitationsBoards;

        // User roles
        public bool IsInBoard(Board board)
        {
            RolesValid();
            return board.Guid == GlobalService.DefaultBoardGuid || _userBoards.Any(g => g!.Id == board.Id || g.Guid == board.Guid);
        }
        public bool IsBoardModerator(Board board)
        {
            RolesValid();
            return _userBoardsRoles.Any(a => (a.BoardId == board.Id || a.Board!.Guid == board.Guid) && a.Role is BoardRole.Moderator or BoardRole.Admin or BoardRole.Owner);
        }
        public bool IsBoardAdmin(Board board)
        {
            RolesValid();
            return _userBoardsRoles.Any(a => (a.BoardId == board.Id || a.Board!.Guid == board.Guid)  && a.Role is BoardRole.Admin or BoardRole.Owner);
        }
        public bool IsBoardOwner(Board board)
        {
            RolesValid();
            return _userBoardsRoles.Any(a => (a.BoardId == board.Id || a.Board!.Guid == board.Guid)  && (a.Role == BoardRole.Owner));
        }
        public bool IsBulletinOwner(Bulletin bulletin)
        {
            RolesValid();
            return User != null && (User.Id == bulletin.UserId || User.Id == bulletin.User!.Id);
        }

        // User privileges
        public bool CanEditBulletin(Board board, Bulletin bulletin)
        {
            return IsInBoard(board) && (IsBulletinOwner(bulletin) || IsBoardModerator(board));
        }
        public bool PendingAcceptance(Board board)
        {
            return _userPendingAcceptanceBoards.Any(g => g!.Guid == board.Guid);
        }
        public bool PendingInvitations(Board board)
        {           
            return _userPendingInvitationsBoards.Any(g => g!.Guid == board.Guid);
        }

        public async Task<IEnumerable<User>> Search(string userName)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Users
                .Where(obj => EF.Functions.Like(obj.UserName, $"{userName}%"))
                .Where(u => u.Id != User!.Id)
                .Take(10)
                .ToListAsync();
        }
        public async Task Bookmark(Bulletin bulletin)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var exist = await dbContext.BulletinsBookmarks.FirstOrDefaultAsync(v => v.BulletinId == bulletin.Id && v.UserId == User!.Id);
            if (exist == null)
                await dbContext.BulletinsBookmarks.AddAsync(new BulletinBookmark { BulletinId = bulletin.Id, UserId = User!.Id });
            else
                dbContext.BulletinsBookmarks.Remove(exist);

            await dbContext.SaveChangesAsync();
        }
        public async Task Vote(Bulletin bulletin)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var exist = await dbContext.BulletinsVotes.FirstOrDefaultAsync(v => v.BulletinId == bulletin.Id && v.UserId == User!.Id);
            if (exist == null)
                await dbContext.BulletinsVotes.AddAsync(new BulletinVote { BulletinId = bulletin.Id, UserId = User!.Id });
            else
                dbContext.BulletinsVotes.Remove(exist);

            await dbContext.SaveChangesAsync();
        }
        public async Task<User?> GetUserInfoAsync(User user)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Users.Include(u => u.Image).Where(u => u.Id == user.Id).FirstOrDefaultAsync();
        }
        private void RolesValid()
        {
            if (_validatorService.CheckValidRoles(User!))
                UpdateUserBoards();
        }

        public async Task UpdateImage(Image image)
        {
            User!.Image = image;
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            dbContext.Users.Update(User);
            await dbContext.SaveChangesAsync();
            User.Image = image;
        }
    }
}
