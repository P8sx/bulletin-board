using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public class Comment
    {
        [Key]
        public ulong Id { get; set; }
        public virtual User? User { get; set; }
        [ForeignKey("User")]
        public ulong? UserId { get; set; }
        public virtual Bulletin? Bulletin { get; set; }
        [ForeignKey("Bulletin")]
        public ulong? BulletinId { get; set; }

        public string? Text { get; set; }
        public DateTime Created { get; set; }
        public Comment()
        {
            Created = DateTime.Now;
        }
    }
}
