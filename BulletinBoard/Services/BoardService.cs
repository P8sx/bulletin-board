using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public class BoardService : BaseService, IBoardService
    {
        private readonly IValidatorService _validatorService;

        public BoardService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<BoardService> logger, IMemoryCache memoryCache, IValidatorService validatorService, GlobalService globalService) : base(dbFactory, logger, memoryCache, globalService)
        {
            _validatorService = validatorService;
        }
        
        public async Task<List<Board>> GetPublicBoard()
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Boards
                .Include(g => g.Image)
                .Where(g => g.PublicListed == true)
                .ToListAsync();
        }
        public async Task<List<BoardUser>> GetPendingApprovalUsers(Board board)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.BoardUsers
                .Include(gu => gu.User)
                .Include(gu => gu.Board)
                .Where(gu => gu.BoardId == board.Id || gu.Board!.Guid == board.Guid)
                .Where(gu => gu.Role == BoardRole.PendingAcceptance)
                .ToListAsync();
        }
        public async Task<List<BoardUser>> GetInvitedUsers(Board board)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.BoardUsers
                .Include(gu => gu.User)
                .Include(gu => gu.Board)
                .Where(gu => gu.BoardId == board.Id)
                .Where(gu => gu.Role == BoardRole.Invited)
                .ToListAsync();
        }

        public async Task<List<BoardUser>> GetBoardUsers(Board board)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.BoardUsers
                .Include(gu => gu.User)
                .Include(gu => gu.Board)
                .Where(gu => gu.BoardId == board.Id)
                .Where(gu => gu.Role != BoardRole.PendingAcceptance)
                .Where(gu => gu.Role != BoardRole.Invited)
                .Select(gu => new BoardUser()
                {
                    Id = gu.Id,
                    Board = gu.Board,
                    BoardId = gu.BoardId,
                    Role = gu.Role,
                    User = gu.User,
                    UserId = gu.UserId,
                })
                .ToListAsync();
        }
        public async Task<BoardUser?> GetBoardUserAsync(Board board, User user)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.BoardUsers
                .Where(gu => gu.UserId == user.Id)
                .Where(gu => gu.BoardId == board.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AddBoard(Board board, User user)
        {
            try
            {
                await using var dbContext = await _dbFactory.CreateDbContextAsync();
                await dbContext.Boards.AddAsync(board);
                await dbContext.SaveChangesAsync();
                await SetBoardUser(board, user, BoardRole.Owner);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return false;
        }
        public async Task<bool> UpdateBoard(Board board)
        {
            try
            {
                await using var dbContext = await _dbFactory.CreateDbContextAsync();
                var dbGroup = await dbContext.Boards.Where(g => g.Guid == board.Guid).FirstOrDefaultAsync();
                if (dbGroup == null) return false;
                dbContext.Entry(dbGroup).CurrentValues.SetValues(board);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return false;
        }
        public async Task<bool> JoinToBoard(Board board, User user)
        {
            if (board.AcceptAnyone)
                return await SetBoardUser(board, user, BoardRole.User);
            return await SetBoardUser(board, user, BoardRole.PendingAcceptance);
        }
        public async Task<bool> CancelJoinToBoard(Board board, User user)
        {
            return await SetBoardUser(board, user, null);
        }

        public async Task<bool> AcceptUser(Board board, User user)
        {
            return await SetBoardUser(board, user, BoardRole.User);
        }
        public async Task<bool> RejectUser(Board board, User user)
        {
            return await SetBoardUser(board, user, null);
        }

        public async Task<bool> ChangeRole(Board board, User user, BoardRole role)
        {
            return await SetBoardUser(board, user, role);
        }
        public async Task<bool> InviteUser(Board board, User user)
        {
            return await SetBoardUser(board, user, BoardRole.Invited);
        }
        public async Task<bool> RemoveBoardUser(Board board, User user)
        {
            return await SetBoardUser(board, user, null);
        }
       
        public async Task<bool> AcceptInvitation(Board board, User user)
        {
            return await SetBoardUser(board, user, BoardRole.User);
        }
        public async Task<bool> CancelInviteUser(Board board, User user)
        {
            return await SetBoardUser(board, user, null);
        }

        public async Task<Board?> GetBoardAsync(Board board)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Boards.Where(g => g.Id == board.Id || g.Guid == board.Guid).Include(g => g.Image).FirstOrDefaultAsync();
        }
        private async Task<bool> SetBoardUser(Board board, User user, BoardRole? role)  // if role equals null then remove role from db
        {
            try
            {
                await using var dbContext = await _dbFactory.CreateDbContextAsync();
                var result = await dbContext.BoardUsers
                    .Where(gu => gu.BoardId == board.Id)
                    .Where(gu => gu.UserId == user.Id)
                    .FirstOrDefaultAsync();
                // if boardUser not found in db
                if (result == null)
                {
                    // if role null return true
                    if (role == null)
                        return true;
                    // else create new boardUser and assign role
                    var groupUser = new BoardUser()
                    {
                        BoardId = board.Id,
                        UserId = user.Id,
                        Role = role.Value,
                    };
                    await dbContext.BoardUsers.AddAsync(groupUser);
                }
                // if boardUser found in db
                else
                {
                    // if role null remove boardUser from db
                    if (role == null)
                        dbContext.BoardUsers.Remove(result);
                    else if(role == BoardRole.Invited && result.Role == BoardRole.PendingAcceptance)
                        result.Role = BoardRole.User;
                    else
                        result.Role = role.Value;
                    dbContext.BoardUsers.Update(result);
                }
                _validatorService.InvalidateUserRoles(user);
                await dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }


    }
}
