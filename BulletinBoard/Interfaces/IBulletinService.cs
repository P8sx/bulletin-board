using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IBulletinService
    {
        Task<bool> AddBulletin(Bulletin bulletin);
        Task<IList<Bulletin>> GetBulletinsAsyncCached(int page, int limit, User user, Group group, BulletinSort sort = default);
        Task<Bulletin?> GetBulletinInfoAsyncCached(User user, Group group, Bulletin bulletin);
        Task<int> GetBulletinsCountAsyncCached(Group group);
    }
}
