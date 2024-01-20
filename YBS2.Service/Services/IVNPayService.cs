using Microsoft.AspNetCore.Http;
using YBS2.Data.Models;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services
{
    public interface IVNPayService
    {
        Task<string> CreateRegisterRequestURL(MembershipRegistration membershipRegistration, MembershipPackage membershipPackage, HttpContext context);
        Task<VNPayRegisterResponse> CallBackRegisterPayment(IQueryCollection collections);
        Task<string> CreateBookingRequestURL(Guid bookingId, HttpContext context);
        Task<VNPayBookingResponse> CallBackBookingPayment(IQueryCollection collections);
        Task<string> CreateExtendMembershipRequestURL(Member member, MembershipPackage membershipPackage, HttpContext context);
        Task<VNPayExtendMembershipResponse> CallBackExtendMembership(IQueryCollection collections);
    }
}