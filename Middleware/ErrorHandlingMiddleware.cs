using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Web.Exceptions;

namespace Web.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private ILogger _logger;

        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // if it's not one of the expected exception, set it to 500
            var code = HttpStatusCode.InternalServerError;

            if (exception is NotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }
            else if (exception is UnauthorizedException)
            {
                code = HttpStatusCode.Unauthorized;
            }
            else if (exception is InvalidParameterException)
            {
                code = HttpStatusCode.BadRequest;
            }

            return WriteExceptionAsync(context, exception, code);
        }

        private Task WriteExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)code;

            _logger.LogError(exception, $"Exception StatusCode[{response.StatusCode}: {code}], Message: {exception.Message}", new { RequestId = context.TraceIdentifier });

            return response.WriteAsync(JsonConvert.SerializeObject(new
            {
                requestId = context.TraceIdentifier,
                message = exception.Message,
                code = code,
                exception = exception.InnerException == null ? exception.GetType().Name : exception.InnerException.GetType().Name
            }));
        }
    }
}
