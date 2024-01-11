using Swashbuckle.AspNetCore.Annotations;
using YBS2.Service.Swaggers;

namespace YBS2.Service.Dtos.Details
{
    public class AuthResponse
    {
        public Guid? MembershipRegistrationId { get; set; }
        [SwaggerSchema(Description = "JWT Token")]
        public string? AccessToken { get; set; }
        public Guid? AccountId { get; set; }
        [SwaggerSchema(Description = "Role of user")]
        [SwaggerSchemaExample("Admin")]
        public string? Role { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool IsInActive { get; set; }
    }
}