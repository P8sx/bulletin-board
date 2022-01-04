using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public class Violation
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }
        public string Description { get; set; } = "";
        public virtual User? User { get; set; }
        [ForeignKey("User")]
        public ulong? UserId { get; set; }
        public virtual Bulletin? Bulletin { get; set; }
        [ForeignKey("Bulletin")]
        public Guid? BulletinId { get; set; }

        public virtual Comment? Comment { get; set; }
        [ForeignKey("Comment")]
        public ulong? CommentId { get; set; }

        public Violation()
        {
            Created = DateTime.UtcNow;
        }
    }
}
