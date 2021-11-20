using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IBulletinService
    {
        public Task<bool> AddBulletin(Bulletin bulletin);
        public Task<IList<Bulletin>> GetBulletinsAsync(int page, int limit, ulong groupId = 1);
        public Task<IList<Bulletin>> GetBulletinsAsyncCached(int page, int limit, ulong groupId = 1);
    }
}
