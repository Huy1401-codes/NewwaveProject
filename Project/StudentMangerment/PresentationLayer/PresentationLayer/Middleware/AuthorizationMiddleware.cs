namespace PresentationLayer.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// lấy thông tin đăng nhập của người dùng để kiểm tra.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.Session.GetString("UserId");
            var role = context.Session.GetString("Role");

            // Chưa đăng nhập
            if (userId == null)
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }

            var endpoint = context.GetEndpoint();

            if (endpoint != null)
            {
                var requiredRoles = endpoint.Metadata.GetMetadata<RequireRoleAttribute>();

                if (requiredRoles != null)
                {
                    if (role == null || !requiredRoles.Roles.Contains(role))
                    {
                        context.Response.Redirect("/Auth/AccessDenied");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
