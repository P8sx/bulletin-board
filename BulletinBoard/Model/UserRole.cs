using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Model;

public class UserRole : IdentityUserRole<ulong>
{
    public virtual User User { get; set; }  
    public virtual Role Role { get; set; }
}