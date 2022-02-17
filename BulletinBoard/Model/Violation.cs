using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BulletinBoard.Model
{
    public class Violation
    {
        [Key]
        public ulong Id { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }
        public string Description { get; set; } = "";
        
        
        public virtual User? User { get; set; }
        [ForeignKey("User")]
        public ulong? UserId { get; set; }
        
        
        public virtual Board? Board { get; set; }
        [ForeignKey("Board")]
        public ulong? BoardId { get; set; }
        
        public virtual Bulletin? Bulletin { get; set; }
        [ForeignKey("Bulletin")]
        public ulong? BulletinId { get; set; }
        

        public virtual Comment? Comment { get; set; }
        [ForeignKey("Comment")]
        public ulong? CommentId { get; set; }
        
    }
}
