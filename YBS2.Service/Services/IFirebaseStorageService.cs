using Microsoft.AspNetCore.Http;

namespace YBS2.Service.Services
{
    public interface IFirebaseStorageService
    {
        Task<Uri> UploadFile(string name, IFormFile file);
    }
}
