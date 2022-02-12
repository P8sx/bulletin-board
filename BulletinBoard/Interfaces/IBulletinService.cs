using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IBulletinService
    {
        Task<bool> AddBulletinAsync(Bulletin bulletin);
        Task<IList<Bulletin>> GetBulletinsAsyncCached(int page, int limit, User? user, Board board, BulletinSort sort = default);
        Task<int> GetBulletinsCountAsyncCached(Board board);

        Task<IList<Bulletin>> GetUserBulletinsAsyncCached(int page, int limit, User? user, BulletinSort sort = default);
        Task<int> GetUserBulletinsCountAsyncCached(User user);

        Task<IList<Bulletin>> GetUserBookmarkBulletinsAsyncCached(int page, int limit, User? user, BulletinSort sort = default);
        Task<int> GetUserBookmarkBulletinsCountAsyncCached(User user);

        Task<Bulletin?> GetBulletinInfoAsyncCached(User? user, Bulletin bulletin);
        Task<Bulletin?> GetBulletinAsync(User? user, Bulletin bulletin);

        Task<bool> RemoveBulletinAsync(Bulletin bulletin);
        Task<bool> UpdateBulletinAsync(Bulletin bulletin);
        Task<bool> PinBulletin(Bulletin bulletin);
    }
}
