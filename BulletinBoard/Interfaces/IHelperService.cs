using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IHelperService
    {
        Task AddToDefaultGroupAsync(User user);
    }
}
