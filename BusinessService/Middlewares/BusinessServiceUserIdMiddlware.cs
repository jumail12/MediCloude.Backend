using System.Security.Claims;

namespace BusinessService.Middlewares
{
    public class BusinessServiceUserIdMiddlware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BusinessServiceUserIdMiddlware> _logger;

        public BusinessServiceUserIdMiddlware(RequestDelegate next, ILogger<BusinessServiceUserIdMiddlware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                var idClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                if (idClaim != null)
                {
                    httpContext.Items["UserId"] = idClaim.Value;
                }
                else
                {
                    _logger.LogWarning("No NameIdentifier found in the Jwt token");
                }
            }

            await _next(httpContext);
        }
    }
}
