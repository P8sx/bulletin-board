using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public class GroupService : IGroupService
    {

        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        public GroupService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<BulletinService> logger, IMemoryCache memoryCache)
        {
            _dbFactory = dbFactory;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<Group?> GetGroupInfoAsyncCached(Group group)
        {
            var result = await _memoryCache.GetOrCreateAsync($"Group{group.Id}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20);
                return await GetGroupAsync(group);
            });
            return result;
        }
        private async Task<Group?> GetGroupAsync(Group group)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.Groups.Where(g => g.Id == group.Id).Include(g => g.Image).FirstOrDefaultAsync();
        }
        public async Task<bool> AddGroupAdmin(Group group, User user)
        {
            try
            {
                using var _dbContext = _dbFactory.CreateDbContext();
                var groupUser = new GroupUser()
                {
                    UserId = user.Id,
                    GroupId = group.Id,
                    RoleId = Convert.ToUInt64(RoleValue.GroupAdmin),
                    Joined = DateTime.UtcNow,
                };
                await _dbContext.GroupUsers.AddAsync(groupUser);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return false;
        }
        public async Task<bool> AddGroup(Group group)
        {
            try
            {
                using var _dbContext = _dbFactory.CreateDbContext();
                await _dbContext.Groups.AddAsync(group);
                await _dbContext.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }         
            return false;
        }
    }
}
