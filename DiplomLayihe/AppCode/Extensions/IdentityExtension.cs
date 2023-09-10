using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Extensions
{
    public static partial class Extension
    {
        public static int? GetPrincipalId(this IActionContextAccessor ctx)
        {
            return GetPrincipalId(ctx.ActionContext.HttpContext.User);
        }


        public static int? GetPrincipalId(this ClaimsPrincipal principal)
        {
            if (principal == null ||
                !principal.Identity.IsAuthenticated)
            {
                return null;
            }

            return Convert.ToInt32(principal.Claims.First(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
        }


        public static bool HasAccess(this ClaimsPrincipal principal, string policyName)
        {

            return principal.HasClaim(c => c.Type.Equals(policyName) && c.Value.Equals("1")) || principal.IsInRole("Superadmin");
        }
    }
}
