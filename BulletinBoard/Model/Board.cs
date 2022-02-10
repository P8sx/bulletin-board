using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public class Board
    {
        [Key]
        public ulong Id { get; set; }
        public Guid Guid { get; init; }
        public DateTime Created { get; private set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool PublicListed { get; set; }
        public bool AcceptAnyone { get; set; }


        public virtual Image? Image { get; set; }
        [ForeignKey("Image")] 
        public ulong? ImageId { get; set; }


        public virtual IList<BoardUser>? GroupUsers { get; set; }
        public virtual IList<Bulletin>? Bulletins { get; set; }
        public Board()
        {
            Guid = Guid.NewGuid();
            Created = DateTime.UtcNow;
        }
        public Board(ulong id)
        {
            Id = id;
            Created = DateTime.UtcNow;
        }
        public Board(Guid guid)
        {
            Guid = guid;
            Created = DateTime.UtcNow;
        }
    }
}
