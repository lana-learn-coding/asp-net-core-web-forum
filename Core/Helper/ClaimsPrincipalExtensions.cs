using System;
using System.Linq;
using System.Security.Claims;

namespace Core.Helper
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsUser(this ClaimsPrincipal obj)
        {
            return obj.Identity?.IsAuthenticated ?? false;
        }

        public static bool IsAdmin(this ClaimsPrincipal obj)
        {
            return obj.IsUser() && obj.IsInRole("Admin");
        }

        public static string GetClaim(this ClaimsPrincipal obj, string name)
        {
            if (!obj.IsUser()) return "";
            var claim = obj.Claims.FirstOrDefault(x => x.Type.Equals(name));
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static Guid? Id(this ClaimsPrincipal obj)
        {
            var id = obj.GetClaim("uid");
            if (Guid.TryParse(id, out var guid)) return guid;
            return null;
        }
    }
}