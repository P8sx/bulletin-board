using BulletinBoard.Model;

namespace BulletinBoard.DTOs
{
    public class BulletinInfoDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Expired { get; set; }
        public bool? IsExpired { get => DateTime.UtcNow - Expired > TimeSpan.Zero; }

        public List<Image> Images { get; set; } = new();
        public bool? Pinned { get; set; }
        public User? User { get; set; }
        public Group? Group { get; set; }

        // Bulletin Optional Location
        public float? Longitude { get; set; } = 0;
        public float? Latitude { get; set; } = 0;

        // Bulletin Comments
        public uint? CommentsCount { get; set; }   

        // Bulletin Votes
        public uint VotesCount { get; set; }
        public bool UserVoted { get; set; }
        public bool UserBookmark { get; set; }

    }
}
