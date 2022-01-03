using BulletinBoard.Data;
using BulletinBoard.DTOs;
using BulletinBoard.Interfaces;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;

        public CommentService(ApplicationDbContext dbContext, ILogger<BulletinService> logger, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _logger = logger;
            _memoryCache = memoryCache;
            _dbContext.Database.SetCommandTimeout(TimeSpan.FromSeconds(5));
        }
        public async Task<IList<CommentDTO>> GetCommentsAsyncCached(Guid BulletinId)
        {
            var result = await _memoryCache.GetOrCreateAsync($"Comments{BulletinId}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await GetCommentsAsync(BulletinId);
            });
            return result;
        }
        public async Task<bool> AddCommentAsync(Comment Comment)
        {
            return false;
        }
        private async Task<IList<CommentDTO>> GetCommentsAsync(Guid BulletinId)
        {
            var comments = _dbContext.Comments
                .Where(c => c.BulletinId == BulletinId)
                .Select(c => new CommentDTO
                {
                    Id = c.Id,
                    Created = c.Created,
                    Text = c.Text,
                    User = c.User
                }).ToListAsync(); ;
            return await comments;
        }
    }
}
