using App.Common.InversionOfControl;
using Service;
using System.Collections.Generic;
using System.Security.Claims;
using BrockAllen.MembershipReboot.Nh;

namespace Web.Infrastructure
{
    public class RoleClaimsAuthenticationManager : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (incomingPrincipal != null && incomingPrincipal.Identity.IsAuthenticated == true)
            {
                var id = (ClaimsIdentity)incomingPrincipal.Identity;
                IRoleService roleService = IoC.GetService<IRoleService>();
                List<Role> roles = roleService.GetRolesForUser(Util.GetUserId(id));
                foreach (var role in roles)
                {
                    id.AddClaim(new Claim(ClaimTypes.Role, role.Name));
                    if (role.Permissions != null && role.Permissions.Count > 0)
                        foreach (var permission in role.Permissions)
                            id.AddClaim(new Claim(ClaimTypes.Role, permission.Name));
                }
            }
            return incomingPrincipal;
        }
    }
}