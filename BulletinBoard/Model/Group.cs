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
        public ulong Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; private set; }
        public bool? Public { get; set; }
        public virtual IList<User>? Users { get; set; }
        public virtual IList<Bulletin>? Bulletins { get; set; }
        public virtual IList<UserRole>? UserRoles { get; set; }
        public Group()
        {
            Created = DateTime.UtcNow;
        }
        public Group(ulong id)
        {
            Id = id;
            Created = DateTime.UtcNow;
        }

    }
}
