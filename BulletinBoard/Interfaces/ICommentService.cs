using BulletinBoard.Model;

namespace BulletinBoard.Interfaces
{
    public interface ICommentService
    {
        Task<bool> AddCommentAsync(Comment Comment);
        Task<IList<Comment>> GetCommentsAsyncCached(Bulletin bulletin);
    }
}
