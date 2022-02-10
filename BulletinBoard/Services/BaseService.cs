using BulletinBoard.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public abstract class BaseService
    {
        protected readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        protected readonly ILogger _logger;
        protected readonly IMemoryCache _memoryCache;
        protected readonly GlobalService GlobalService;

        protected BaseService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger logger, IMemoryCache memoryCache, GlobalService globalService)
        {
            _dbFactory = dbFactory;
            _logger = logger;
            _memoryCache = memoryCache;
            GlobalService = globalService;
        }
    }
}
