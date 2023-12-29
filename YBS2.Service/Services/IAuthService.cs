using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;

namespace YBS2.Service.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginWithGoogle(string idToken);
        Task<AuthResponse?> LoginWithCredentials(CredentialsInputDto credentials);
    }
}