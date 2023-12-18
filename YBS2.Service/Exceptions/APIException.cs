using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Exceptions
{
    public class APIException : Exception
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public APIException(int StatusCode, string Message)
        {
            this.StatusCode = StatusCode;
            this.Message = Message;
        }
    }
}