using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IUserService
    {
        Task<List<Group>> GetUserGroups(User user);
        Task Bookmark(BulletinBookmark bookmark);
        Task<List<GroupUser>> GetUserGroupsRolesAsync(User user);
    }
}
