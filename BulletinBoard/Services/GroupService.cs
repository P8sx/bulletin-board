using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BulletinBoard.Services
{
    public class GroupService : BaseService, IGroupService
    {
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
        public async Task<List<Group>> GetPublicGroups()
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.Groups
                .Include(g => g.Image)
                .Where(g => g.PublicListed == true)
                .ToListAsync();
        }
        public async Task<List<GroupUser>> GetPendingApprovalUsers(Group group)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.GroupUsers
                .Include(gu => gu.User)
                .Include(gu => gu.Group)
                .Where(gu => gu.GroupId == group.Id)
                .Where(gu => gu.Role == GroupRole.PendingAcceptance)
                .ToListAsync();
        }
        public async Task<List<GroupUser>> GetInvitedUsers(Group group)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.GroupUsers
                .Include(gu => gu.User)
                .Include(gu => gu.Group)
                .Where(gu => gu.GroupId == group.Id)
                .Where(gu => gu.Role == GroupRole.Invited)
                .ToListAsync();
        }

        public async Task<List<GroupUser>> GetGroupUsers(Group group)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.GroupUsers
                .Include(gu => gu.User)
                .Include(gu => gu.Group)
                .Where(gu => gu.GroupId == group.Id)
                .Where(gu => gu.Role != GroupRole.PendingAcceptance)
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
        public async Task<GroupUser?> GetGroupUserAsync(Group group, User user)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.GroupUsers
                .Where(gu => gu.UserId == user.Id)
                .Where(gu => gu.GroupId == group.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AddGroup(Group group, User user)
        {
            try
            {
                using var _dbContext = _dbFactory.CreateDbContext();
                await _dbContext.Groups.AddAsync(group);
                await _dbContext.SaveChangesAsync();
                await SetGroupUser(group, user, GroupRole.Owner);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return false;
        }
        public async Task<bool> UpdateGroup(Group group)
        {
            try
            {
                using var _dbContext = _dbFactory.CreateDbContext();
                var dbGroup = await _dbContext.Groups.Where(g => g.Id == group.Id).FirstOrDefaultAsync();
                if (dbGroup == null) return false;
                _dbContext.Entry(dbGroup).CurrentValues.SetValues(group);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return false;
        }

        public async Task<bool> JoinToGroup(Group group, User user)
        {
            if (group.AcceptAnyone == true)
                return await SetGroupUser(group, user, GroupRole.User);
            return await SetGroupUser(group, user, GroupRole.PendingAcceptance);
        }
        public async Task<bool> CancelJoinToGroup(Group group, User user)
        {
            return await SetGroupUser(group, user, null);
        }
        public async Task<bool> RemoveGroupUser(Group group, User user)
        {
            return await SetGroupUser(group, user, null);
        }
        public async Task<bool> ChangeRole(Group group, User user, GroupRole role)
        {
            return await SetGroupUser(group, user, role);
        }
        public async Task<bool> AcceptUser(Group group, User user)
        {
            return await SetGroupUser(group, user, GroupRole.User);
        }
        public async Task<bool> RejectUser(Group group, User user)
        {
            return await SetGroupUser(group, user, null);
        }
        public async Task<bool> InviteUser(Group group, User user)
        {
            return await SetGroupUser(group, user, GroupRole.Invited);
        }
        public async Task<bool> AcceptInvitation(Group group, User user)
        {
            return await SetGroupUser(group, user, GroupRole.User);
        }

        public async Task<bool> CancelInviteUser(Group group, User user)
        {
            return await SetGroupUser(group, user, null);
        }

        private async Task<Group?> GetGroupAsync(Group group)
        {
            using var _dbContext = _dbFactory.CreateDbContext();
            return await _dbContext.Groups.Where(g => g.Id == group.Id).Include(g => g.Image).FirstOrDefaultAsync();
        }
        private async Task<bool> SetGroupUser(Group group, User user, GroupRole? role)  // if role equals null then remove role from db
        {
            try
            {
                using var _dbContext = _dbFactory.CreateDbContext();
                var result = await _dbContext.GroupUsers
                    .Where(gu => gu.GroupId == group.Id)
                    .Where(gu => gu.UserId == user.Id)
                    .FirstOrDefaultAsync();
                // if groupuser not found in db
                if (result == null)
                {
                    // if role null return true
                    if (role == null)
                        return true;
                    // else create new groupuser and assign role
                    var groupUser = new GroupUser()
                    {
                        GroupId = group.Id,
                        UserId = user.Id,
                        Role = role.Value,
                    };
                    await _dbContext.GroupUsers.AddAsync(groupUser);
                }
                // if groupuser found in db
                else
                {
                    // if role null remove grouprole from db
                    if (role == null)
                        _dbContext.GroupUsers.Remove(result);
                    // update groupuser role
                    else
                    {
                        result.Role = role.Value;
                        _dbContext.GroupUsers.Update(result);
                    }
                }
                _validatorService.InvalidateUserRoles(user);
                await _dbContext.SaveChangesAsync();
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
