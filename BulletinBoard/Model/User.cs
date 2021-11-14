using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BulletinBoard.Model
{
    public class User : IdentityUser<ulong>
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? PostCode { get; set; }
        public DateTime Joined { get; }
        public virtual IList<Group>? Groups { get; set; }

        public virtual Image? Image { get; set; }
        [ForeignKey("Image")]
        public Guid? ImageId { get; set; }
        public User()
        {
            Joined = DateTime.UtcNow;
        }
    }
}
