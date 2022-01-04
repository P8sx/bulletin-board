using BulletinBoard.Data;
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
        public async Task<bool> AddCommentAsync(Comment Comment)
        {
            try
            {
                _dbContext.Comments.Add(Comment);
                await _dbContext.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
                  
        }
        public async Task<IList<Comment>> GetCommentsAsyncCached(Bulletin bulletin)
        {
            var result = await _memoryCache.GetOrCreateAsync($"Comments{bulletin.Id}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return await GetCommentsAsync(bulletin);
            });
            return result;
        }
        private async Task<IList<Comment>> GetCommentsAsync(Bulletin bulletin)
        {
            var comments = _dbContext.Comments
                .Where(c => c.BulletinId == bulletin.Id)
                .Include(c=>c.User)
                .OrderByDescending(c=>c.Created)
                .Select(c => new Comment
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
