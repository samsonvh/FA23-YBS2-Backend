namespace YBS2.Service.Dtos.Details
{
    public class MemberDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string? AvatarURL { get; set; } = null;
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentityNumber { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public DateTime MemberSinceDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}