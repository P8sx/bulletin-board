using BulletinBoard.Data;
using BulletinBoard.Extensions;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public User? User { get; private set; }
        public List<GroupUser>? UserGroups { get; private set; }

        public bool IsAuthenticated
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.Identity != null)
                    return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
                return false;
            }
        }

        public UserService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<BulletinService> logger, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor): base(dbFactory,logger,memoryCache)
        {
            _httpContextAccessor = httpContextAccessor;
            User = GetUser();
            UserGroups = GetUserGroups(User);
        }

        public void UpdateUserGroups()
        {
            UserGroups = GetUserGroups(User);
        }

        private User? GetUser()
        {
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.Identity != null && _httpContextAccessor.HttpContext.User.Identity.Name != null)
            {
                using var _dbContext = _dbFactory.CreateDbContext();
                return _dbContext.Users.Where(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name).FirstOrDefault();
            }
            return null;
        }


        private List<GroupUser>? GetUserGroups(User? user)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            var groupUser = _dbContext.GroupUsers
                .Include(gu => gu.User)
                .Include(gu => gu.Role)
                .Include(gu => gu.Group)
                .ThenInclude(gu => gu!.Image)
                .Where(gu => gu.User == user)
                .ToList();
            return groupUser;
        }
        public bool IsInGroup(Group group)
        {
            if (group.Id == Const.DefaultGroupId)
                return true;
            if (UserGroups == null || UserGroups.Count == 0)
                return false;
            return UserGroups.Any(g => g.GroupId == group.Id);
        }

        public bool IsGroupModerator(Group group)
        {
            if (UserGroups!.Any(a => (a.GroupId == group.Id)&&(a.Role!.RoleValue == RoleValue.GroupModerator || a.Role!.RoleValue == RoleValue.GroupAdmin)))
                return true;
            return false;
        }
        public bool IsBulletinOwner(Bulletin bulletin)
        {
            if (User != null && (User.Id == bulletin.UserId || User.Id == bulletin.User!.Id))
                return true;
            return false;
        }
        public void AddUserToGroup(Group group)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            try
            {
                _dbContext.GroupUsers.Add(new GroupUser { GroupId = group.Id, UserId=User!.Id, Role=new Role("User") });
            }catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task Bookmark(BulletinBookmark bookmark)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            var exist = await _dbContext.BulletinBookmarks.FirstOrDefaultAsync(v => v.BulletinId == bookmark.BulletinId && v.UserId == bookmark.UserId);
            if (exist == default)
                await _dbContext.BulletinBookmarks.AddAsync(bookmark);
            else
                _dbContext.BulletinBookmarks.Remove(exist);

            await _dbContext.SaveChangesAsync();
        }
        public async Task Bookmark(Bulletin bulletin)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            var exist = await _dbContext.BulletinBookmarks.FirstOrDefaultAsync(v => v.BulletinId == bulletin.Id && v.UserId == User!.Id);
            if (exist == null)
                await _dbContext.BulletinBookmarks.AddAsync(new BulletinBookmark { BulletinId = bulletin.Id, UserId = User!.Id});
            else
                _dbContext.BulletinBookmarks.Remove(exist);

            await _dbContext.SaveChangesAsync();
        }
        public async Task Vote(Bulletin bulletin)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            var exist = await _dbContext.BulletinsVotes.FirstOrDefaultAsync(v => v.BulletinId == bulletin.Id && v.UserId == User!.Id);
            if (exist == null)
                await _dbContext.BulletinsVotes.AddAsync(new BulletinVote { BulletinId = bulletin.Id, UserId = User!.Id });
            else
                _dbContext.BulletinsVotes.Remove(exist);

            await _dbContext.SaveChangesAsync();
        }
    }
}
