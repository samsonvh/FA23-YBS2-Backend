namespace YBS2.Service.Dtos.Listings
{
    public class MemberListingDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
    }
}