using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public class CommentService : BaseService, ICommentService
    {

        public CommentService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<CommentService> logger, IMemoryCache memoryCache, GlobalService globalService) : base(dbFactory, logger, memoryCache,globalService)
        {

        }
        public async Task<bool> AddCommentAsync(Comment comment)
        {
            try
            {
                await using var dbContext = await _dbFactory.CreateDbContextAsync();
                dbContext.Comments.Add(comment);
                await dbContext.SaveChangesAsync();
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
            var result = await _memoryCache.GetOrCreateAsync($"Comments{bulletin.Guid}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return await GetCommentsAsync(bulletin);
            });
            return result;
        }
        private async Task<IList<Comment>> GetCommentsAsync(Bulletin bulletin)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var comments = dbContext.Comments
                .Where(c => c.BulletinId == bulletin.Id)
                .Include(c => c.User)
                .OrderByDescending(c => c.Created)
                .Select(c => new Comment
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Created = c.Created,
                    Text = c.Text,
                    User = c.User
                }).ToListAsync();
            return await comments;
        }

        public async Task<bool> RemoveComment(Comment comment)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var dbComment = await dbContext.Comments.Where(c => c.Id == comment.Id).FirstOrDefaultAsync();
            if (dbComment == null) return false;
            dbContext.Remove(dbComment);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
