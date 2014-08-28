using App.Common.InversionOfControl;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Web.Infrastructure.Extensions;
namespace Web.Controllers.Common
{
    public class MaintenanceWarningController : ApiController
    {
        public dynamic Get()
        {
            var settingService = IoC.GetService<ISettingService>();
            var startTime = settingService.GetSettingByKey<DateTime>(Constants.SETTING_KEYS_MAINTENANCE_START);
            var endTime = settingService.GetSettingByKey<DateTime>(Constants.SETTING_KEYS_MAINTENANCE_END);
            var maintenanceMessage = settingService.GetSettingByKey<string>(Constants.SETTING_KEYS_MAINTENANCE_MESSAGE, "The site is under maintenance.");
            var warningLead = settingService.GetSettingByKey<double>(Constants.SETTING_KEYS_MAINTENANCE_WARNING_LEAD, 24 * 60 * 60);
            var maintenanceWarningMessage = settingService.GetSettingByKey<string>(Constants.SETTING_KEYS_MAINTENANCE_WARNING_MESSAGE, string.Format("The site is going to be down for maintenance at {0}.", startTime.ToClientTime()));
            maintenanceWarningMessage = string.Format(maintenanceWarningMessage, startTime.ToClientTime());
            var user = HttpContext.Current.User;
            var claimsIdentity = user==null?null:user.Identity as ClaimsIdentity;
            bool canBypass = user != null && user.Identity.IsAuthenticated && claimsIdentity.HasClaim(x => x.Type == ClaimTypes.Role && (x.Value == Constants.ROLE_ADMIN || x.Value == Constants.PERMISSION_SMOKETEST));
            if (!canBypass && startTime != default(DateTime) && DateTime.UtcNow >= startTime)
            {
                if (endTime == default(DateTime) || DateTime.UtcNow <= endTime)
                {
                    //string fullyQualifiedUrl = string.Format("{0}/Maintenance.html", Request.RequestUri.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath);
                    //return Redirect(new Uri(fullyQualifiedUrl));
                    return Json<object>(new { Success = true, Redirect = true, Message=maintenanceMessage }); //maintenanceWarningMessage;
                }
            }
            if (startTime != default(DateTime) && startTime > DateTime.UtcNow)
            {
                var difference = (startTime - DateTime.UtcNow);
                if (difference.TotalSeconds < warningLead)
                {
                    return Json<object>(new { Success = true, Message = maintenanceWarningMessage }); //maintenanceWarningMessage;
                }
            }
            return null;
        }
    }
}
