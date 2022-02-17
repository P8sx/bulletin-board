using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static BulletinBoard.Services.GlobalService;

namespace BulletinBoard.Services
{
    public class HelperService : BaseService, IHelperService
    {
        public HelperService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<HelperService> logger, IMemoryCache memoryCache, GlobalService globalService) : base(dbFactory, logger, memoryCache, globalService)
        {

        }
        public async Task AddToDefaultGroupAsync(User user)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var boardId = await dbContext.Boards.Where(b => b.Guid == GlobalService.DefaultBoardGuid).Select(b=>b.Id)
                .FirstOrDefaultAsync();
            await dbContext.BoardUsers.AddAsync(new BoardUser() { BoardId = boardId, Role = BoardRole.User, UserId = user.Id });
            await dbContext.SaveChangesAsync();
        }

        public async Task Report(Violation violation)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            await dbContext.Violations.AddAsync(violation);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Violation>> GetViolations(int page = 1, int limit = 20)
        {
            if (page == 0)
                page = 1;

            if (limit == 0)
                limit = int.MaxValue;

            var skip = (page - 1) * limit;
            
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Violations
                .Include(v => v.Board)
                .Include(v => v.Comment).ThenInclude(c=>c.User)
                .Include(v => v.Bulletin).ThenInclude(b=>b!.Images)
                .Include(v => v.Bulletin).ThenInclude(b=>b!.User)
                .Include(v=>v.User).ThenInclude(u=>u!.Image)
                .Skip(skip)
                .Take(limit)
                .ToListAsync();
        }
    }
}
