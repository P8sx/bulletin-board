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
            var defaultGroup = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Id == 1);
            var groups = await _dbContext.UserRoles.Where(u => u.UserId == user.Id).Select(g => g.Group).ToListAsync();
            if(groups is not null)
            {
                groups.Add(defaultGroup);
                return groups;
            }
            else
                return new List<Group>() { defaultGroup };
        }
    }
}
