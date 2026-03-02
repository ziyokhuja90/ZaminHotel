using System.Net;
using System.Text.Json;

namespace ZaminHotel.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case InvalidCastException:
                    statusCode = HttpStatusCode.Conflict;
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Forbidden;
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;

            }

            var response = new
            {
                status = (int)statusCode,
                message = exception.Message
            };
            var json = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
                
            return context.Response.WriteAsync(json);
        }
    }
}
