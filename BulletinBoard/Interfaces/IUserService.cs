using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IUserService
    {
        public Task<List<Group>> GetUserGroups(User user);
        public Task Bookmark(BulletinBookmark bookmark);
    }
}
