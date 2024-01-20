using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.FirebaseRealtimeDatabase.v1beta;
using Google.Apis.FirebaseRealtimeDatabase.v1beta.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;

namespace YBS2.Service.Services.Implements
{
    public class FirebaseRealtimeService : IFirebaseRealtimeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _jsonPath;
        private readonly string _databaseURL;
        private readonly string _scoped;
        private readonly string _appName;
        private readonly GoogleCredential _credential;
        private readonly IConfiguration _configuration;
        public FirebaseRealtimeService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _jsonPath = _configuration["Firebase:RealtimeDatabase:JSonPath"];
            _databaseURL = _configuration["Firebase:RealtimeDatabase:DatabaseURL"];
            _scoped = _configuration["Firebase:RealtimeDatabase:Scoped"];
            _appName = _configuration["Firebase:RealtimeDatabase:AppName"];
            GoogleCredential.FromFile(_jsonPath).CreateScoped(_scoped);
        }
        public async Task<string> GetData(string key, Guid id)
        {
            string databaseUrl = "https://<your-firebase-project>.firebasedatabase.app";

            // Replace with the path to your service account credentials JSON file
            string credentialsPath = "<path-to-service-account-credentials.json>";

            // Create a new Firebase Realtime Database client
            var service = new FirebaseRealtimeDatabaseService(new BaseClientService.Initializer
            {
                HttpClientInitializer = _credential,
                ApplicationName = "YourAppName"
            });
            // var getRequest = service.Get(databaseUrl + "users/1.json");
            return "";
        }

        public async Task WriteData(string key, Guid id)
        {
            StringContent content = new StringContent(true.ToString(), System.Text.Encoding.UTF8, "application/json");
        }
    }
}