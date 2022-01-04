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
        public List<Group>? UserGroups { get; private set; }
        public List<GroupUser>? UserGroupsRoles { get; private set; }

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
            UserGroupsRoles = GetUserGroupsRoles(User);
        }

        public void UpdateUserGroups()
        {
            UserGroups = GetUserGroups(User);
            UserGroupsRoles = GetUserGroupsRoles(User);
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


        private List<Group>? GetUserGroups(User? user)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            var mainGroup = _dbContext.Groups.Include(g => g.Image).FirstOrDefault(g => g.Id == Const.DefaultGroupId);

            if (mainGroup == null)
                return null;

            var userGroups = _dbContext.GroupUsers.Where(gu => gu.User == user).Include(g => g.Group!.Image).Select(gu => gu.Group).ToList();
            userGroups.Add(mainGroup);
            return userGroups!;
        }
        private List<GroupUser>? GetUserGroupsRoles(User? user)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            var groupUser = _dbContext.GroupUsers.Include(gu => gu.User).Include(gu => gu.Role).Where(gu => gu.User == user).ToList();
            return groupUser;
        }
        public bool IsInGroup(Group group)
        {
            if (UserGroups == null || UserGroups.Count == 0)
                return false;
            return UserGroups.Any(g => g.Id == group.Id);
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
