using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public class BulletinVote
    {
        public virtual User? User { get; set; }
        [ForeignKey("User")]
        public ulong UserId { get; set; }
        public virtual Bulletin? Bulletin { get; set; }
        [ForeignKey("Bulletin")]
        public Guid BulletinId { get; set; }
    }
}
