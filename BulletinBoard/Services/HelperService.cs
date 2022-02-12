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
    }
}
