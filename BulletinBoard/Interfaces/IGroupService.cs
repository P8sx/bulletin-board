using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IGroupService
    {
        Task<bool> AddGroup(Group group, User user);
        Task<bool> CancelJoinToGroup(Group group, User user);
        Task<List<GroupUser>> GetPendingApprovalUsers(Group group);
        Task<Group?> GetGroupInfoAsyncCached(Group group);
        Task<List<GroupUser>> GetGroupUsers(Group group);
        Task<List<Group>> GetPublicGroups();
        Task<bool> JoinToGroup(Group group, User user);
        Task<bool> RemoveGroupUser(Group group, User user);
        Task<bool> ChangeRole(Group group, User user, GroupRole role);
        Task<bool> AcceptUser(Group group, User user);
        Task<bool> RejectUser(Group group, User user);
        Task<GroupUser?> GetGroupUserAsync(Group group, User user);
        Task<bool> CancelInviteUser(Group group, User user);
        Task<bool> InviteUser(Group group, User user);
        Task<List<GroupUser>> GetInvitedUsers(Group group);
    }
}
