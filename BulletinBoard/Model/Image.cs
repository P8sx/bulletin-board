using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BulletinBoard.Extensions.ExtensionsMethod;

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

        public string GetFullName() => $"{Id}.{Extension}";
        public string GetSubFolder()
        {
            var subFolder = $"{Created:yyyyMMdd}";
            return subFolder;
        }
        public string Path() => $"{Consts.DefaultImageFolder}/{GetSubFolder()}/{GetFullName()}";

        public Image SetExtension(string fileName)
        {
            Extension = fileName.Split('.').Last();
            return this;
        }
        public Image()
        {
            Id = Guid.NewGuid();
        }
        public Image(string file)
        {
            Id = Guid.NewGuid();
            Extension = file.Split('.').Last();
            OrginalName = file.Split('.').First();
        }
        public Image(Guid id)
        {
            Id = id;
        }

    }
}
