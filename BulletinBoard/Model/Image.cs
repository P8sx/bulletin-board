using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BulletinBoard.Services.GlobalService;

namespace BulletinBoard.Model
{

    public class Image
    {
        [Key] public Guid Id { get; set; }
        [MaxLength(5)] private string Extension { get; set; } = "";
        public string OriginalName { get; set; } = "";
        private DateTime Created { get; set; } = DateTime.UtcNow;

        [ForeignKey("Bulletin")] public Guid? BulletinId { get; set; }
        
        
        public string FileName() => $"{Id}.{Extension}";

        public Image(string file)
        {
            Id = Guid.NewGuid();
            Extension = file.Split('.').Last();
            OriginalName = file.Split('.').First();
        }
        public Image(Guid id)
        {
            Id = id;
        }

    }
}
