using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IGroupService
    {
        Task<bool> AddGroup(Group group);
        Task<bool> AddGroupAdmin(Group group, User user);
        Task<bool> CancelJoinToGroup(Group group, User user);
        Task<List<User?>> GetAwaitingAcceptanceUsers(Group group);
        Task<Group?> GetGroupInfoAsyncCached(Group group);
        Task<List<GroupUser>> GetGroupUsers(Group group);
        Task<List<Group>> GetPublicGroups();
        Task<bool> JoinToGroup(Group group, User user);
    }
}
