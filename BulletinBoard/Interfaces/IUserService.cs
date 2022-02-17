using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IUserService
    {
        User? User { get; }

        bool IsInBoard(Board board);
        void UpdateUserBoards();
        Task Bookmark(Bulletin bulletin);
        Task Vote(Bulletin bulletin);
        bool IsBoardModerator(Board board);
        bool IsBulletinOwner(Bulletin bulletin);
        List<Board?> GetUserBoards();
        List<Board?> GetUserPendingAcceptanceBoards();
        List<Board?> GetUserPendingInvitationsBoards();
        bool PendingAcceptance(Board board);
        bool IsBoardAdmin(Board board);
        Task<IEnumerable<User>> Search(string userName);
        Task<User?> GetUserInfoAsync(User user);
        bool IsBoardOwner(Board board);
        bool PendingInvitations(Board board);
        Task UpdateImage(Image image);
    }
}
