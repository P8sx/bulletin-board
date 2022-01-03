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
            return await _dbContext.Groups.Where(g => g.Id == group.Id).Include(g => g.Image).FirstOrDefaultAsync();
        }
    }
}
