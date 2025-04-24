using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Api.Utils
{
    public static class UserUtils
    {
        public static int GetCurrentUserId(HttpContext httpContext)
        {
            var claim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null && int.TryParse(claim.Value, out int id)
                ? id
                : throw new UnauthorizedAccessException("Usuário não autenticado.");
        }
    }
}
