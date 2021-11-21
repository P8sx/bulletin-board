using BulletinBoard.DTOs;
using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IBulletinService
    {
        public Task<bool> AddBulletin(Bulletin bulletin);
        public Task<IList<BulletinInfoDTO>> GetBulletinsAsyncCached(int page, int limit, User user, ulong groupId = 1);
        public Task<IList<BulletinInfoDTO>> GetUserBulletinsAsyncCached(int page, int limit, User user);
        public Task Vote(BulletinVote vote);
    }
}
