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

        private List<GroupUser> _userGroupsRoles = new();
        private List<Group?> _userGroups = new();
        private List<Group?> _userAwaitingAcceptanceGroups = new();
        private List<Group?> _userAwaitingInvitationsGroups = new();

        public UserService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<BulletinService> logger, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor) : base(dbFactory, logger, memoryCache)
        {
            _httpContextAccessor = httpContextAccessor;
            User = GetUser();
            UserGroups = GetUserGroups(User);
            UpdateUserGroups2();
        }


        public void UpdateUserGroups2()
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            _userGroupsRoles = _dbContext.GroupUsers
                .Include(gu => gu.User)
                .Include(gu => gu.Role)
                .Include(gu => gu.Group)
                .ThenInclude(gu => gu!.Image)
                .Where(gu => gu.User == User)
                .ToList();
            _userGroups = _userGroupsRoles
                .Where(g => g.Role!.RoleValue != RoleValue.GroupInvited)
                .Where(g => g.Role!.RoleValue != RoleValue.GroupAwaitingAcceptance)
                .Select(u => u.Group).Distinct()
                .ToList();
            _userAwaitingAcceptanceGroups = _userGroupsRoles
                .Where(g => g.Role!.RoleValue == RoleValue.GroupAwaitingAcceptance)
                .Select(u => u.Group).Distinct()
                .ToList();
            _userAwaitingInvitationsGroups = _userGroupsRoles
                .Where(g => g.Role!.RoleValue == RoleValue.GroupInvited)
                .Select(u => u.Group).Distinct()
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
        public List<Group?> GetUserGroups()
        {
            var userGroups = UserGroups!.Where(g => g.Role!.RoleValue != RoleValue.GroupInvited).Select(u => u.Group).Distinct().ToList();
            return userGroups;
        }
        public List<Group?> GetUserAwaitingGroups()
        {
            return UserGroups!.Where(g => g.Role!.RoleValue == RoleValue.GroupInvited).Select(u => u.Group).Distinct().ToList();
        }
        public bool IsInGroup(Group group)
        {
            if (group.Id == Consts.DefaultGroupId)
                return true;
            if (UserGroups == null || UserGroups.Count == 0)
                return false;
            return UserGroups.Any(g => g.GroupId == group.Id);
        }
        public bool IsGroupModerator(Group group)
        {
            if (UserGroups!.Any(a => (a.GroupId == group.Id) && (a.Role!.RoleValue == RoleValue.GroupModerator || a.Role!.RoleValue == RoleValue.GroupAdmin)))
                return true;
            return false;
        }
        public bool IsBulletinOwner(Bulletin bulletin)
        {
            if (User != null && (User.Id == bulletin.UserId || User.Id == bulletin.User!.Id))
                return true;
            return false;
        }
        public bool CanEditBulletin(Group group, Bulletin bulletin)
        {
            return IsInGroup(group) && (IsBulletinOwner(bulletin) || IsGroupModerator(group));
        }
        public void AddUserToGroup(Group group)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            try
            {
                _dbContext.GroupUsers.Add(new GroupUser { GroupId = group.Id, UserId = User!.Id, Role = new Role("User") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task Bookmark(Bulletin bulletin)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            var exist = await _dbContext.BulletinsBookmarks.FirstOrDefaultAsync(v => v.BulletinId == bulletin.Id && v.UserId == User!.Id);
            if (exist == null)
                await _dbContext.BulletinsBookmarks.AddAsync(new BulletinBookmark { BulletinId = bulletin.Id, UserId = User!.Id });
            else
                _dbContext.BulletinsBookmarks.Remove(exist);

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
