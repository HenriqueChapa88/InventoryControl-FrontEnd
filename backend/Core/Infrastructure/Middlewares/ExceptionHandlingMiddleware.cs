using System.Net;
using System.Text.Json;

namespace InventoryControl.Infrastructure.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new
            {
                status = (int)HttpStatusCode.InternalServerError,
                error = "An error occurred while processing your request",
                timestamp = DateTime.UtcNow.ToString("o")
            };

            switch (exception)
            {
                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response = new
                    {
                        status = (int)HttpStatusCode.Unauthorized,
                        error = "Unauthorized access",
                        timestamp = DateTime.UtcNow.ToString("o")
                    };
                    break;

                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response = new
                    {
                        status = (int)HttpStatusCode.NotFound,
                        error = "Resource not found",
                        timestamp = DateTime.UtcNow.ToString("o")
                    };
                    break;

                case ArgumentException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new
                    {
                        status = (int)HttpStatusCode.BadRequest,
                        error = exception.Message,
                        timestamp = DateTime.UtcNow.ToString("o")
                    };
                    break;

                default:
                    _logger.LogError(exception, "Unhandled exception occurred");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}