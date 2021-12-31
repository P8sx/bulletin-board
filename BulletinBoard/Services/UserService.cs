using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public UserService(ApplicationDbContext dbContext, ILogger<BulletinService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

        }

        public async Task<List<Group>> GetUserGroups(User user)
        {
            var mainGroup = await _dbContext.Groups.Include(g=>g.Image).FirstOrDefaultAsync(g => g.Id == 1);

            if (mainGroup == null) 
                return new();

            var userGroups = await _dbContext.GroupUsers.Where(gu => gu.User == user).Include(g => g.Group.Image).Select(gu => gu.Group).ToListAsync();
            var groups = new List<Group>
            {
                mainGroup
            };
            if (userGroups != null && userGroups.Any())
                groups.AddRange(userGroups);
            return groups;  
        }
        public async Task<List<GroupUser>> GetUserGroupsRolesAsync(User user)
        {
            var groupUser = _dbContext.GroupUsers.Include(gu => gu.User).Include(gu => gu.Role).Where(gu => gu.User == user).ToListAsync();
            return await groupUser;
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
    }
}
