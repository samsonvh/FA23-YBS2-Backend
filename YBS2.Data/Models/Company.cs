using System.ComponentModel.DataAnnotations.Schema;

namespace YBS2.Data.Models
{
    public class Company
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string HotLine { get; set; }
        public string FacebookURL { get; set; }
        public string LinkedlnURL { get; set; }
        public string InstagramURL { get; set; }
        public string LogoURL { get; set; }
    }
}