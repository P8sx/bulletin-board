using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model
{
    public class Bulletin
    {
        [Key]
        public ulong Id { get; init; }
        public Guid Guid { get; init; }

        [StringLength(50, MinimumLength = 0, ErrorMessage = "Title max lenght is 50)")]
        public string? Title { get; set; } = "";

        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be at least 10 characters long (max 1000)")]
        public string Description { get; set; } = "";
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Expired { get; set; }
        public List<Image> Images { get; set; } = new();
        public bool Pinned { get; set; }
        public bool Deleted { get; set; }


        public virtual User? User { get; init; }
        [ForeignKey("User")]
        public ulong UserId { get; set; }

        public virtual Board? Board { get; init; }
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
            Guid = Guid.NewGuid();
        }
        public Bulletin(Guid guid)
        {
            Guid = guid;
        }
        public Bulletin(ulong id)
        {
            Id = id;
        }
        
    }
}
