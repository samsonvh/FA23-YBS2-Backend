using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS2.Service.Services.Implements
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly StorageClient _storageClient;
        private const string BUCKET_NAME = "yacht-booking-system-2.appspot.com";

        public FirebaseStorageService(StorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<Uri> UploadFile(string name, IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var blob = await _storageClient.UploadObjectAsync(BUCKET_NAME, name, file.ContentType, stream);
            var fileUri = new Uri(blob.MediaLink);
            return fileUri;
        }
    }
}
