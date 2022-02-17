using BulletinBoard.Model;

namespace BulletinBoard.Services
{
    public interface IHelperService
    {
        Task AddToDefaultGroupAsync(User user);
        Task Report(Violation violation);
        Task<List<Violation>> GetViolations(int page = 1, int limit = 20);
    }
}
