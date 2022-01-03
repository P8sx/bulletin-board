using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
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

        public UserService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext, ILogger<BulletinService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
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
                return _dbContext.Users.Where(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name).FirstOrDefault();
            }
            return null;
        }


        private List<Group>? GetUserGroups(User? user)
        {
            var mainGroup = _dbContext.Groups.Include(g => g.Image).FirstOrDefault(g => g.Id == 1);

            if (mainGroup == null)
                return null;

            var userGroups = _dbContext.GroupUsers.Where(gu => gu.User == user).Include(g => g.Group.Image).Select(gu => gu.Group).ToList();
            userGroups.Add(mainGroup);
            return userGroups!;
        }
        private List<GroupUser>? GetUserGroupsRoles(User? user)
        {
            var groupUser = _dbContext.GroupUsers.Include(gu => gu.User).Include(gu => gu.Role).Where(gu => gu.User == user).ToList();
            return groupUser;
        }

        public bool IsInGroup(long groupId)
        {
            if (UserGroups == null || UserGroups.Count == 0) 
                return false;
            return UserGroups.Any(g => g.Id == Convert.ToUInt64(groupId));
        }
        public bool IsInGroup(Group group)
        {
            if (UserGroups == null || UserGroups.Count == 0)
                return false;
            return UserGroups.Any(g => g.Id == group.Id);
        }

        public async Task Bookmark(BulletinBookmark bookmark)
        {
            var exist = await _dbContext.BulletinBookmarks.FirstOrDefaultAsync(v => v.BulletinId == bookmark.BulletinId && v.UserId == bookmark.UserId);
            if (exist == default)
                await _dbContext.BulletinBookmarks.AddAsync(bookmark);
            else
                _dbContext.BulletinBookmarks.Remove(exist);

            await _dbContext.SaveChangesAsync();
        }
        public async Task Bookmark(Bulletin bulletin)
        {
            var exist = await _dbContext.BulletinBookmarks.FirstOrDefaultAsync(v => v.BulletinId == bulletin.Id && v.UserId == User!.Id);
            if (exist == default)
                await _dbContext.BulletinBookmarks.AddAsync(new BulletinBookmark { Bulletin = bulletin, User = User});
            else
                _dbContext.BulletinBookmarks.Remove(exist);

            await _dbContext.SaveChangesAsync();
        }
        public async Task Vote(Bulletin bulletin)
        {
            var exist = await _dbContext.BulletinsVotes.FirstOrDefaultAsync(v => v.BulletinId == bulletin.Id && v.UserId == User!.Id);
            if (exist == default)
                await _dbContext.BulletinsVotes.AddAsync(new BulletinVote { Bulletin = bulletin, User = User });
            else
                _dbContext.BulletinsVotes.Remove(exist);

            await _dbContext.SaveChangesAsync();
        }
    }
}
