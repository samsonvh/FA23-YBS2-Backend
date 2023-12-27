using Microsoft.AspNetCore.Http;
using YBS2.Data.Models;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services
{
    public interface IVNPayService
    {
        Task<string> CreateRegisterRequestURL(Guid membershipPackageId, HttpContext context);
        Task<VNPayResponseModel> CallBackRegisterPayment(IQueryCollection collections);
    }
}