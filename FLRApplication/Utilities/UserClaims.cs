using System.Security.Claims;
using System.Security.Principal;

namespace FLRApplication.Utilities
{
    public static class UserClaims
    {
        public static string GetName(this IPrincipal user)
        {
            var NameClaim = ((ClaimsIdentity)user.Identity).FindFirst("FullName");

            if (NameClaim != null)
            {
                return NameClaim.Value;
            }
            else
            {
                return "";
            }
        }
    }
}