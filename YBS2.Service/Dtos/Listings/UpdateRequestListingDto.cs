namespace YBS2.Service.Dtos.Listings
{
    public class UpdateRequestListingDto
    {
        public Guid Id { get; set; }
        public string LogoURL { get; set; }
        public string CompanyName { get; set; }
        public string ApproverUserName { get; set; }
        public string Status { get; set; }
    }
}
