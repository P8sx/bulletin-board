using BulletinBoard.Data;
using BulletinBoard.Extensions;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public class HelperService : BaseService, IHelperService
    {
        public HelperService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<HelperService> logger, IMemoryCache memoryCache) : base(dbFactory, logger, memoryCache)
        {

        }
        public async Task AddToDefaultGroupAsync(User user, string roleName)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            await _dbContext.GroupUsers.AddAsync(new GroupUser() { GroupId = Consts.DefaultGroupId, Role = GroupRole.User, UserId = user.Id });
            await _dbContext.SaveChangesAsync();
        }
    }
}
