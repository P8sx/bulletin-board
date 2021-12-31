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
        public async Task<Group> GetDefaultGroupAsync()
        {
            return await _dbContext.Groups.Where(g => g.Id == 1).Include(g=>g.Image).FirstOrDefaultAsync();
        }
        public async Task<Group> GetGroupAsync(ulong groupId)
        {
            return await _dbContext.Groups.Where(g => g.Id == groupId).Include(g => g.Image).FirstOrDefaultAsync();
        }
    }
}
