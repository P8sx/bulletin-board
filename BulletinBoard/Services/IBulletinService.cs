using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IBulletinService
    {
        public Task<bool> AddBulletin(Bulletin bulletin);
        public Task<IList<Bulletin>> GetBulletinsAsync(int page, int limit);
    }
}
