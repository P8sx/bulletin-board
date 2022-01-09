using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public class GroupService : BaseService, IGroupService
    {

        private readonly ILogger _logger;
        private readonly IValidatorService _validatorService;

        public GroupService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<ValidatorService> logger, IMemoryCache memoryCache, IValidatorService validatorService) : base(dbFactory, logger, memoryCache)
        {
            _validatorService = validatorService;
        }

        public async Task<Group?> GetGroupInfoAsyncCached(Group group)
        {
            var result = await _memoryCache.GetOrCreateAsync($"Group{group.Id}", async p =>
            {
                p.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20);
                return await GetGroupAsync(group);
            });
            return result;
        }
        private async Task<Group?> GetGroupAsync(Group group)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.Groups.Where(g => g.Id == group.Id).Include(g => g.Image).FirstOrDefaultAsync();
        }
        public async Task<bool> AddGroupAdmin(Group group, User user)
        {
            try
            {
                using var _dbContext = _dbFactory.CreateDbContext();
                var groupUser = new GroupUser()
                {
                    UserId = user.Id,
                    GroupId = group.Id,
                    Role= GroupRole.Admin,
                    Joined = DateTime.UtcNow,
                };
                await _dbContext.GroupUsers.AddAsync(groupUser);
                await _dbContext.SaveChangesAsync();
                _validatorService.InvalidateUserRoles(user);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return false;
        }
        public async Task<bool> AddGroup(Group group)
        {
            try
            {
                using var _dbContext = _dbFactory.CreateDbContext();
                await _dbContext.Groups.AddAsync(group);
                await _dbContext.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }         
            return false;
        }
        public async Task<List<Group>> GetPublicGroups()
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.Groups
                .Include(g=>g.Image)
                .Where(g => g.Public == true)
                .ToListAsync();
        }


        public async Task<bool> JoinToGroup(Group group,User user)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            var result = await _dbContext.GroupUsers
                .Where(gu => gu.UserId == user.Id)
                .Where(gu => gu.GroupId == group.Id)
                .Where(gu => gu.Role == GroupRole.AwaitingAcceptance)
                .FirstOrDefaultAsync();
            if (result != default) return false;
            var groupUser = new GroupUser() { GroupId = group.Id, UserId = user.Id, Role = GroupRole.AwaitingAcceptance };
            await _dbContext.GroupUsers.AddAsync(groupUser);
            await _dbContext.SaveChangesAsync();
            _validatorService.InvalidateUserRoles(user);
            return true;
        }
        public async Task<bool> CancelJoinToGroup(Group group, User user)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            var result = await _dbContext.GroupUsers
                .Where(gu => gu.UserId == user.Id)
                .Where(gu => gu.GroupId == group.Id)
                .Where(gu => gu.Role == GroupRole.AwaitingAcceptance)
                .FirstOrDefaultAsync();
            if (result == default) return false;
            _dbContext.GroupUsers.Remove(result);
            await _dbContext.SaveChangesAsync();
            _validatorService.InvalidateUserRoles(user);
            return true;
        }
        public async Task<bool> SetGroupUser(Group group, User user, GroupRole role)
        {
            try
            {
                using var _dbContext = _dbFactory.CreateDbContext();
                var result = await _dbContext.GroupUsers
                    .Where(gu => gu.GroupId == group.Id)
                    .Where(gu => gu.UserId == user.Id)
                    .FirstOrDefaultAsync();
                if (result == null)
                {
                    var groupUser = new GroupUser()
                    {
                        GroupId = group.Id,
                        UserId = user.Id,
                        Role = role,
                    };
                    await _dbContext.GroupUsers.AddAsync(groupUser);
                    await _dbContext.SaveChangesAsync();
                    _validatorService.InvalidateUserRoles(user);
                    return true;
                }
                result.Role = role;
                _dbContext.GroupUsers.Update(result);
                await _dbContext.SaveChangesAsync();
                _validatorService.InvalidateUserRoles(user);
                return true;
            }catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }
        public async Task<bool> RemoveGroupUser(GroupUser groupUser)
        {
            try
            {
                using var _dbContext = _dbFactory.CreateDbContext();
                _dbContext.GroupUsers.Remove(groupUser);
                await _dbContext.SaveChangesAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
            _validatorService.InvalidateUserRoles(groupUser.User!);
            return true;
        }


        public async Task<List<User?>> GetAwaitingAcceptanceUsers(Group group)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.GroupUsers
                .Include(gu => gu.User)
                .Include(gu => gu.Group)
                .Where(gu => gu.GroupId == group.Id)
                .Where(gu=>gu.Role == GroupRole.AwaitingAcceptance)
                .Select(gu => gu.User)
                .ToListAsync();
        }
        public async Task<List<GroupUser>> GetGroupUsers(Group _group)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.GroupUsers
                .Include(gu => gu.User)
                .Include(gu=>gu.Group)
                .Where(gu => gu.GroupId == _group.Id)
                .Where(gu => gu.Role != GroupRole.AwaitingAcceptance)
                .Where(gu => gu.Role != GroupRole.Invited)
                .Select(gu => new GroupUser()
                {
                    Id = gu.Id,
                    Group = gu.Group,
                    GroupId = gu.Group!.Id,
                    Role = gu.Role,
                    User = gu.User,
                    UserId = gu.UserId,
                })
                .ToListAsync();
        }
    }
}
