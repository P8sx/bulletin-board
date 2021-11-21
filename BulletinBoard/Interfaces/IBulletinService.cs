using BulletinBoard.DTOs;
using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IBulletinService
    {
        public Task<bool> AddBulletin(Bulletin bulletin);
        public Task<IList<BulletinInfoDTO>> GetBulletinsInfoAsync(int page, int limit,User user ,ulong groupId = 1);
        public Task<IList<BulletinInfoDTO>> GetBulletinsInfoAsyncCached(int page, int limit, User user, ulong groupId = 1);
        public Task Vote(BulletinVote vote);
    }
}
