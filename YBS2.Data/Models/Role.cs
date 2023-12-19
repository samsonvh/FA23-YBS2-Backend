using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EnumRoleStatus Status { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}