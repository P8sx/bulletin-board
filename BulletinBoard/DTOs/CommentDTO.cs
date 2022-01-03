using BulletinBoard.Model;

namespace BulletinBoard.DTOs
{
    public class CommentDTO
    {
        public ulong Id { get; set; }
        public User? User { get; set; }
        public string? Text { get; set; }
        public DateTime Created { get; set; }
    }
}
