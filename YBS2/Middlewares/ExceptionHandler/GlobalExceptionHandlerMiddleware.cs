using System.Net;
using System.Text.Json;
using YBS2.Service.Exceptions;

namespace YBS2.Middlewares.ExceptionHandler
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (APIException exception)
            {
                _logger.LogError($"An exception : {exception.Message}");
                //Set up the response status code 
                context.Response.StatusCode = (int)exception.StatusCode;
                //Set up the response type to Json
                context.Response.ContentType = "application/json";

                var jsonResponse = JsonSerializer.Serialize(new { title = exception.Message, exception.Errors });
                //Write error json to response body
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An Unhadled exception : {exception.Message}");
                //Set up the response status code 
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //Set up the response type to Json
                context.Response.ContentType = "application/json";
                //Create API Exception and serialize to Json 
                var apiResponse = new { message = exception.Message };
                //
                var jsonResponse = JsonSerializer.Serialize(apiResponse);
                //Write error json to response body
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}