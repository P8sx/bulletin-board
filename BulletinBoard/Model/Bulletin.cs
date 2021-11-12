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
        public ulong Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Expired { get; set; }
        public List<Guid>? AttachmentFiles { get; set; }
        public bool? Pinned { get; set; }

        // Bulletin Creator
        public virtual User? User { get; set; }
        [ForeignKey("User")]
        public ulong UserId { get; set; }

        // Bulletin Group
        public virtual Group? Group { get; set; }
        [ForeignKey("Group")]
        public ulong? GroupId { get; set; }

        // Bulletin Users Vote
        public uint? UpVotes { get; set; } = 0;
        public uint? DownVotes { get; set; } = 0;

        // Bulletin Optional Location
        public float? Longitude { get; set; }
        public float? Latitude { get; set; }

        // Bulletin Comments
        public virtual IList<Comment>? Comments { get; set; }



    }
}
