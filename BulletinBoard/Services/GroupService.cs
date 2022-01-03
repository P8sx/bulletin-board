using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;

        public GroupService(ApplicationDbContext dbContext, ILogger<BulletinService> logger, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _logger = logger;
            _memoryCache = memoryCache;
            _dbContext.Database.SetCommandTimeout(TimeSpan.FromSeconds(5));

        }
        public async Task<Group> GetDefaultGroupAsyncCached()
        {
            var result = await _memoryCache.GetOrCreateAsync($"DefaultGroup", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20);
                return await GetDefaultGroupAsync();
            });
            return result;
        }
        public async Task<Group> GetGroupAsyncCached(ulong groupId)
        {
            var result = await _memoryCache.GetOrCreateAsync($"Group{groupId}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20);
                return await GetGroupAsync(groupId);
            });
            return result;
        }

        private async Task<Group> GetDefaultGroupAsync()
        {
            return await _dbContext.Groups.Where(g => g.Id == 1).Include(g=>g.Image).FirstOrDefaultAsync();
        }
        private async Task<Group> GetGroupAsync(ulong groupId)
        {
            return await _dbContext.Groups.Where(g => g.Id == groupId).Include(g => g.Image).FirstOrDefaultAsync();
        }
    }
}
