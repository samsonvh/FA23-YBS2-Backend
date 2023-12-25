namespace YBS2.Service.Dtos.Listings
{
    public class CompanyListingDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Hotline { get; set; }
        public string LogoURL { get; set; }
        public string Status { get; set; }
    }
}