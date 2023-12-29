namespace YBS2.Service.Dtos.Details
{
    public class UpdateRequestDto
    {
        public Guid Id { get; set; }
        public Guid ApproverId { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string HotLine { get; set; }
        public string? FacebookURL { get; set; } = null;
        public string? LinkedInURL { get; set; } = null;
        public string? InstagramURL { get; set; } = null;
        public string? LogoURL { get; set; } = null;
        public string? Comment { get; set; } = null;
        public DateTime CreatedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Status { get; set; }
    }
}
