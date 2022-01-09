using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IValidatorService
    {
        bool CheckValidRoles(User user);
        void InvalidateUserRoles(User user);
    }
}
