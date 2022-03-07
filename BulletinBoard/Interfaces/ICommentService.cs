using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface ICommentService
    {
        Task<bool> RemoveComment(Comment comment);
        Task<bool> AddCommentAsync(Comment comment);
        Task<IList<Comment>> GetCommentsAsyncCached(Bulletin bulletin, bool forceReload = false);
    }
}
