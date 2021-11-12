using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BulletinBoard.Model
{
    public class UserRole : IdentityUserRole<ulong>
    {
        public virtual Group? Group { get; set; }
        [ForeignKey("Group")]
        public ulong? GroupId { get; set; }

    }
}
