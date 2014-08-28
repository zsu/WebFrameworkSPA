using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Web.Security;
using System.Linq;
using App.Common.InversionOfControl;
using System.Security.Claims;
namespace Web.Infrastructure.Extensions
{
    public static class PrincipleExtension
    {
        //[DebuggerStepThrough]
        public static bool IsInAnyRole(this IPrincipal principal, List<string> roles)
        {
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
                return false;
            var user = principal.Identity as ClaimsIdentity;
            if (user == null || !user.IsAuthenticated)
                return false;
            return user.Claims.Any(c => c.Type == ClaimTypes.Role && roles.Contains(c.Value));
        }
    }
}