using System.Security.Claims;

namespace PresentationLayer.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var role = context.User?.FindFirst(ClaimTypes.Role)?.Value ?? "None";

            _logger.LogInformation(
                "Request {Method} {Path} | UserId: {UserId} | Role: {Role}",
                context.Request.Method,
                context.Request.Path,
                userId,
                role
            );

            await _next(context);
        }
    }
}
