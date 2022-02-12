using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BulletinBoard.Services.GlobalService;

namespace BulletinBoard.Model
{

    public class Image
    {
        [Key]
        public ulong Id { get; set; }
        public Guid Guid { get; set; }
        [MaxLength(5)] 
        public string FileExtension { get; set; } = "";

        public string Name { get; set; } = "";
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        
        private DateTime Created { get; set; }

        [ForeignKey("Bulletin")] 
        public ulong? BulletinId { get; set; }
        
        public string FileName() => $"{Guid}.{FileExtension}";

        public Image(string file)
        {
            Guid = Guid.NewGuid();
            FileExtension = file.Split('.').Last();
            Name = file.Split('.').First();
        }
        public Image(Guid guid)
        {
            Guid = guid;
        }

    }
}
