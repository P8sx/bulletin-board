using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public class CommentService : BaseService, ICommentService
    {

        public CommentService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<CommentService> logger, IMemoryCache memoryCache) : base(dbFactory, logger, memoryCache)
        {

        }
        public async Task<bool> AddCommentAsync(Comment Comment)
        {
            try
            {
                using var _dbContext = _dbFactory.CreateDbContext();
                _dbContext.Comments.Add(Comment);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }

        }
        public async Task<IList<Comment>> GetCommentsAsyncCached(Bulletin bulletin, bool forceReload = false)
        {
            if (forceReload) return await GetCommentsAsync(bulletin);
            var result = await _memoryCache.GetOrCreateAsync($"Comments{bulletin.Id}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return await GetCommentsAsync(bulletin);
            });
            return result;
        }
        private async Task<IList<Comment>> GetCommentsAsync(Bulletin bulletin)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            var comments = _dbContext.Comments
                .Where(c => c.BulletinId == bulletin.Id)
                .Include(c => c.User)
                .OrderByDescending(c => c.Created)
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
