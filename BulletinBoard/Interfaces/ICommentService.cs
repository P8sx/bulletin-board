using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface ICommentService
    {
        Task<bool> AddCommentAsync(Comment Comment);
        Task<IList<Comment>> GetCommentsAsyncCached(Bulletin bulletin);
    }
}
