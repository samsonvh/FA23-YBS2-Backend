using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using YBS2.Data.Models;
using YBS2.Service.Exceptions;

namespace YBS2.Service.Utils
{
    public static class JWTUtils
    {
        public static string GenerateJWTToken(Account account, IConfiguration configuration)
        {
            var claims = new List<Claim>()
            {
                new Claim("Id", account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.Role),
            };
            //if (account.Role.Name == "Member")
            //{
            //    if (account.Member == null)
            //    {
            //        throw new APIException(HttpStatusCode.BadRequest, "Account doesn't have detail member information");
            //    }
            //    claims.Add(new Claim("MemberId", account.Member.Id.ToString()));
            //    //claims.Add(new Claim("MembershipPackageId", account.Member.MembershipRegistrations.LastOrDefault(memberRegistration => memberRegistration.MemberId == account.Member.Id).MembershipPackageId.ToString()));
            //}
            if (account.Role == "Company")
            {
                if (account.Company == null)
                {
                    throw new APIException(HttpStatusCode.BadRequest, "Account doesn't have detail company information");
                }
                claims.Add(new Claim("CompanyId", account.Company.Id.ToString()));
            }
            var issuer = configuration["JWT:Issuer"];
            var audience = configuration["JWT:Audience"];
            var secretKey = configuration["JWT:SecretKey"];
            var expires = configuration["JWT:expires"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
            issuer,
            audience,
            claims: claims,
            expires: DateTime.Now.AddMilliseconds(int.Parse(expires)),
            signingCredentials: signingCredentials
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }

        public static ClaimsPrincipal GetClaim(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            string accessToken = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accessTokenPrefix = "Bearer ";
            if (accessToken == null)
            {
                throw new APIException(HttpStatusCode.Unauthorized, "Unauthorized");
            }
            if (accessToken.Contains(accessTokenPrefix))
            {
                accessToken = accessToken.Substring(accessTokenPrefix.Length);
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:SecretKey"]));
            var issuer = configuration["JWT:Issuer"];
            var audience = configuration["JWT:Audience"];
            // Configure token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidIssuer = issuer,
                ValidAudience = audience,
            };

            // Decrypt the token and retrieve the claims
            ClaimsPrincipal claimsPrincipal;
            claimsPrincipal = tokenHandler.ValidateToken(accessToken.Trim(), tokenValidationParameters, out _);
            if (claimsPrincipal == null)
            {
                throw new APIException(HttpStatusCode.BadRequest, "Failed to decrypt/validate the JWT token");
            }
            return claimsPrincipal;
        }
        public static async Task<GoogleJsonWebSignature.Payload?> GetPayload(string idToken, IConfiguration configuration)
        {
            try
            {
                var audience = configuration["JWT:Google_Client_Id"];
                GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new string[]
                    {
                        audience
                    }
                };
                return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            }
            catch
            {
                return null;
            }
        }
    }
}