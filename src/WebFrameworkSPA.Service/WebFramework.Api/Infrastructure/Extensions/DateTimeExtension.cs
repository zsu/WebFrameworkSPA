using App.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Infrastructure.Extensions
{
    public static class DateTimeExtension
    {
        public static string ToClientTime(this DateTime dt)
        {
            return ToClientTime(dt, true);
        }
        public static string ToClientTime(this DateTime dt, bool withTimeZoneInfo)
        {
            string timezoneId = null;
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies.AllKeys.Contains(Util.COOKIE_KEY_TIME_ZONE_ID))
                    timezoneId = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies[Util.COOKIE_KEY_TIME_ZONE_ID].Value);
                string windowsTimezone = Util.GetClientTimeZone();
                if (!string.IsNullOrEmpty(windowsTimezone))
                {
                    var localTime = TimeZoneInfo.ConvertTimeFromUtc(dt, TimeZoneInfo.FindSystemTimeZoneById(windowsTimezone));
                    if (withTimeZoneInfo)
                        return string.Format("{0} {1}", localTime.ToString(), windowsTimezone);
                    else
                        return localTime.ToString();
                }
                //else
                //    Logger.Log(LogLevel.Error, string.Format("Invalid timezone {0}", timezoneId));

            }
            catch (Exception exception)
            {
                Logger.Log(LogLevel.Error, string.Format("Invalid timezone {0}", timezoneId), exception);
            }

            // if there is no timezoneid in session return the datetime in server timezone
            if (withTimeZoneInfo)
                return string.Format("{0} UTC", dt.ToString());
            else
                return dt.ToString();
        }
    }
}