using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;

namespace YBS2.Service.Services
{
    public interface IAuthService
    {
        Task<object> LoginWithGoogle(string idToken);
        Task<object> LoginWithCredentials(CredentialsInputDto credentials);
    }
}