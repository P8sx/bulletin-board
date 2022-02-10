using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public enum BoardRole
    {
        User = 1,
        Moderator = 2,
        Admin = 3,
        Invited = 4,
        PendingAcceptance = 5,
        Owner = 6
    }
    public class BoardUser
    {
        [Key]
        public ulong Id { get; set; }

        [ForeignKey("Board")]
        public ulong BoardId { get; set; }
        public virtual Board? Board { get; set; }
        [ForeignKey("User")]
        public ulong UserId { get; set; }
        public virtual User? User { get; set; }
        public BoardRole Role { get; set; }
        public DateTime? Joined { get; set; }

        public BoardUser()
        {
            Joined = DateTime.UtcNow;
        }
    }
}
