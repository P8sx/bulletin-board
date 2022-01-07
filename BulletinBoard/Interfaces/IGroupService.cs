using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IGroupService
    {
        Task<bool> AddGroup(Group group);
        Task<bool> AddGroupAdmin(Group group, User user);
        Task<Group?> GetGroupInfoAsyncCached(Group group);
    }
}
