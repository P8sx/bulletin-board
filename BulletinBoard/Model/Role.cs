using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulletinBoard.Model
{
    public enum RoleValue
    {
        User,
        Moderator,
        Admin,
        GroupInvited,
        GroupUser,
        GroupModerator,
        GroupAdmin,
    }
    public class Role : IdentityRole<ulong>
    {
        public RoleValue RoleValue { get; set; }
        public Role()
        {
        }

        public Role(string name, RoleValue roleValue) : base(name)
        {
            RoleValue = roleValue;
        }
    }
}
