using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using YBS2.Service.Services;

namespace YBS2.Service.Utils
{
    public static class FirebaseUtil
    {
        public static async Task<string>? UpLoadFile(List<IFormFile> imageURLs, Guid id, IFirebaseStorageService _firebaseStorageService)
        {
            if (imageURLs.Count > 0)
            {
                string imageURL = "";
                foreach (var image in imageURLs)
                {
                    int count = 1;
                    string imageName = id.ToString() + "-" + count;
                    Uri imageUri = await _firebaseStorageService.UploadFile(imageName, image);
                    imageURL += imageUri.ToString() + ",";
                }
                imageURL = imageURL.Remove(imageURL.Length - 1, 1);
                return imageURL;
            }
            return null;
        }
        public static async Task DeleteFile(string imageURL, IConfiguration _configuration, IFirebaseStorageService _firebaseStorageService)
        {
            string[] imageArraySplit = imageURL.Split(',');
            foreach (string image in imageArraySplit)
            {
                int lastIndexOfPrefixString = _configuration["Firebase:ImagePrefixURL"].Length;
                int indexOfSuffix = image.IndexOf("?");
                string imageName = image.Substring(0, lastIndexOfPrefixString).Substring(indexOfSuffix, image.Length);
                await _firebaseStorageService.DeleteFile(imageName);
            }
        }
    }
}