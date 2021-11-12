using BulletinBoard.Data;
using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public class BulletinService
    {
        private readonly ApplicationDbContext dbContext;

        public BulletinService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddBulletin(Bulletin bulletin)
        {

        }
        public async Task<bool> RemoveBulletin(ulong id)
        {
            return true;
        }   
        public async Task<bool> UpdateBulletin(Bulletin bulletin)
        {
            return true;
        }
    }
}
