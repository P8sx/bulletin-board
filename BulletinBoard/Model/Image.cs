using BulletinBoard.Extensions;
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
        public string OrginalName { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [ForeignKey("Bulletin")]
        public Guid? BulletinId { get; set; }

        public string GetUserImagePath() => $"{Consts.DefaultAvatarFolder}/{GetFullName()}";
        public string GetBulletinImagePath(Guid groupId, Guid bulletinId) => $"{Consts.DefaultBulletinFolder}/{groupId}/{bulletinId}/{GetFullName()}";
        public string GetGroupImagePath(Guid groupId) => $"{Consts.DefaultGroupFolder}/{groupId}/{GetFullName()}";
        public string GetFullName() => $"{Id}.{Extension}";

        public Image SetExtension(string fileName)
        {
            Extension = fileName.Split('.').Last();
            return this;
        }
        public Image()
        {
            Id = Guid.NewGuid();
        }
        public Image(Guid id)
        {
            Id = id; 
        }

    }
}
