using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IGroupService
    {
        Task<Group> GetDefaultGroupAsyncCached();
        Task<Group> GetGroupAsyncCached(ulong groupId);
    }
}
