using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IUserService
    {
        User? User { get; }

        bool IsInGroup(Group group);
        void UpdateUserGroups();
        Task Bookmark(Bulletin bulletin);
        Task Vote(Bulletin bulletin);
        bool IsGroupModerator(Group group);
        bool IsBulletinOwner(Bulletin bulletin);
        bool CanEditBulletin(Group group, Bulletin bulletin);
        List<Group?> GetUserGroups();
        List<Group?> GetUserPendingAcceptanceGroups();
        List<Group?> GetUserPendingInvitationsGroups();
        bool PendingAcceptance(Group group);
        bool IsGroupAdmin(Group group);
    }
}
