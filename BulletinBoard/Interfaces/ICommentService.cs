using BulletinBoard.DTOs;
using BulletinBoard.Model;

namespace BulletinBoard.Interfaces
{
    public interface ICommentService
    {
        Task<bool> AddCommentAsync(Comment Comment);
        Task<IList<CommentDTO>> GetCommentsAsyncCached(Guid BulletinId);
    }
}
