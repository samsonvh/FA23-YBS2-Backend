using System.Net;

namespace YBS2.Service.Exceptions
{
    public class APIException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public APIException(HttpStatusCode StatusCode, string Message)
        {
            this.StatusCode = StatusCode;
            this.Message = Message;
        }
    }
}