using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public class User : IdentityUser<ulong>
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? PostCode { get; set; }
        public DateTime Joined { get; }
        public virtual IList<GroupUser>? GroupUsers { get; set; }

        public virtual Image? Image { get; set; }
        [ForeignKey("Image")]
        public Guid? ImageId { get; set; }
        public User()
        {
            Joined = DateTime.UtcNow;
        }

        public virtual IList<BulletinVote>? Votes { get; set; }
        public virtual IList<BulletinBookmark>? Bookmarks { get; set; }

    }
}
