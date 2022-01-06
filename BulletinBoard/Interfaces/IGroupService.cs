using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IGroupService
    {
        Task<Group?> GetGroupInfoAsyncCached(Group group);
    }
}
