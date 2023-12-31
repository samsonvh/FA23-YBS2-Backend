using System.ComponentModel.DataAnnotations.Schema;
using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "MEMBER";
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(7);
        public Company? Company { get; set; }
        public Member? Member { get; set; }
        public EnumAccountStatus Status { get; set; }
        public ICollection<UpdateRequest>? UpdateRequests { get; set; } = null;
    }
}