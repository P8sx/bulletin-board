using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Model
{
    public enum RoleValue
    {
        User = 1,
        Moderator = 2,
        Admin = 3,
        Banned = 4
    }
    public class Role : IdentityRole<ulong>
    {
        public RoleValue RoleValue { get; set; }

        public Role(string name) : base(name)
        {
            if (Enum.TryParse(name, out RoleValue roleValue))
            {
                RoleValue = roleValue;
                NormalizedName = name.ToUpper();
                Id = Convert.ToUInt64(roleValue);
            }

        }
    }
}
