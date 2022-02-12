using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IBoardService
    {
        Task<bool> AddBoardAsync(Board board, User user);
        Task<bool> CancelJoinToBoardAsync(Board board, User user);
        Task<List<BoardUser>> GetPendingApprovalUsersAsync(Board board);
        Task<Board?> GetBoardAsync(Board board);
        Task<List<BoardUser>> GetBoardUsersAsync(Board board);
        Task<List<Board>> GetPublicBoardAsync();
        Task<bool> JoinToBoardAsync(Board board, User user);
        Task<bool> RemoveBoardUserAsync(Board board, User user);
        Task<bool> ChangeRoleAsync(Board board, User user, BoardRole role);
        Task<bool> AcceptUserAsync(Board board, User user);
        Task<bool> RejectUserAsync(Board board, User user);
        Task<BoardUser?> GetBoardUserAsync(Board board, User user);
        Task<bool> CancelInviteUserAsync(Board board, User user);
        Task<bool> InviteUserAsync(Board board, User user);
        Task<List<BoardUser>> GetInvitedUsersAsync(Board board);
        Task<bool> UpdateBoardAsync(Board board);
        Task<bool> AcceptInvitationAsync(Board board, User user);
    }
}
