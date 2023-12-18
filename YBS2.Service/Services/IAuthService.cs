using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Service.Dtos.Input;
using static YBS2.Service.Dtos.PageResponses.DefaultAPIResponse;

namespace YBS2.Service.Services
{
    public interface IAuthService
    {
        Task<Response> LoginWithGoogle(string idToken);
        Task<Response> LoginWithEmailAndPassword(AuthenticateInputDto authenticateInputDto);
    }
}