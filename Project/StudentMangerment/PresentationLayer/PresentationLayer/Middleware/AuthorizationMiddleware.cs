namespace PresentationLayer.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Lấy thông tin user từ session
            var userId = context.Session.GetString("UserId");
            var role = context.Session.GetString("Role");

            // Nếu chưa đăng nhập → redirect về login
            if (userId == null)
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }

            // Nếu request có endpoint yêu cầu role
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var requiredRoles = endpoint.Metadata.GetMetadata<RequireRoleAttribute>();
                if (requiredRoles != null)
                {
                    if (role == null || !requiredRoles.Roles.Contains(role))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("403 - Forbidden");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
