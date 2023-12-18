using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YBS2.Data.Enums;
using YBS2.Data.Models;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos.Input;
using YBS2.Service.Dtos.PageResponses;
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

        public async Task<AuthResponse> LoginWithEmailAndPassword(AuthenticateInputDto authenticateInputDto)
        {
            //validate email and password
            var existAccount = await _unitOfWork.AccountRepository.Find(account => account.Email == authenticateInputDto.Email)
                                                                    .Include(account => account.Role)
                                                                    .FirstOrDefaultAsync();
            if (existAccount == null)
            {
                throw new APIException( HttpStatusCode.BadRequest, "Invalid email or password");
            }
            var checkPassword = PasswordUtils.VerifyHashedPassword(existAccount.Password,authenticateInputDto.Password);
            if (!checkPassword)
            {
                throw new APIException( HttpStatusCode.BadRequest, "Invalid email or password");
            }
            var accessToken = JWTUtils.GenerateJWTToken(existAccount,_configuration);
            var authResponse = new AuthResponse()
            {
                AccessToken = accessToken,
                AccountId = existAccount.Id,
                Email = existAccount.Email,
                Role = existAccount.Role.Name,
                Username = existAccount.Username
            };
            return authResponse;
        }

        public async Task<AuthResponse> LoginWithGoogle(string idToken)
        {
            GoogleJsonWebSignature.Payload? payload = await JWTUtils.GetPayload(idToken, _configuration);
            if (payload == null)
            {
                throw new APIException( HttpStatusCode.BadRequest, "Can not get Google payload");
            }
            var existAccount = await _unitOfWork.AccountRepository.Find(account => account.Email.Trim().ToUpper() == payload.Email.Trim().ToUpper())
                                                                        .Include(account => account.Role)
                                                                        .FirstOrDefaultAsync();
            if (existAccount == null)
            {
                throw new APIException( HttpStatusCode.BadRequest, "You have not registered");
            }
            if (existAccount.Status == EnumAccountStatus.Ban)
            {
                throw new APIException( HttpStatusCode.BadRequest, "You can not login, your account is banned");
            }
            var accessToken = JWTUtils.GenerateJWTToken(existAccount,_configuration);
            var authResponse = new AuthResponse()
            {
                AccessToken = accessToken,
                AccountId = existAccount.Id,
                Email = existAccount.Email,
                Role = existAccount.Role.Name,
                Username = existAccount.Username
            };
            return authResponse;
        }

    }
}