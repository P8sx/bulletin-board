using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public class Image
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(5)]
        public string Extension { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.UtcNow; 
        public virtual User? User { get; set; }
        [ForeignKey("User")]
        public ulong? UserId { get; set; }

        public virtual Group? Group { get; set; }
        [ForeignKey("Group")]
        public ulong? GroupId { get; set; }

        public Image()
        {
            Id = Guid.NewGuid();
        }
        public string GetPath()
        {
            if(GroupId == null)
            {
                return $"/images/avatars/{Id}.{Extension}";
            }
            else if(GroupId == 1)
            {
                return $"/images/public/{Id}.{Extension}";
            }
            else
            {
                return $"/images/group/{GroupId}/{Id}.{Extension}";
            }
        }
        public Image SetExtension(string fileName)
        {
            Extension = fileName.Split('.').Last();
            return this;
        }
        public Image SetGroup(ulong groupId)
        {
            GroupId = groupId;
            return this;
        }
        public Image SetGroup(Group group)
        {
            Group = group;
            GroupId = Group.Id;
            return this;
        }
        public Image SetUser(User user)
        {
            User = user;
            UserId = user.Id;
            return this;
        }
        public Image(Guid id)
        {
            Id = id; 
        }

    }
}
