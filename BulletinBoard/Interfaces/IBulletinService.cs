using BulletinBoard.DTOs;
using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IBulletinService
    {
        Task<bool> AddBulletin(Bulletin bulletin);

        Task<IList<BulletinInfoDTO>> GetBulletinsAsyncCached(int page, int limit, User user, Group group, BulletinSort sort = default);
        Task<IList<BulletinInfoDTO>> GetBulletinsAsyncCached(int page, int limit, User user, BulletinSort sort = default);
        Task<int> GetBulletinsCountAsyncCached(User user, Group group);
        Task<int> GetBulletinsCountAsyncCached(User user);
        Task Vote(BulletinVote vote);
    }
}
