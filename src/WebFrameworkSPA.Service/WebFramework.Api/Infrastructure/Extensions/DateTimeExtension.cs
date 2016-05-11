using App.Common.Logging;
using NodaTime;
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
            return ToClientTime(dt, true, null);
        }
        public static string ToClientTime(this DateTime dt, string format)
        {
            return ToClientTime(dt, true, format);
        }
        public static string ToClientTime(this DateTime dt, bool withTimeZoneInfo, string format)
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
                        return string.Format("{0} {1}", localTime.ToString(format), windowsTimezone);
                    else
                        return localTime.ToString(format);
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
                return string.Format("{0} UTC", dt.ToString(format));
            else
                return dt.ToString(format);
        }
    }
}