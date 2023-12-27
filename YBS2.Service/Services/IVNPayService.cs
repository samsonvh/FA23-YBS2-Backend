using Microsoft.AspNetCore.Http;
using YBS2.Data.Models;
using YBS2.Service.Dtos.Inputs;

namespace YBS2.Service.Services
{
    public interface IVNPayService
    {
        public Task<string> CreateRegisterRequestURL(MemberInputDto inputDto, MembershipPackage membershipPackage, HttpContext context, string baseUrl, string vnpHashSecret);
    }
}