using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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