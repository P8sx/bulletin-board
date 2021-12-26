using BulletinBoard.Data;
using BulletinBoard.DTOs;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace BulletinBoard.Services
{
    public struct BulletinSort
    {
        public SortBy sortBy { get; set; } = SortBy.Created;
        public OrderBy orderBy { get; set; } = OrderBy.Ascending;
        
        public BulletinSort(SortBy sortBy, OrderBy orderBy)
        {
            this.orderBy = orderBy;
            this.sortBy = sortBy;
        }
    }
    public enum SortBy
    {
        Created,
        Popular,
        Commented,
        Expiring
    }
    public enum OrderBy
    {
        Ascending,
        Descending
    }

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
            _dbContext.Database.SetCommandTimeout(TimeSpan.FromSeconds(5));

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



        public async Task<IList<BulletinInfoDTO>> GetBulletinsAsyncCached(int page, int limit, User user, Group group, BulletinSort sort = default)
        {
            var result = await _memoryCache.GetOrCreateAsync($"Bulletins{page}{limit}{user.Id}{group.Id}{sort.orderBy}{sort.sortBy}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return await GetBulletinsAsync(page, limit, user, group, sort);
            });
            return result;
        }
        public async Task<IList<BulletinInfoDTO>> GetBulletinsAsyncCached(int page, int limit, User user, BulletinSort sort = default)
        {
            var result = await _memoryCache.GetOrCreateAsync($"Bulletins{page}{limit}{sort.orderBy}{sort.sortBy}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return await GetBulletinsAsync(page, limit, user, new Group() { Id = 1}, sort);
            });
            return result;
        }


        public async Task<int> GetBulletinsCountAsyncCached(User user, Group group)
        {
            var result = await _memoryCache.GetOrCreateAsync($"BulletinsCount{group.Id}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return await GetBulletinsCountAsync(user, group);
            });
            return result;
        }
        public async Task<int> GetBulletinsCountAsyncCached(User user)
        {
            var result = await _memoryCache.GetOrCreateAsync($"BulletinsCount", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return await GetBulletinsCountAsync(user, new Group() { Id = 1 });
            });
            return result;
        }


        private async Task<IList<BulletinInfoDTO>> GetBulletinsAsync(int page, int limit, User user, Group group, BulletinSort sort)
        {
            if (page == 0)
                page = 1;

            if (limit == 0)
                limit = int.MaxValue;

            var skip = (page - 1) * limit;

            var bulletins = _dbContext.Bulletins
                .Include(x => x.Images)
                .Include(u => u.User)
                .ThenInclude(i => i.Image)
                .Where(g => g.GroupId == group.Id)
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

            if (sort.sortBy == SortBy.Commented)
                bulletins = sort.orderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.VotesCount) : bulletins.OrderByDescending(d => d.VotesCount);
            else if (sort.sortBy == SortBy.Expiring)
                bulletins = sort.orderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.Expired) : bulletins.OrderByDescending(d => d.Expired);
            else if (sort.sortBy == SortBy.Commented)
                bulletins = sort.orderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.CommentsCount) : bulletins.OrderByDescending(d => d.CommentsCount);
            else
                bulletins = sort.orderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.Created) : bulletins.OrderByDescending(d => d.Created);


            return await bulletins.ToListAsync();
        }
        private async Task<int> GetBulletinsCountAsync(User user, Group group)
        {
            var bulletinsCount = await _dbContext.Bulletins
                .Include(x => x.Images)
                .Include(u => u.User)
                .ThenInclude(i => i.Image)
                .Where(g => g.GroupId == group.Id)
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
                }).CountAsync();

            return bulletinsCount;
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
