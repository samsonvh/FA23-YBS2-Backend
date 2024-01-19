using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.FirebaseRealtimeDatabase.v1beta;

namespace YBS2.Service.Services
{
    public interface IFirebaseRealtimeService
    {
        public Task<string> GetData (string key, Guid id);
        public Task WriteData (string key , Guid id);
    }
}