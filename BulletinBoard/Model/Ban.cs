using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.Model;

public class Ban
{
    [Key]
    public ulong Id { get; set; }
    [ForeignKey("User")]
    public ulong UserId { get; set; }
    public virtual User? User { get; set; }
    public string Reason { get; set; } = "";
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime BanDate { get; set; }
    
    public DateTime BanExpiringDate { get; set; }
}