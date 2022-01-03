using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BulletinBoard.Model
{
    public class Group
    {
        [Key]
        public Guid Id { get; set; } 
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; private set; }
        public bool? Public { get; set; }
        public virtual Image? Image { get; set; }
        [ForeignKey("Image")]
        public Guid? ImageId { get; set; }


        public virtual IList<GroupUser>? GroupUsers { get; set; }
        public virtual IList<Bulletin>? Bulletins { get; set; }
        public Group()
        {
            Id = Guid.NewGuid();
            Created = DateTime.UtcNow;
        }
        public Group(Guid id)
        {
            Id = id;
            Created = DateTime.UtcNow;
        }

    }
}
