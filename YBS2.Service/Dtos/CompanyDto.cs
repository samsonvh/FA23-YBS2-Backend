﻿namespace YBS2.Service.Dtos
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string HotLine { get; set; }
        public string? FacebookURL { get; set; } = null;
        public string? LinkedInURL { get; set; } = null;
        public string? InstagramURL { get; set; } = null;
        public string? LogoURL { get; set; } = null;
        public string Status { get; set; }
    }
}
