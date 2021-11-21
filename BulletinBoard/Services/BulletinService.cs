using BulletinBoard.Data;
using BulletinBoard.DTOs;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public class BulletinService : IBulletinService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;

        public BulletinService(ApplicationDbContext dbContext, ILogger<BulletinService> logger, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<bool> AddBulletin(Bulletin bulletin)
        {
            try
            {
                await _dbContext.Bulletins.AddAsync(bulletin);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            return true;
        }


        public async Task<IList<BulletinInfoDTO>> GetBulletinsAsyncCached(int page, int limit, User user, ulong groupId = 1)
        {
            var result = await _memoryCache.GetOrCreateAsync($"Bulletins{page}{limit}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return await GetBulletinsAsync(page, limit, user ,groupId);
            });
            return result;
        }
        public async Task<IList<BulletinInfoDTO>> GetUserBulletinsAsyncCached(int page, int limit, User user)
        {
            var result = await _memoryCache.GetOrCreateAsync($"UserBulletins{page}{limit}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return await GetUserBulletinsAsync(page, limit, user);
            });
            return result;
        }
        public async Task<IList<BulletinInfoDTO>> GetUserBookmarkBulletinsAsyncCached(int page, int limit, User user)
        {
            var result = await _memoryCache.GetOrCreateAsync($"UserBulletins{page}{limit}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return await GetUserBookmarkBulletinsAsync(page, limit, user);
            });
            return result;
        }

        private async Task<IList<BulletinInfoDTO>> GetBulletinsAsync(int page, int limit,User user, ulong groupId = 1)
        {
            if (page == 0)
                page = 1;

            if (limit == 0)
                limit = int.MaxValue;

            var skip = (page - 1) * limit;

            var savedSearches = _dbContext.Bulletins
                .Include(x => x.Images)
                .Include(u => u.User)
                .ThenInclude(i => i.Image)
                .Where(g => g.GroupId == groupId)
                .Skip(skip)
                .Take(limit)
                .Select(a => new BulletinInfoDTO
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Created = a.Created,
                    Modified = a.Modified,
                    Expired = a.Expired,
                    Images = a.Images,
                    Pinned = a.Pinned,
                    User = a.User,
                    Group = a.Group,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    CommentsCount = Convert.ToUInt32(a.Comments.Count()),
                    VotesCount = Convert.ToUInt32(a.Votes.Count()),
                    UserVoted = user!=null && a.Votes.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1,
                    UserBookmark = user != null && a.Bookmarks.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1
                });
            return await savedSearches.ToListAsync();
        }
        private async Task<IList<BulletinInfoDTO>> GetUserBulletinsAsync(int page, int limit, User user)
        {
            if (page == 0)
                page = 1;

            if (limit == 0)
                limit = int.MaxValue;

            var skip = (page - 1) * limit;

            var savedSearches = _dbContext.Bulletins
                .Where(g => g.User == user)
                .Include(x => x.Images)
                .Include(u => u.User)
                .ThenInclude(i => i.Image)
                .Skip(skip)
                .Take(limit)
                .Select(a => new BulletinInfoDTO
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Created = a.Created,
                    Modified = a.Modified,
                    Expired = a.Expired,
                    Images = a.Images,
                    Pinned = a.Pinned,
                    User = a.User,
                    Group = a.Group,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    CommentsCount = Convert.ToUInt32(a.Comments.Count()),
                    VotesCount = Convert.ToUInt32(a.Votes.Count()),
                    UserVoted = user != null && a.Votes.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1,
                    UserBookmark = user != null && a.Bookmarks.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1
                });
            return await savedSearches.ToListAsync();
        }
        private async Task<IList<BulletinInfoDTO>> GetUserBookmarkBulletinsAsync(int page, int limit, User user)
        {
            if (page == 0)
                page = 1;

            if (limit == 0)
                limit = int.MaxValue;

            var skip = (page - 1) * limit;
            var userBookmarks = await _dbContext.BulletinBookmarks.Where(u => u.UserId == user.Id).ToListAsync();

            var savedSearches = _dbContext.Bulletins
                .Include(x => x.Images)
                .Include(u => u.User)
                .ThenInclude(i => i.Image)
                .Skip(skip)
                .Take(limit)
                .Select(a => new BulletinInfoDTO
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Created = a.Created,
                    Modified = a.Modified,
                    Expired = a.Expired,
                    Images = a.Images,
                    Pinned = a.Pinned,
                    User = a.User,
                    Group = a.Group,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    CommentsCount = Convert.ToUInt32(a.Comments.Count()),
                    VotesCount = Convert.ToUInt32(a.Votes.Count()),
                    UserVoted = user != null && a.Votes.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1,
                    UserBookmark = user != null && a.Bookmarks.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1
                });
            return await savedSearches.ToListAsync();
        }

        public async Task Vote(BulletinVote vote)
        {
            var exist = await _dbContext.BulletinsVotes.FirstOrDefaultAsync(v=> v.BulletinId == vote.BulletinId && v.UserId==vote.UserId);
            if (exist == default)
                await _dbContext.BulletinsVotes.AddAsync(vote);
            else
                _dbContext.BulletinsVotes.Remove(exist);

            await _dbContext.SaveChangesAsync();
        }

    }
}
