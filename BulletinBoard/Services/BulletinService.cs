using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public struct BulletinSort
    {
        public SortBy SortBy { get; set; } = SortBy.Created;
        public OrderBy OrderBy { get; set; } = OrderBy.Ascending;

        public BulletinSort(SortBy sortBy, OrderBy orderBy)
        {
            this.OrderBy = orderBy;
            this.SortBy = sortBy;
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

    public class BulletinService : BaseService, IBulletinService
    {
        public BulletinService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<BulletinService> logger, IMemoryCache memoryCache, GlobalService globalService) : base(dbFactory, logger, memoryCache, globalService)
        {
        }

        // Request bulletin for specific boards
        public async Task<IList<Bulletin>> GetBulletinsAsyncCached(int page, int limit, User? user, Board board, BulletinSort sort = default)
        {
            var uId = user != null ? user.Id.ToString() : Guid.NewGuid().ToString();
            var result = await _memoryCache.GetOrCreateAsync($"Bulletins{page}{limit}{uId}{board.Id}{sort.OrderBy}{sort.SortBy}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
                return await GetBulletinsAsync(page, limit, user, board, sort);
            });
            return result;
        }
        private async Task<IList<Bulletin>> GetBulletinsAsync(int page, int limit, User? user, Board board, BulletinSort sort)
        {
            if (page == 0)
                page = 1;

            if (limit == 0)
                limit = int.MaxValue;

            var skip = (page - 1) * limit;



            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var bulletins = dbContext.Bulletins
                .Include(x => x.Images)
                .Include(u => u.User)
                .ThenInclude(i => i!.Image)
                .Where(g => g.BoardId == board.Id)
                .Where(b => b.Deleted == false)
                .Select(a => new Bulletin
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
                    UserId = a.UserId,
                    Board = a.Board,
                    BoardId = a.Id,
                    CommentsCount = a.Comments!.Count,
                    VotesCount = a.Votes!.Count,
                    UserVoted = user != null && a.Votes.Count(v => v.BulletinId == a.Id && v.UserId == user.Id) == 1,
                    UserBookmark = user != null && a.Bookmarks!.Count(v => v.BulletinId == a.Id && v.UserId == user.Id) == 1
                });

            switch (sort.SortBy)
            {
                case SortBy.Popular:
                    bulletins = sort.OrderBy == OrderBy.Ascending
                        ? bulletins.OrderBy(d => d.VotesCount)
                        : bulletins.OrderByDescending(d => d.VotesCount);
                    break;
                case SortBy.Expiring:
                    bulletins = sort.OrderBy == OrderBy.Ascending
                        ? bulletins.OrderBy(d => d.Expired)
                        : bulletins.OrderByDescending(d => d.Expired);
                    break;
                case SortBy.Commented:
                    bulletins = sort.OrderBy == OrderBy.Ascending
                        ? bulletins.OrderBy(d => d.CommentsCount)
                        : bulletins.OrderByDescending(d => d.CommentsCount);
                    break;
                case SortBy.Created:
                default:
                    bulletins = sort.OrderBy == OrderBy.Ascending
                        ? bulletins.OrderBy(d => d.Created)
                        : bulletins.OrderByDescending(d => d.Created);
                    break;
            }

            bulletins = bulletins
                .Skip(skip)
                .Take(limit);
            return await bulletins.ToListAsync();
        }

        public async Task<int> GetBulletinsCountAsyncCached(Board board)
        {
            var result = _memoryCache.GetOrCreateAsync($"BulletinsCount{board.Id}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
                return await GetBulletinsCountAsync(board);
            });
            return await result;
        }
        private async Task<int> GetBulletinsCountAsync(Board board)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Bulletins
                .Where(g => g.BoardId == board.Id)
                .Where(b => b.Deleted == false)
                .CountAsync();
        }

        // Request info for specific bulletin
        public async Task<Bulletin?> GetBulletinInfoAsyncCached(User? user, Bulletin bulletin)
        {
            var uId = user != null ? user.Id.ToString() : Guid.NewGuid().ToString();
            return await _memoryCache.GetOrCreateAsync($"Bulletin{uId}{bulletin.Id}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
                return await GetBulletinInfoAsync(user!, bulletin);
            });
        }
        public async Task<Bulletin?> GetBulletinInfoAsync(User? user, Bulletin bulletin)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Bulletins
            .Include(x => x.Images)
            .Include(u => u.User)
            .ThenInclude(i => i!.Image)
            .Where(b => b.Id == bulletin.Id)
            .Where(b => b.Deleted == false)
            .Select(a => new Bulletin
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
                UserId = a.UserId,
                Board = a.Board,
                BoardId = a.BoardId,
                CommentsCount = a.Comments!.Count,
                VotesCount = a.Votes!.Count,
                UserVoted = user != null && a.Votes.Count(v => v.BulletinId == a.Id && v.UserId == user.Id) == 1,
                UserBookmark = user != null && a.Bookmarks!.Count(v => v.BulletinId == a.Id && v.UserId == user.Id) == 1
            }).FirstOrDefaultAsync();
        }

        // Request bulletin for specific user
        public async Task<IList<Bulletin>> GetUserBulletinsAsyncCached(int page, int limit, User? user, BulletinSort sort = default)
        {
            var uId = user != null ? user.Id.ToString() : Guid.NewGuid().ToString();
            var result = await _memoryCache.GetOrCreateAsync($"UserBulletins{page}{limit}{uId}{sort.OrderBy}{sort.SortBy}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
                return await GetUserBulletinsAsync(page, limit, user, sort);
            });
            return result;
        }
        private async Task<IList<Bulletin>> GetUserBulletinsAsync(int page, int limit, User? user, BulletinSort sort)
        {
            if (page == 0)
                page = 1;

            if (limit == 0)
                limit = int.MaxValue;

            var skip = (page - 1) * limit;



            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var bulletins = dbContext.Bulletins
                .Include(x => x.Images)
                .Include(u => u.User)
                .ThenInclude(i => i!.Image)
                .Where(g => g.UserId == user!.Id)
                .Where(b => b.Deleted == false)
                .Select(a => new Bulletin
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
                    UserId = a.UserId,
                    Board = a.Board,
                    BoardId = a.Id,
                    CommentsCount = a.Comments!.Count,
                    VotesCount = a.Votes!.Count,
                    UserVoted = user != null && a.Votes.Count(v => v.BulletinId == a.Id && v.UserId == user.Id) == 1,
                    UserBookmark = user != null && a.Bookmarks!.Count(v => v.BulletinId == a.Id && v.UserId == user.Id) == 1
                });

            switch (sort.SortBy)
            {
                case SortBy.Popular:
                    bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.VotesCount) : bulletins.OrderByDescending(d => d.VotesCount);
                    break;
                case SortBy.Expiring:
                    bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.Expired) : bulletins.OrderByDescending(d => d.Expired);
                    break;
                case SortBy.Commented:
                    bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.CommentsCount) : bulletins.OrderByDescending(d => d.CommentsCount);
                    break;
                default:
                    bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.Created) : bulletins.OrderByDescending(d => d.Created);
                    break;
            }

            bulletins = bulletins
                .Skip(skip)
                .Take(limit);
            return await bulletins.ToListAsync();
        }

        public async Task<int> GetUserBulletinsCountAsyncCached(User user)
        {
            var result = _memoryCache.GetOrCreateAsync($"UserBulletinsCount{user.Id}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
                return await GetUserBulletinsCountAsync(user);
            });
            return await result;
        }
        private async Task<int> GetUserBulletinsCountAsync(User user)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Bulletins
                .Where(g => g.UserId == user.Id)
                .Where(b => b.Deleted == false)
                .CountAsync();
        }

        // Request bulletin for user bookmarks

        public async Task<IList<Bulletin>> GetUserBookmarkBulletinsAsyncCached(int page, int limit, User? user, BulletinSort sort = default)
        {
            var uId = user != null ? user.Id.ToString() : Guid.NewGuid().ToString();
            var result = await _memoryCache.GetOrCreateAsync($"UserBookmarkBulletins{page}{limit}{uId}{sort.OrderBy}{sort.SortBy}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
                return await GetUserBookmarkBulletinsAsync(page, limit, user, sort);
            });
            return result;
        }
        private async Task<IList<Bulletin>> GetUserBookmarkBulletinsAsync(int page, int limit, User? user, BulletinSort sort)
        {
            if (page == 0)
                page = 1;

            if (limit == 0)
                limit = int.MaxValue;

            var skip = (page - 1) * limit;

            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var bulletins = dbContext.Bulletins
                .Include(x => x.Images)
                .Include(u => u.User)
                .ThenInclude(i => i!.Image)
                .Where(b => b.Deleted == false)
                .Select(a => new Bulletin
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
                    UserId = a.UserId,
                    Board = a.Board,
                    BoardId = a.Id,
                    CommentsCount = a.Comments!.Count,
                    VotesCount = a.Votes!.Count,
                    UserVoted = user != null && a.Votes.Count(v => v.BulletinId == a.Id && v.UserId == user.Id) == 1,
                    UserBookmark = user != null && a.Bookmarks!.Count(v => v.BulletinId == a.Id && v.UserId == user.Id) == 1
                })
                .Where(a => a.UserBookmark == true);

            switch (sort.SortBy)
            {
                case SortBy.Popular:
                    bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.VotesCount) : bulletins.OrderByDescending(d => d.VotesCount);
                    break;
                case SortBy.Expiring:
                    bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.Expired) : bulletins.OrderByDescending(d => d.Expired);
                    break;
                case SortBy.Commented:
                    bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.CommentsCount) : bulletins.OrderByDescending(d => d.CommentsCount);
                    break;
                default:
                    bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.Created) : bulletins.OrderByDescending(d => d.Created);
                    break;
            }

            bulletins = bulletins
                .Skip(skip)
                .Take(limit);
            return await bulletins.ToListAsync();
        }

        public async Task<int> GetUserBookmarkBulletinsCountAsyncCached(User user)
        {
            var result = _memoryCache.GetOrCreateAsync($"UserBookmarkBulletinsCount{user.Id}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
                return await GetUserBookmarkBulletinsCountAsync(user);
            });
            return await result;
        }
        private async Task<int> GetUserBookmarkBulletinsCountAsync(User user)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.BulletinsBookmarks
                .Where(g => g.UserId == user.Id).Include(b => b.Bulletin)
                .Where(b => b.Bulletin!.Deleted == false)
                .CountAsync();
        }


        public async Task<bool> AddBulletin(Bulletin bulletin)
        {
            try
            {
                await using var dbContext = await _dbFactory.CreateDbContextAsync();
                await dbContext.Bulletins.AddAsync(bulletin);
                await dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
            return true;
        }
        public async Task<bool> UpdateBulletin(Bulletin bulletin)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            try
            {
                bulletin.Images.ForEach(i => i.BulletinId = bulletin.Id);
                var dbBulletin = await dbContext.Bulletins.Include(b => b.Images).Where(b => b.Id == bulletin.Id).FirstOrDefaultAsync();
                if (dbBulletin == null) return false;

                dbContext.Entry(dbBulletin).CurrentValues.SetValues(bulletin);
                await dbContext.SaveChangesAsync();

                List<Image> existingImages = new();
                existingImages.AddRange(dbBulletin.Images);

                foreach (var existingImage in existingImages.Where(existingImage => bulletin.Images.All(i => i.Id != existingImage.Id)))
                {
                    dbContext.Images.Remove(existingImage);
                }
                foreach (var image in bulletin.Images.Where(image => dbBulletin.Images.All(i => i.Id != image.Id)))
                {
                    dbContext.Images.Add(image);
                }

                await dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }

        }
        public async Task<bool> RemoveBulletin(Bulletin bulletin)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var dbBulletin = await dbContext.Bulletins
                .Where(b => b.Id == bulletin.Id)
                .Include(b => b.Images)
                .Include(b => b.Bookmarks)
                .Include(b => b.Comments)
                .Include(b => b.Votes)
                .FirstOrDefaultAsync();
            if (dbBulletin == default)
                return false;

            // Mark as deleted but don't delete
            dbBulletin.Deleted = true;
            dbContext.Bulletins.Update(dbBulletin);

            //_dbContext.Remove(dbBulletin);

            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
