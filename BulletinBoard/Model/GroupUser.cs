using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public enum GroupRole
    {
        User = 1,
        Moderator = 2,
        Admin = 3,
        Invited = 4,
        PendingAcceptance = 5,
    }
    public class GroupUser
    {
        [Key]
        public ulong Id { get; set; } 

        [ForeignKey("Group")]
        public Guid GroupId { get; set; }
        public virtual Group? Group { get; set; }
        [ForeignKey("User")]
        public ulong UserId { get; set; }
        public virtual User? User { get; set; }
        public GroupRole Role { get; set; }
        public DateTime? Joined { get; set; }

        public GroupUser()
        {
            Joined = DateTime.UtcNow;
        }
    }
}
