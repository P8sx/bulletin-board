using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IBulletinService
    {
        Task<bool> AddBulletin(Bulletin bulletin);
        Task<IList<Bulletin>> GetBulletinsAsyncCached(int page, int limit, User user, Group group, BulletinSort sort = default);
        Task<int> GetBulletinsCountAsyncCached(Group group);

        Task<IList<Bulletin>> GetUserBulletinsAsyncCached(int page, int limit, User user, BulletinSort sort = default);
        Task<int> GetUserBulletinsCountAsyncCached(User user);

        Task<IList<Bulletin>> GetUserBookmarkBulletinsAsyncCached(int page, int limit, User user, BulletinSort sort = default);
        Task<int> GetUserBookmarkBulletinsCountAsyncCached(User user);

        Task<Bulletin?> GetBulletinInfoAsyncCached(User? user, Bulletin bulletin);
        Task<Bulletin?> GetBulletinInfoAsync(User? user, Bulletin bulletin);

        Task<bool> RemoveBulletin(Bulletin bulletin);
        Task<bool> UpdateBulletin(Bulletin bulletin);
    }
}
