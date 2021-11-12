using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulletinBoard.Model
{
    public class User : IdentityUser<ulong>
    {
        public Guid? Avatar { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? PostCode { get; set; }
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public DateTime Joined { get; }
        public virtual IList<Group>? Groups { get; set; }

        public User()
        {
            Joined = DateTime.UtcNow;
        }
    }
}
