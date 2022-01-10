using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public class Group
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; private set; }
        public bool PublicListed { get; set; } = false;
        public bool AcceptAnyone { get; set; } = false;

        public virtual Image? Image { get; set; }
        [ForeignKey("Image")]
        public Guid? ImageId { get; set; }


        public virtual IList<GroupUser>? GroupUsers { get; set; }
        public virtual IList<Bulletin>? Bulletins { get; set; }
        public Group()
        {
            Created = DateTime.UtcNow;
        }
        public Group(Guid id)
        {
            Id = id;
            Created = DateTime.UtcNow;
        }

    }
}
