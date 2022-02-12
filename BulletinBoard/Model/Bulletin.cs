using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public class Bulletin
    {
        [Key]
        public ulong Id { get; set; }
        public Guid Guid { get; set; }

        [StringLength(50, MinimumLength = 0, ErrorMessage = "Title must be at least 3 characters long (max 50)")]
        public string? Title { get; set; } = "";
        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be at least 10 characters long (max 1000)")]
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Expired { get; set; }
        public List<Image> Images { get; set; } = new();
        public bool? Pinned { get; set; } = false;
        public bool? Deleted { get; set; } = false;


        public virtual User User { get; set; }
        [ForeignKey("User")]
        public ulong UserId { get; set; }

        public virtual Board? Board { get; set; }
        [ForeignKey("Board")]
        public ulong? BoardId { get; set; }

        public virtual IList<Comment>? Comments { get; set; }
        public virtual IList<BulletinVote>? Votes { get; set; }
        public virtual IList<BulletinBookmark>? Bookmarks { get; set; }

        [NotMapped]
        public int CommentsCount { get; set; }
        [NotMapped]
        public int VotesCount { get; set; }
        [NotMapped]
        public bool UserVoted { get; set; }
        [NotMapped]
        public bool UserBookmark { get; set; }

        public Bulletin()
        {
            Created = DateTime.UtcNow;
            Guid = Guid.NewGuid();
        }
        public Bulletin(Guid guid)
        {
            Guid = guid;
            Created = DateTime.UtcNow;
        }
        public Bulletin(ulong id)
        {
            Id = id;
            Created = DateTime.UtcNow;
        }
    }
}
