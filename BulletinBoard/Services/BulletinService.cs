﻿using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

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
        public BulletinService(IDbContextFactory<ApplicationDbContext> dbFactory,  ILogger<BulletinService> logger, IMemoryCache memoryCache ):base(dbFactory, logger, memoryCache)
        {
        }
        public async Task<bool> AddBulletin(Bulletin bulletin)
        {
            try
            {
                using var _dbContext = _dbFactory.CreateDbContext();
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

        // Request bulletin for specific group
        public async Task<IList<Bulletin>> GetBulletinsAsyncCached(int page, int limit, User user, Group group, BulletinSort sort = default)
        {
            var uId = user != null ? user.Id.ToString() : Guid.NewGuid().ToString();
            var result = await _memoryCache.GetOrCreateAsync($"Bulletins{page}{limit}{uId}{group.Id}{sort.OrderBy}{sort.SortBy}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
                return await GetBulletinsAsync(page, limit, user, group, sort);
            });
            return result;
        }
        private async Task<IList<Bulletin>> GetBulletinsAsync(int page, int limit, User? user, Group group, BulletinSort sort)
        {
            if (page == 0)
                page = 1;

            if (limit == 0)
                limit = int.MaxValue;

            var skip = (page - 1) * limit;



            using var _dbContext = _dbFactory.CreateDbContext();
            var bulletins = _dbContext.Bulletins
                .Include(x => x.Images)
                .Include(u => u.User)
                .ThenInclude(i => i!.Image)
                .Where(g => g.GroupId == group.Id)
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
                    Group = a.Group,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    CommentsCount = a.Comments!.Count,
                    VotesCount = a.Votes!.Count,
                    UserVoted = user != null && a.Votes.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1,
                    UserBookmark = user != null && a.Bookmarks!.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1
                });

            if (sort.SortBy == SortBy.Popular)
                bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.VotesCount) : bulletins.OrderByDescending(d => d.VotesCount);
            else if (sort.SortBy == SortBy.Expiring)
                bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.Expired) : bulletins.OrderByDescending(d => d.Expired);
            else if (sort.SortBy == SortBy.Commented)
                bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.CommentsCount) : bulletins.OrderByDescending(d => d.CommentsCount);
            else
                bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.Created) : bulletins.OrderByDescending(d => d.Created);

            bulletins = bulletins
                .Skip(skip)
                .Take(limit);
            return await bulletins.ToListAsync();
        }

        public async Task<int> GetBulletinsCountAsyncCached(Group group)
        {
            var result = _memoryCache.GetOrCreateAsync($"BulletinsCount{group.Id}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
                return await GetBulletinsCountAsync(group);
            });
            return await result;
        }
        private async Task<int> GetBulletinsCountAsync(Group group)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.Bulletins
                .Where(g => g.GroupId == group.Id)
                .CountAsync();
        }

        // Request info for specific bulletin
        public async Task<Bulletin?> GetBulletinInfoAsyncCached(User user, Group group, Bulletin bulletin)
        {
            var uId = user != null ? user.Id.ToString() : Guid.NewGuid().ToString();
            return await _memoryCache.GetOrCreateAsync($"Bulletin{uId}{group.Id}{bulletin.Id}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
                return await GetBulletinInfoAsync(user!, group, bulletin);
            });
        }
        private async Task<Bulletin?> GetBulletinInfoAsync(User user, Group group, Bulletin bulletin)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.Bulletins
            .Include(x => x.Images)
            .Include(u => u.User)
            .ThenInclude(i => i!.Image)
            .Where(g => g.GroupId == group.Id)
            .Where(b => b.Id == bulletin.Id)
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
                Group = a.Group,
                Latitude = a.Latitude,
                Longitude = a.Longitude,
                CommentsCount = a.Comments!.Count,
                VotesCount = a.Votes!.Count,
                UserVoted = user != null && a.Votes.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1,
                UserBookmark = user != null && a.Bookmarks!.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1
            }).FirstOrDefaultAsync();
        }

        // Request bulletin for specific user
        public async Task<IList<Bulletin>> GetUserBulletinsAsyncCached(int page, int limit, User user,  BulletinSort sort = default)
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



            using var _dbContext = _dbFactory.CreateDbContext();
            var bulletins = _dbContext.Bulletins
                .Include(x => x.Images)
                .Include(u => u.User)
                .ThenInclude(i => i!.Image)
                .Where(g => g.UserId == user!.Id)
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
                    Group = a.Group,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    CommentsCount = a.Comments!.Count,
                    VotesCount = a.Votes!.Count,
                    UserVoted = user != null && a.Votes.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1,
                    UserBookmark = user != null && a.Bookmarks!.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1
                });

            if (sort.SortBy == SortBy.Popular)
                bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.VotesCount) : bulletins.OrderByDescending(d => d.VotesCount);
            else if (sort.SortBy == SortBy.Expiring)
                bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.Expired) : bulletins.OrderByDescending(d => d.Expired);
            else if (sort.SortBy == SortBy.Commented)
                bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.CommentsCount) : bulletins.OrderByDescending(d => d.CommentsCount);
            else
                bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.Created) : bulletins.OrderByDescending(d => d.Created);

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
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.Bulletins
                .Where(g => g.UserId == user.Id)
                .CountAsync();
        }

        // Request bulletin for user bookmarks

        public async Task<IList<Bulletin>> GetUserBookmarkBulletinsAsyncCached(int page, int limit, User user, BulletinSort sort = default)
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

            using var _dbContext = _dbFactory.CreateDbContext();
            var bulletins = _dbContext.Bulletins
                .Include(x => x.Images)
                .Include(u => u.User)
                .ThenInclude(i => i!.Image)
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
                    Group = a.Group,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    CommentsCount = a.Comments!.Count,
                    VotesCount = a.Votes!.Count,
                    UserVoted = user != null && a.Votes.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1,
                    UserBookmark = user != null && a.Bookmarks!.Where(v => v.BulletinId == a.Id && v.UserId == user.Id).Count() == 1
                })
                .Where(a=>a.UserBookmark == true);

            if (sort.SortBy == SortBy.Popular)
                bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.VotesCount) : bulletins.OrderByDescending(d => d.VotesCount);
            else if (sort.SortBy == SortBy.Expiring)
                bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.Expired) : bulletins.OrderByDescending(d => d.Expired);
            else if (sort.SortBy == SortBy.Commented)
                bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.CommentsCount) : bulletins.OrderByDescending(d => d.CommentsCount);
            else
                bulletins = sort.OrderBy == OrderBy.Ascending ? bulletins.OrderBy(d => d.Created) : bulletins.OrderByDescending(d => d.Created);

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
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.BulletinBookmarks
                .Where(g => g.UserId == user.Id)
                .CountAsync();
        }
    }
}
