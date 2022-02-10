using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IBoardService
    {
        Task<bool> AddBoard(Board board, User user);
        Task<bool> CancelJoinToBoard(Board board, User user);
        Task<List<BoardUser>> GetPendingApprovalUsers(Board board);
        Task<Board?> GetBoardInfoAsyncCached(Board board);
        Task<List<BoardUser>> GetBoardUsers(Board board);
        Task<List<Board>> GetPublicBoard();
        Task<bool> JoinToBoard(Board board, User user);
        Task<bool> RemoveBoardUser(Board board, User user);
        Task<bool> ChangeRole(Board board, User user, BoardRole role);
        Task<bool> AcceptUser(Board board, User user);
        Task<bool> RejectUser(Board board, User user);
        Task<BoardUser?> GetBoardUserAsync(Board board, User user);
        Task<bool> CancelInviteUser(Board board, User user);
        Task<bool> InviteUser(Board board, User user);
        Task<List<BoardUser>> GetInvitedUsers(Board board);
        Task<bool> UpdateBoard(Board board);
        Task<bool> AcceptInvitation(Board board, User user);
    }
}
