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
using YBS2.Service.Utils;
using static YBS2.Service.Dtos.PageResponses.DefaultAPIResponse;

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

        public async Task<Response> LoginWithEmailAndPassword(AuthenticateInputDto authenticateInputDto)
        {
            //validate email and password
            var existAccount = await _unitOfWork.AccountRepository.Find(account => account.Email == authenticateInputDto.Email)
                                                                    .Include(account => account.Role)
                                                                    .FirstOrDefaultAsync();
            if (existAccount == null)
            {
                ErrorApiResult("Invalid email or password", HttpStatusCode.BadRequest);
            }
            var checkPassword = PasswordUtil.VerifyHashedPassword(existAccount.Password,authenticateInputDto.Password);
            if (!checkPassword)
            {
                ErrorApiResult("Invalid email or password",HttpStatusCode.BadRequest);
            }
            var accessToken = JWTUtil.GenerateJWTToken(existAccount,_configuration);
            var authResponse = new AuthResponse()
            {
                AccessToken = accessToken,
                AccountId = existAccount.Id,
                Email = existAccount.Email,
                Role = existAccount.Role.Name,
                Username = existAccount.Username
            };
            return Ok(authResponse,"Login Successfully");
        }

        public async Task<Response> LoginWithGoogle(string idToken)
        {
            GoogleJsonWebSignature.Payload? payload = await JWTUtil.GetPayload(idToken, _configuration);
            if (payload == null)
            {
                ErrorApiResult("Can not get Google payload",HttpStatusCode.BadRequest);
            }
            var existAccount = await _unitOfWork.AccountRepository.Find(account => account.Email.Trim().ToUpper() == payload.Email.Trim().ToUpper())
                                                                        .Include(account => account.Role)
                                                                        .FirstOrDefaultAsync();
            if (existAccount == null)
            {
                ErrorApiResult("You have not registered",HttpStatusCode.BadRequest);
            }
            if (existAccount.Status == EnumAccountStatus.Ban)
            {
                ErrorApiResult("You can not login, your account is banned",HttpStatusCode.BadRequest);
            }
            var accessToken = JWTUtil.GenerateJWTToken(existAccount,_configuration);
            var authResponse = new AuthResponse()
            {
                AccessToken = accessToken,
                AccountId = existAccount.Id,
                Email = existAccount.Email,
                Role = existAccount.Role.Name,
                Username = existAccount.Username
            };
            return Ok(authResponse,"Login Successfully");
        }

    }
}