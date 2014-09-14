using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Security.Principal;
using App.Common.InversionOfControl;
using Service;
using System.Text.RegularExpressions;
using App.Common.Logging;
using WebFramework.Data.Domain;
using System.Linq.Dynamic;
using Web.Infrastructure.JqGrid;
using System.Text;
namespace Web.Infrastructure
{
    public class Util
    {
        public const string COOKIE_KEY_TIME_ZONE_ID = "timezoneid";
        public static readonly string SETTING_KEYS_LOG_LEVEL = (Constants.SHOULD_FILTER_BY_APP?App.Common.Util.ApplicationConfiguration.AppAcronym+".":String.Empty) + "logsettings.loglevel.";
        public static Guid GetCurrentUserId()
        {
            return GetUserId(ClaimsPrincipal.Current.Identity);
        }
        public static Guid GetUserId(IIdentity identity)
        {
            if (identity == null || !identity.IsAuthenticated)
                throw new Exception(string.Format("Identity {0} is not authenticated.", identity == null ? string.Empty : identity.Name));
            //return new Guid(identity.GetUserId());
            if (identity == null)
                throw new ArgumentNullException("identity");
            ClaimsIdentity identity1 = identity as ClaimsIdentity;
            if (identity1 != null)
                return new Guid(FindFirstValue(identity1, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
            else
                return Guid.Empty;
        }

        private static string FindFirstValue(ClaimsIdentity identity, string claimType)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");
            Claim first = identity.FindFirst(claimType);
            if (first != null)
                return first.Value;
            else
                return (string)null;
        }
        public static void ChangeLogLevels(Setting item)
        {
            try
            {
                //var settingService = IoC.GetService<ISettingService>();
                string logLevelSettingPrefix = SETTING_KEYS_LOG_LEVEL;
                //var settings = settingService.GetAllSettings();
                //var logLevelSettings = settings.AsQueryable().Where(x => x.Key.Trim().ToLower().StartsWith(logLevelSettingPrefix)).ToList();
                string pattern = logLevelSettingPrefix + "(.+)";
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);

                Match match = rgx.Match(item.Name);
                if (!match.Success)
                {
                    Logger.Log(LogLevel.Error, string.Format("Invalid log level setting key {0}", item.Name));
                }
                else
                {
                    string loggerName = match.Groups[1].Value;
                    log4net.Repository.Hierarchy.Hierarchy rootRepo = (log4net.LogManager.GetRepository() as log4net.Repository.Hierarchy.Hierarchy);
                    switch (loggerName.ToLowerInvariant())
                    {
                        case "root":
                            rootRepo.Root.Level = rootRepo.LevelMap["Info"];
                            break;
                        default:
                            var log = log4net.LogManager.GetLogger(loggerName);
                            log4net.Repository.Hierarchy.Logger logger = (log4net.Repository.Hierarchy.Logger)log.Logger;
                            logger.Level = rootRepo.LevelMap[item.Value];
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, ex);
            }
        }
        public static void ChangeLogLevels()
        {
            try
            {
                var settingService = IoC.GetService<ISettingService>();
                string logLevelSettingPrefix = SETTING_KEYS_LOG_LEVEL;
                var settings = settingService.GetAllSettings();
                var logLevelSettings = settings.AsQueryable().Where(x => x.Key.Trim().ToLower().StartsWith(logLevelSettingPrefix)).ToList();
                string pattern = logLevelSettingPrefix + "(.+)";
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                foreach (var setting in logLevelSettings)
                {
                    Match match = rgx.Match(setting.Key);
                    if (!match.Success)
                    {
                        Logger.Log(LogLevel.Error, string.Format("Invalid log level setting key {0}", setting.Key));
                    }
                    else
                    {
                        string loggerName = match.Groups[1].Value;
                        log4net.Repository.Hierarchy.Hierarchy rootRepo = (log4net.LogManager.GetRepository() as log4net.Repository.Hierarchy.Hierarchy);
                        switch (loggerName.ToLowerInvariant())
                        {
                            case "root":
                                rootRepo.Root.Level = rootRepo.LevelMap["Info"];
                                break;
                            default:
                                var log = log4net.LogManager.GetLogger(loggerName);
                                log4net.Repository.Hierarchy.Logger logger = (log4net.Repository.Hierarchy.Logger)log.Logger;
                                logger.Level = rootRepo.LevelMap[setting.Value.Value];
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, ex);
            }
        }

        public static void DeleteLogs(DateTime olderThan)
        {
            DeleteLogs(NHibernateConfig.LogDomainFactory(), "ELMAH_Error", "TimeUtc", olderThan);
        }
        public static void DeleteActivityLogs(DateTime olderThan)
        {
            DeleteLogs(NHibernateConfig.AppDomainFactory(), "ActivityLogs", "CreatedDate", olderThan);
        }
        public static void DeleteAuthenticationAudits(DateTime olderThan)
        {
            DeleteLogs(NHibernateConfig.LogDomainFactory(), "AuthenticationAudits", "CreatedDate", olderThan);
        }
        public static GridModel<T> GetGridData<T>([System.Web.Http.FromUri] SearchModel searchModelIn, IQueryable<T> query)
        {
            try
            {
                Web.Infrastructure.NgGridSearchModel searchModel = NgGridSearchModelConverter.Convert(searchModelIn);
                int totalRecords;
                int startRow = (searchModel.page * searchModel.rows) + 1;
                int skip = (searchModel.page > 0 ? searchModel.page - 1 : 0) * searchModel.rows;


                // note - these queries require "using System.Dynamic.Linq" library
                IQueryable<T> data = query;
                if (searchModel._search && !String.IsNullOrEmpty(searchModel.filters))
                {
                    var whereClause = searchModel.GenerateWhereClause(typeof(T));

                    if (!string.IsNullOrEmpty(whereClause.Clause))
                        data = data.Where(whereClause.Clause, whereClause.FormatObjects);
                }
                totalRecords = data.Count();
                if (!string.IsNullOrWhiteSpace(searchModel.sidx))
                    data = data.OrderBy(searchModel.sidx + " " + searchModel.sord);
                data = data.Skip(skip);
                data = data.Take(searchModel.rows == 0 ? totalRecords : searchModel.rows);
                var totalPages = (int)Math.Ceiling((float)totalRecords / searchModel.rows);

                return new GridModel<T>
                {
                    TotalNumber = totalRecords,
                    Items = data
                };
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Debug, ex);
                return new GridModel<T> { TotalNumber = 0, Items = new List<T>().AsQueryable<T>() };
            }
        }
        public static GridModel<T> GetGridData<T>([System.Web.Http.FromUri] JqGridSearchModel searchModel, IQueryable<T> query,long maxRecords=100000)
        {
            int totalRecords;
            int startRow = (searchModel.page * searchModel.rows) + 1;
            int skip = (searchModel.page > 0 ? searchModel.page - 1 : 0) * searchModel.rows;


            // note - these queries require "using System.Dynamic.Linq" library
            IQueryable<T> data = query;
            if (searchModel._search && !String.IsNullOrEmpty(searchModel.filters))
            {
                var whereClause = searchModel.GenerateWhereClause(typeof(T));

                if (!string.IsNullOrEmpty(whereClause.Clause))
                    data = data.Where(whereClause.Clause, whereClause.FormatObjects);
            }
            totalRecords = data.Count();
            if (searchModel.rows == 0 && totalRecords > maxRecords)
                throw new Exception("Too many records returned. Please refine your query.");
            if (!string.IsNullOrWhiteSpace(searchModel.sidx))
                data = data.OrderBy(searchModel.sidx + " " + searchModel.sord);
            data = data.Skip(skip);
            if(totalRecords>0)
                data = data.Take(searchModel.rows == 0 ? totalRecords : searchModel.rows);
            var totalPages = (int)Math.Ceiling((float)totalRecords / searchModel.rows);

            return new GridModel<T>
            {
                TotalNumber = totalRecords,
                Items = data
            };
        }
        public static string GetClientTimeZone()
        {
            string timezoneId = null;
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies.AllKeys.Contains(COOKIE_KEY_TIME_ZONE_ID))
                    timezoneId = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies[COOKIE_KEY_TIME_ZONE_ID].Value);
                var windowsTimeZone = IanaToWindows(timezoneId);
                if (!string.IsNullOrEmpty(windowsTimeZone))
                {
                    return windowsTimeZone;
                }
            }
            catch (Exception exception)
            {
                Logger.Log(LogLevel.Error, string.Format("Invalid timezone {0}", timezoneId), exception);
            }
            //Logger.Log(LogLevel.Error, string.Format("Invalid timezone {0}", timezoneId));
            return null;
        }
        public static System.Net.Http.HttpResponseMessage DisplayExportError(Exception ex)
        {
            if (!ex.Message.StartsWith("Too many records returned."))
            {
                Logger.Log(LogLevel.Error, ex);
            }
            StringBuilder message = new StringBuilder();
            message.AppendLine(@"<style type='text/css'>
                        .well {
                        background-image: -webkit-linear-gradient(top, #e8e8e8 0%, #f5f5f5 100%);
                        background-image: linear-gradient(to bottom, #e8e8e8 0%, #f5f5f5 100%);
                        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffe8e8e8', endColorstr='#fff5f5f5', GradientType=0);
                        background-repeat: repeat-x;
                        border-color: #dcdcdc;
                        -webkit-box-shadow: inset 0 1px 3px rgba(0, 0, 0, .05), 0 1px 0 rgba(255, 255, 255, .1);
                        box-shadow: inset 0 1px 3px rgba(0, 0, 0, .05), 0 1px 0 rgba(255, 255, 255, .1);
                        }
                        </style>
                        <center class='well'><div><h1>Error</h1></div>");
            message.AppendFormat("<div>{0}</div><br/>", ex.Message);
            message.AppendLine("<input type='button' value='Close' onClick='javascript:self.close();'></center>");
            return new System.Net.Http.HttpResponseMessage()
            {
                Content = new System.Net.Http.StringContent(message.ToString(),
                    Encoding.UTF8,
                    "text/html")
            };
        }
        //This will return the Windows zone that matches the IANA zone, if one exists.
        private static string IanaToWindows(string ianaZoneId)
        {
            var tzdbSource = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default;
            var mappings = tzdbSource.WindowsMapping.MapZones;
            var item = mappings.FirstOrDefault(x => x.TzdbIds.Contains(ianaZoneId));
            if (item == null) return null;
            return item.WindowsId;
        }

        // This will return the "primary" IANA zone that matches the given windows zone.
        private static string WindowsToIana(string windowsZoneId)
        {
            var tzdbSource = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default;
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(windowsZoneId);
            return tzdbSource.MapTimeZoneId(tzi);
        }
        private static void DeleteLogs(NHibernate.ISessionFactory sessionFactory, string table, string columnName, DateTime olderThan)
        {
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory");
            if (string.IsNullOrEmpty(table))
                throw new ArgumentNullException("table");
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException("columnName");
            string sql = string.Format("Delete from {0} where {1}<'{2}'", table, columnName, olderThan.ToShortDateString());
            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                session.CreateSQLQuery(sql).ExecuteUpdate();
                tx.Commit();
                Logger.Log(LogLevel.Debug, string.Format("Sql executed: {0}", sql));
            }
        }
    }
}