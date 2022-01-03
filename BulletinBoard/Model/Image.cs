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
        public DateTime Created { get; set; } = DateTime.UtcNow; 

        public Image()
        {
            Id = Guid.NewGuid();
        }
        public string GetPath()
        {          
            return $"/images/{Id}.{Extension}";        
        }
        public Image SetExtension(string fileName)
        {
            Extension = fileName.Split('.').Last();
            return this;
        }
        public Image(Guid id)
        {
            Id = id; 
        }

    }
}
