﻿using BulletinBoard.Model;
using System.Collections.Concurrent;

namespace BulletinBoard.Services
{
    public class ValidatorService : IValidatorService
    {
        private readonly ILogger<ValidatorService> _logger;
        private ConcurrentDictionary<ulong, bool> _rolesValidators = new();
        public ValidatorService(ILogger<ValidatorService> logger)
        {
            _logger = logger;
        }
        public void InvalidateUserRoles(User user)
        {
            _rolesValidators.TryAdd(user.Id, true);
        }
        public bool CheckValidRoles(User user)
        {
            if (_rolesValidators == null || user == null) return false;
            if (_rolesValidators.TryGetValue(user.Id, out _))
            {
                _rolesValidators.TryRemove(user.Id, out _);
                return false;
            }
            return true;
        }
    }
}
