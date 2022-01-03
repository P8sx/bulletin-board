using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BulletinBoard.Model
{
    public class Bulletin
    {
        // Bulletin Basic Info
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [StringLength(50,MinimumLength = 6, ErrorMessage = "Title must be at least 6 characters long (max 50)")]
        public string? Title { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 20, ErrorMessage = "Description must be at least 20 characters long (max 1000)")]
        public string? Description { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Expired { get; set; }
        public List<Image> Images { get; set; } = new();
        public bool? Pinned { get; set; }

        // Bulletin Creator
        public virtual User? User { get; set; }
        [ForeignKey("User")]
        public ulong UserId { get; set; }

        // Bulletin Group
        public virtual Group? Group { get; set; }
        [ForeignKey("Group")]
        public ulong? GroupId { get; set; }

        // Bulletin Optional Location
        public float? Longitude { get; set; } = 0;
        public float? Latitude { get; set; } = 0;

        // Bulletin Comments
        public virtual IList<Comment>? Comments { get; set; }

        // Bulletin Votes
        public virtual IList<BulletinVote>? Votes { get; set; }

        // Bulletin Bookmark
        public virtual IList<BulletinBookmark>? Bookmarks { get; set; }

        [NotMapped]
        public uint CommentsCount { get; set; }
        [NotMapped]
        public uint VotesCount { get; set; }
        [NotMapped]
        public bool UserVoted { get; set; }
        [NotMapped]
        public bool UserBookmark { get; set; }

    }
}
