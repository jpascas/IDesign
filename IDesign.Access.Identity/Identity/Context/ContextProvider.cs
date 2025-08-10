using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace IDesign.Access.Identity
{
    public class ContextProvider : IContextProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public ContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public long GetCurrentUserId()
        {
            return Convert.ToInt64(this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
