using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using YBS2.Data.Enums;
using YBS2.Data.Models;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Exceptions;
using YBS2.Service.Utils;

namespace YBS2.Service.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponse?> LoginWithCredentials(CredentialsInputDto credentials)
        {
            //validate email and password
            Account? existAccount = await _unitOfWork.AccountRepository
                .Find(account => account.Email == credentials.Email)
                .Include(account => account.Role)
                .FirstOrDefaultAsync();
            if (existAccount != null)
            {
                bool checkPassword = PasswordUtils.VerifyHashedPassword(existAccount.Password, credentials.Password);
                if (checkPassword)
                {
                    if (existAccount.Status == EnumAccountStatus.Ban)
                    {
                        throw new APIException(HttpStatusCode.OK, "Your account is banned");
                    }
                    string accessToken = JWTUtils.GenerateJWTToken(existAccount, _configuration);
                    return new AuthResponse()
                    {
                        AccessToken = accessToken,
                        AccountId = existAccount.Id,
                        Email = existAccount.Email,
                        Role = existAccount.Role.Name,
                        Username = existAccount.Username
                    };
                }
            }
            return null;
        }

        public async Task<AuthResponse?> LoginWithGoogle(string idToken)
        {
            GoogleJsonWebSignature.Payload? payload = await JWTUtils.GetPayload(idToken, _configuration);
            if (payload == null)
            {
                throw new APIException(HttpStatusCode.BadRequest, "Invalid IdToken");
            }
            var existAccount = await _unitOfWork.AccountRepository
                .Find(account => account.Email.Trim().ToUpper() == payload.Email.Trim().ToUpper())
                .Include(account => account.Role)
                .FirstOrDefaultAsync();
            if (existAccount != null)
            {
                if (existAccount.Status == EnumAccountStatus.Ban)
                {
                    throw new APIException(HttpStatusCode.OK, "Your account is banned");
                }
                var accessToken = JWTUtils.GenerateJWTToken(existAccount, _configuration);
                return new AuthResponse()
                {
                    AccessToken = accessToken,
                    AccountId = existAccount.Id,
                    Email = existAccount.Email,
                    Role = existAccount.Role.Name,
                    Username = existAccount.Username
                };
            }
            return null;
        }

    }
}