using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using System.Net;
using YBS2.Data.Enums;
using YBS2.Data.Models;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos.Details;
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
        private readonly IVNPayService _vnpayService;
        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IVNPayService vNPayService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _vnpayService = vNPayService;
        }

        public async Task<AuthResponse> LoginWithCredentials(CredentialsInputDto credentials)
        {
            //validate email and password
            Account? existAccount = await _unitOfWork.AccountRepository
                .Find(account => account.Email.Trim().ToUpper() == credentials.Email.Trim().ToUpper() && account.Status == EnumAccountStatus.Active)
                .Include(account => account.Member)
                .Include(account => account.Company)
                .Include(account => account.Member.MembershipRegistrations)
                .FirstOrDefaultAsync();
            if (existAccount == null)
            {
                return null;
            }
            if (existAccount.Member != null && existAccount.Status == EnumAccountStatus.Inactive)
            {
                return new AuthResponse
                {
                    MembershipRegistrationId = existAccount.Member.MembershipRegistrations
                        .FirstOrDefault(membershipRegistration => membershipRegistration.Status == EnumMembershipRegistrationStatus.Inactive)
                        .Id,
                    IsInActive = true
                };
            }
            bool checkPassword = PasswordUtils.VerifyHashedPassword(existAccount.Password, credentials.Password);
            if (checkPassword)
            {
                if (existAccount.Status == EnumAccountStatus.Ban)
                {
                    dynamic errors = new ExpandoObject();
                    errors.Unauthorized = "Your account is banned";
                    throw new APIException(HttpStatusCode.Unauthorized, errors.Unauthorized, errors);
                }
                string accessToken = JWTUtils.GenerateJWTToken(existAccount, _configuration);
                return new AuthResponse()
                {
                    AccessToken = accessToken,
                    AccountId = existAccount.Id,
                    Email = existAccount.Email,
                    Role = existAccount.Role.ToUpper(),
                    Username = existAccount.Username,
                    IsInActive = false
                };
            }
            return null;

        }

        public async Task<AuthResponse> LoginWithGoogle(string idToken)
        {
            GoogleJsonWebSignature.Payload? payload = await JWTUtils.GetPayload(idToken, _configuration);
            if (payload == null)
            {
                dynamic errors = new ExpandoObject();
                errors.IdToken = "Invalid IdToken";
                throw new APIException(HttpStatusCode.BadRequest, errors.IdToken, errors);
            }

            var existAccount = await _unitOfWork.AccountRepository
                .Find(account => account.Email.Trim().ToUpper() == payload.Email.Trim().ToUpper() && account.Status == EnumAccountStatus.Active)
                .Include(account => account.Member)
                .Include(account => account.Company)
                .FirstOrDefaultAsync();
            if (existAccount == null)
            {
                return null;
            }

            if (existAccount.Member != null && existAccount.Status == EnumAccountStatus.Inactive)
            {
                return new AuthResponse
                {
                    MembershipRegistrationId = existAccount.Member.MembershipRegistrations
                        .FirstOrDefault(membershipRegistration => membershipRegistration.Status == EnumMembershipRegistrationStatus.Inactive)
                        .Id,
                    IsInActive = true
                };
            }
            if (existAccount.Status == EnumAccountStatus.Ban)
            {
                dynamic errors = new ExpandoObject();
                errors.Unauthorized = "Unauthorized";
                throw new APIException(HttpStatusCode.Unauthorized, errors.Unauthorized, errors);
            }
            var accessToken = JWTUtils.GenerateJWTToken(existAccount, _configuration);
            return new AuthResponse()
            {
                AccessToken = accessToken,
                AccountId = existAccount.Id,
                Email = existAccount.Email,
                Role = existAccount.Role.ToUpper(),
                Username = existAccount.Username,
                IsInActive = false
            };

        }

    }
}