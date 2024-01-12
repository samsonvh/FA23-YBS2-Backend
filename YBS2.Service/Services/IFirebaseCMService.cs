using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS2.Service.Dtos.Details;

namespace YBS2.Service.Services
{
    public interface IFirebaseCMService
    {
        Task<bool> SendTransactionNotification(FCMMessageRequest request);
    }
}
