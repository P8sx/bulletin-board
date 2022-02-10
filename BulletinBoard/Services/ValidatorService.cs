using BulletinBoard.Model;
using System.Collections.Concurrent;

namespace BulletinBoard.Services
{
    public class ValidatorService : IValidatorService
    {
        private readonly ILogger<ValidatorService> _logger;
        private readonly ConcurrentDictionary<ulong, bool> _rolesValidators = new();
        public ValidatorService(ILogger<ValidatorService> logger)
        {
            _logger = logger;
        }
        public void InvalidateUserRoles(User user)
        {
            _rolesValidators.TryAdd(user.Id, true);
        }
        public bool CheckValidRoles(User? user)
        {
            if (user == null) return false;
            if (!_rolesValidators.TryGetValue(user.Id, out _)) return true;
            _rolesValidators.TryRemove(user.Id, out _);
            return false;
        }
    }
}
