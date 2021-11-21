using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<Group>> GetUserGroups(User user)
        {
            var mainGroup = _dbContext.Groups.FirstOrDefault(g => g.Id == 1);
            if (mainGroup == null) 
                return new();

            var userGroups = await _dbContext.GroupUsers.Where(gu => gu.User == user).Select(gu => gu.Group).ToListAsync();
            var groups = new List<Group>
            {
                mainGroup
            };
            if (userGroups != null && userGroups.Any())
                groups.AddRange(userGroups);

            return groups;  
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
