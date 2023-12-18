using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Service.Dtos.Input;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginWithGoogle(string idToken);
        Task<AuthResponse> LoginWithEmailAndPassword(AuthenticateInputDto authenticateInputDto);
    }
}