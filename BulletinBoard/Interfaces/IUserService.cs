using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IUserService
    {
        User? User { get; }
        List<Group>? UserGroups { get; }
        List<GroupUser>? UserGroupsRoles { get; }

        Task Bookmark(BulletinBookmark bookmark);
        bool IsInGroup(long groupId);
        bool IsInGroup(Group group);
        void UpdateUserGroups();
        Task Bookmark(Bulletin bulletin);
        Task Vote(Bulletin bulletin);
    }
}
