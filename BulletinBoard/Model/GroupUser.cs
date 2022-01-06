using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
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

        public virtual Role? Role { get; set; }
        [ForeignKey("Role")]
        public ulong? RoleId { get; set; }

        public DateTime? Joined { get; set; }

        public GroupUser()
        {
            Joined = DateTime.UtcNow;
        }
    }
}
