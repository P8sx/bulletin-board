using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Services
{
    public class BulletinService : IBulletinService
    {
        private readonly ApplicationDbContext _dbContext;

        public BulletinService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<bool> AddBulletin(Bulletin bulletin)
        {
            await _dbContext.Bulletins.AddAsync(bulletin);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<IList<Bulletin>> GetBulletinsAsync(int page, int limit)
        {
            if (page == 0)
                page = 1;

            if (limit == 0)
                limit = int.MaxValue;

            var skip = (page - 1) * limit;

            var savedSearches = _dbContext.Bulletins.Include(x => x.Images).Include(u => u.User).Skip(skip).Take(limit);
            return await savedSearches.ToListAsync();
        }

    }
}
