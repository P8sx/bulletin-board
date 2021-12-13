using BulletinBoard.DTOs;
using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IBulletinService
    {
        Task<bool> AddBulletin(Bulletin bulletin);

        Task<IList<BulletinInfoDTO>> GetBulletinsAsyncCached(int page, int limit, User user, Group group, BulletinSort sort = default);
        Task<IList<BulletinInfoDTO>> GetBulletinsAsyncCached(int page, int limit, User user, BulletinSort sort = default);


        Task Vote(BulletinVote vote);
    }
}
