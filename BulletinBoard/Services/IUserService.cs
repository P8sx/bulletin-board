using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IUserService
    {
        Task<List<Group>> GetUserGroups(User user);
    }
}
