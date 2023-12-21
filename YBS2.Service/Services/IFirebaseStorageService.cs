using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS2.Service.Services
{
    public interface IFirebaseStorageService
    {
        Task<Uri> UploadFile(string name, IFormFile file);
    }
}
