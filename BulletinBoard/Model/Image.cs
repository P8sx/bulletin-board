using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public class Image
    {
        [Key]
        public Guid Id { get; set; }
        public string Extension { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.UtcNow; 
        public virtual User? User { get; set; }
        [ForeignKey("User")]
        public ulong? UserId { get; set; }

        public virtual Group? Group { get; set; }
        [ForeignKey("Group")]
        public ulong? GroupId { get; set; }

        public string GetPath()
        {
            if(GroupId == null)
            {
                return $"/images/public/{Id}.{Extension}";
            }
            else
            {
                return $"/images/group/{GroupId}/{Id}.{Extension}";
            }
        }
        public Image()
        {
            Id = Guid.NewGuid(); 
        }
        public void SetExtension(string fileName)
        {
            Extension = fileName.Split('.').Last();
        }
        public Image(Guid id)
        {
            Id= id; 
        }

    }
}
