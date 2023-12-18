using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using YBS2.Service.Exceptions;

namespace YBS2.Service.Dtos.PageResponses
{
    public class DefaultAPIResponse
    {
        public class Response
        {
            public int StatusCode { get; set;}
            public object Data { get; set;}
            public string Message { get; set;}
        }
        public static Response Ok (object Data, string Message = null)
        {
            var response = new Response()
            {
                StatusCode = 200,
                Data = Data,
                Message = Message
            };
            return response;
        }
        public static void ErrorApiResult(string message, HttpStatusCode statusCode)
        {
            throw new APIException((int)statusCode, message);
        }
    }
}