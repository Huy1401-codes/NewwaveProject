using System.Net;

namespace PresentationLayer.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception | Path: {Path}",
                    context.Request.Path);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (context.Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        message = "Hệ thống đang gặp lỗi, vui lòng thử lại sau"
                    });
                }
                else
                {
                    context.Response.Redirect("/Auth/AccessDenied");
                }
            }
        }
    }
}
