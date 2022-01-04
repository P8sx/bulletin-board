using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IUserService
    {
        User? User { get; }
        List<GroupUser>? UserGroups { get; }

        Task Bookmark(BulletinBookmark bookmark);
        bool IsInGroup(Group group);
        void UpdateUserGroups();
        Task Bookmark(Bulletin bulletin);
        Task Vote(Bulletin bulletin);
    }
}
