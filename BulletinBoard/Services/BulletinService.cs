using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Services
{
    public class BulletinService : IBulletinService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public BulletinService(ApplicationDbContext dbContext, ILogger<BulletinService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> AddBulletin(Bulletin bulletin)
        {
            try
            {
                await _dbContext.Bulletins.AddAsync(bulletin);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            return true;
        }
        public async Task<IList<Bulletin>> GetBulletinsAsync(int page, int limit, ulong groupId = 1)
        {
            if (page == 0)
                page = 1;

            if (limit == 0)
                limit = int.MaxValue;

            var skip = (page - 1) * limit;

            var savedSearches = _dbContext.Bulletins.Include(x => x.Images).Include(u => u.User).ThenInclude(i =>i.Image).Where(g=>g.GroupId==groupId).Skip(skip).Take(limit);
            return await savedSearches.ToListAsync();
        }

    }
}
