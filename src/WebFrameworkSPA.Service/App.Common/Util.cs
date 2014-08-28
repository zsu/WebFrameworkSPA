using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Web;
using System.Globalization;
using System.ComponentModel;

namespace App.Common
{
    public static class Util
    {
        #region Fields
        public const string LogConfigKey = "LogConfig";
        public const string AppConfigKey = "AppConfig";
        public const string ApplicationCacheKey = "ApplicationCache";
        public const string PerRequestCacheKey = "PerRequestCache";
        public const string ContextCacheKey = "ContextCache";
        public const string AppDBConnectionStringName = "AppDB";
        public const string SecurityDBConnectionStringName = "SecurityDB";
        public const string LogDBConnectionStringName = "LogDB";
        public static string AppDBConnectionString = ConfigurationManager.ConnectionStrings[AppDBConnectionStringName] != null ? ConfigurationManager.ConnectionStrings[AppDBConnectionStringName].ConnectionString : null;
        public static string SecurityDBConnectionString = ConfigurationManager.ConnectionStrings[SecurityDBConnectionStringName] != null ? ConfigurationManager.ConnectionStrings[SecurityDBConnectionStringName].ConnectionString : null;
        public static string LogDBConnectionString = ConfigurationManager.ConnectionStrings[LogDBConnectionStringName] != null ? ConfigurationManager.ConnectionStrings[LogDBConnectionStringName].ConnectionString : null;
        #endregion

        #region Methods
        public static string LogConfigFilePath
        {
            get { return GetFullPath(ConfigurationManager.AppSettings[LogConfigKey]); }
        }
        public static string ApplicationConfigFilePath
        {
            get { return GetFullPath(ConfigurationManager.AppSettings[AppConfigKey]); }
        }
        /// <summary>
        /// Get full path of the file
        /// </summary>
        /// <param name="sPath">Relative path to the webroot or absolute physical path of file</param>
        /// <returns>Absolute Physical full path of the file</returns>
        public static string GetFullPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            path = path.Trim();
            string pathRoot = Path.GetPathRoot(path);
            if (pathRoot == string.Empty || pathRoot == "\\")
                if (System.Web.HttpContext.Current != null)
                {
                    StringBuilder url = new StringBuilder("~");
                    if (!path.StartsWith("/"))
                        url.Append("/");
                    url.Append(path);
                    return System.Web.HttpContext.Current.Server.MapPath(url.ToString());
                }
                else
                    if (Assembly.GetEntryAssembly() != null)
                        return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), path);
                    else
                        return Path.GetFullPath(path);
            else
                return path;
        }

        public static IAppConfig ApplicationConfiguration
        {
            get { return AppConfigManager.GetConfig(ConfigurationManager.AppSettings[AppConfigKey]); }
        }
        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        }
        public static string MakeValidFileName(string name)
        {
            string fileName = name;
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }
        #region Restart Application
        public static void RestartAppDomain(bool makeRedirect = false, string redirectUrl = "")
        {
            if (GetTrustLevel() > AspNetHostingPermissionLevel.Medium)
            {
                //full trust
                HttpRuntime.UnloadAppDomain();

                TryWriteGlobalAsax();
            }
            else
            {
                //medium trust
                bool success = TryWriteWebConfig();
                if (!success)
                {
                    throw new Exception(string.Format("{0} needs to be restarted due to a configuration change, but was unable to do so." + Environment.NewLine +
                        "To prevent this issue in the future, a change to the web server configuration is required:" + Environment.NewLine +
                        "- run the application in a full trust environment, or" + Environment.NewLine +
                        "- give the application write access to the 'web.config' file.", ApplicationConfiguration.AppAcronym));
                }

                success = TryWriteGlobalAsax();
                if (!success)
                {
                    throw new Exception(string.Format("{0} nopCommerce needs to be restarted due to a configuration change, but was unable to do so." + Environment.NewLine +
                        "To prevent this issue in the future, a change to the web server configuration is required:" + Environment.NewLine +
                        "- run the application in a full trust environment, or" + Environment.NewLine +
                        "- give the application write access to the 'Global.asax' file.", ApplicationConfiguration.AppAcronym));
                }
            }

            // If setting up extensions/modules requires an AppDomain restart, it's very unlikely the
            // current request can be processed correctly.  So, we redirect to the same URL, so that the
            // new request will come to the newly started AppDomain.
            //if (HttpContext.Current != null && makeRedirect)
            //{
            //    if (String.IsNullOrEmpty(redirectUrl))
            //        redirectUrl = GetThisPageUrl(true);
            //    _httpContext.Response.Redirect(redirectUrl, true /*endResponse*/);
            //}
        }
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            AspNetHostingPermissionLevel? trustLevel = null;
            if (!trustLevel.HasValue)
            {
                //set minimum
                trustLevel = AspNetHostingPermissionLevel.None;

                //determine maximum
                foreach (AspNetHostingPermissionLevel level in
                        new AspNetHostingPermissionLevel[] {
                                AspNetHostingPermissionLevel.Unrestricted,
                                AspNetHostingPermissionLevel.High,
                                AspNetHostingPermissionLevel.Medium,
                                AspNetHostingPermissionLevel.Low,
                                AspNetHostingPermissionLevel.Minimal 
                            })
                {
                    try
                    {
                        new AspNetHostingPermission(level).Demand();
                        trustLevel = level;
                        break; //we've set the highest permission we can
                    }
                    catch (System.Security.SecurityException)
                    {
                        continue;
                    }
                }
            }
            return trustLevel.Value;
        }
        private static bool TryWriteWebConfig()
        {
            try
            {
                // In medium trust, "UnloadAppDomain" is not supported. Touch web.config
                // to force an AppDomain restart.
                File.SetLastWriteTimeUtc(HttpContext.Current.Server.MapPath("~/web.config"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryWriteGlobalAsax()
        {
            try
            {
                //When a new plugin is dropped in the Plugins folder and is installed into nopCommerce, 
                //even if the plugin has registered routes for its controllers, 
                //these routes will not be working as the MVC framework couldn't 
                //find the new controller types and couldn't instantiate the requested controller. 
                //That's why you get these nasty errors 
                //i.e "Controller does not implement IController".
                //The issue is described here: http://www.nopcommerce.com/boards/t/10969/nop-20-plugin.aspx?p=4#51318
                //The solutino is to touch global.asax file
                File.SetLastWriteTimeUtc(HttpContext.Current.Server.MapPath("~/global.asax"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion Restart Application
        public static T ConvertTo<T>(object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)ConvertTo(value, typeof(T));
        }
        public static object ConvertTo(object value, Type destinationType)
        {
            return ConvertTo(value, destinationType, CultureInfo.InvariantCulture);
        }
        public static object ConvertTo(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                Type dstType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;

                var sourceType = Nullable.GetUnderlyingType(value.GetType())??value.GetType();

                TypeConverter destinationConverter = TypeDescriptor.GetConverter(dstType);
                TypeConverter sourceConverter = TypeDescriptor.GetConverter(sourceType);
                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                    return destinationConverter.ConvertFrom(null, culture, value);
                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                    return sourceConverter.ConvertTo(null, culture, value, destinationType);
                if (destinationType.IsEnum && value is int)
                    return Enum.ToObject(destinationType, (int)value);
                if (!destinationType.IsAssignableFrom(value.GetType()))
                    return Convert.ChangeType(value, destinationType, culture);
            }
            return value;
        }

        #endregion Methods
    }

    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.UtcNow;
    }
}
