using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public class Board
    {
        [Key]
        public Guid Id { get; init; }
        public DateTime Created { get; private set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool PublicListed { get; set; } = false;
        public bool AcceptAnyone { get; set; } = false;


        public virtual Image? Image { get; set; }
        [ForeignKey("Image")]
        public Guid? ImageId { get; set; }


        public virtual IList<BoardUser>? GroupUsers { get; set; }
        public virtual IList<Bulletin>? Bulletins { get; set; }
        public Board()
        {
            Created = DateTime.UtcNow;
        }
        public Board(Guid id)
        {
            Id = id;
            Created = DateTime.UtcNow;
        }

    }
}
