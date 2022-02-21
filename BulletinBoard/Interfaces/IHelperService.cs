using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IHelperService
    {
        Task AddToDefaultGroupAsync(User user);
        Task Report(Violation violation);

        Task<List<Violation>> GetViolations(int page = 1, int limit = 20,
            ViolationSortBy sortBy = ViolationSortBy.Bulletin);

        Task<int> GetViolationsCount();
        Task<bool> RemoveViolation(Violation violation);
        Task<bool> DeleteViolation(Violation violation);
        Task<bool> BanUser(Ban ban, Violation violation);
        Task<IEnumerable<User>> GetUsers();
    }
}
