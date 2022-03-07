using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public class User : IdentityUser<ulong>
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? PostCode { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Joined { get; set; }
        public virtual IList<BoardUser>? BoardUsers { get; set; }

        public virtual Image? Image { get; set; }
        [ForeignKey("Image")]
        public ulong? ImageId { get; set; }

        public virtual IList<BulletinVote>? Votes { get; set; }
        public virtual IList<BulletinBookmark>? Bookmarks { get; set; }
        public virtual IList<Role> Roles { get; set; }
        [NotMapped]
        public IEnumerable<string> RolesName { get; set; } 
    }
}
