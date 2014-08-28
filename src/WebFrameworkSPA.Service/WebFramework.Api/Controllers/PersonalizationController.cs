using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using Web.Models;

namespace Web.Controllers
{
    public class PersonalizationController : ApiController
    {
        /// <summary>
        /// Web API to deliver personalized application features and UI claims based on incoming user identity.
        /// </summary>

        /// <summary>
        /// Get application personalization information based on the icnoming user.
        /// </summary>
        /// <returns></returns>
        public PersonalizationData GetPersonalizationData()
        {
            var user = RequestContext.Principal;
            var x = user as ClaimsPrincipal;

            var persData = new PersonalizationData
            {
                Features = GetFeatures(user).ToList(),
                UiClaims = new UiClaimsData
                {
                    UserName = user.Identity.Name,
                    Capabilities = GetCapabilities(user),
                    Constraints = GetConstraints(user),
                    NameValueClaims = GetNameValueClaims(user)
                }
            };

            return persData;
        }

        private IEnumerable<FeatureItem> GetFeatures(IPrincipal principal)
        {
            var claimsIdentity = principal.Identity as ClaimsIdentity;
            List<FeatureItem> menuItems = new List<FeatureItem>();
            menuItems.Add(new FeatureItem { Module = "About", DisplayText = "About", Url = "/about", MatchPattern = "/about" });
            menuItems.Add(new FeatureItem { Module = "Logs", DisplayText = "Logs", Url = "/logs", MatchPattern = "/logs",Roles=new List<string>{Constants.ROLE_ADMIN} });
            menuItems.Add(new FeatureItem { Module = "User", DisplayText = "Users", Url = "/user", MatchPattern = "/user", Roles = new List<string> { Constants.ROLE_ADMIN,Constants.ROLE_USERADMIN,Constants.PERMISSION_EDIT_USER } });
            menuItems.Add(new FeatureItem { Module = "Maintenance", DisplayText = "Maintenance", Url = "/maintenance", MatchPattern = "/maintenance", Roles = new List<string> { Constants.ROLE_ADMIN } });
            menuItems.Add(new FeatureItem { Module = "Profile", DisplayText = "Profile", Url = "/profile", MatchPattern = "/profile",Roles=new List<string> {"*"} });
            List<string> roles = claimsIdentity.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
            return menuItems.Where(m => m.Roles==null || m.Roles.Count==0 || (principal.Identity.IsAuthenticated && m.Roles.Contains("*")) || m.Roles.Intersect(roles, StringComparer.OrdinalIgnoreCase).Count() > 0 );
        }

        private List<Constraint> GetConstraints(IPrincipal principal)
        {
            double itemsLimit = GetItemsLimit(principal.Identity.Name);

            return new List<Constraint>
            {
                new NumericConstraint
                {
                    Name = "MaxNumberOfItems",
                    LowerLimit = 0,
                    UpperLimit = itemsLimit
                }
            };
        }

        private double GetItemsLimit(string userName)
        {
            if (userName == "cw")
            {
                return 10;
            }

            return 5;
        }

        private List<string> GetCapabilities(IPrincipal principal)
        {
            if (principal.Identity.Name.Equals("cw"))
            {
                return new List<string>
                {
                    "AddArticle"
                };
            }
            var claimsIdentity = principal.Identity as ClaimsIdentity;
            if (claimsIdentity.HasClaim(x => x.Type == ClaimTypes.Role && x.Value == Constants.ROLE_ADMIN))
                return new List<string>
                {
                    "AddArticle"
                };
            return new List<string>();
        }

        private List<NameValueClaim> GetNameValueClaims(IPrincipal principal)
        {
            if (principal.Identity.Name.Equals("cw"))
            {
                return new List<NameValueClaim>
                {
                    new NameValueClaim("Email", "aaa@aaa.aaa"),
                    new NameValueClaim("GivenName", "zsu"),
                };
            }

            var claimsIdentity = principal.Identity as ClaimsIdentity;
            return new List<NameValueClaim>
                {
                    new NameValueClaim("Email", claimsIdentity.Claims.Where(x=>x.Type==ClaimTypes.Email).Select(x=>x.Value).SingleOrDefault()),
                    new NameValueClaim("GivenName", claimsIdentity.Claims.Where(x=>x.Type==ClaimTypes.GivenName).Select(x=>x.Value).SingleOrDefault()),
                };
        }
    }
}
