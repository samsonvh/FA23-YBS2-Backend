using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS2.Service.Dtos.Details;

namespace YBS2.Service.Services.Implements
{
    public class FirebaseCMService : IFirebaseCMService
    {

        public FirebaseCMService() { }

        public async Task<bool> SendTransactionNotification(FCMMessageRequest request)
        {
            var message = new Message()
            {
                Notification = new Notification
                {
                    Title = request.Title,
                    Body = request.Body,
                },
                //Data = new Dictionary<string, string>()
                //{
                //    ["FirstName"] = "John",
                //    ["LastName"] = "Doe"
                //},
                Token = request.DeviceToken
            };

            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);

            if (!string.IsNullOrEmpty(result))
            {
                // Message was sent successfully
                return true;
            }
            else
            {
                // There was an error sending the message
                return false;
            }
        }
    }
}
