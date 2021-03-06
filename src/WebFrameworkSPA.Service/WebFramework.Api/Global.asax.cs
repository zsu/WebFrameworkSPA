using App.Common;
using App.Common.InversionOfControl;
using App.Common.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            IoC.InitializeWith(new DependencyResolverFactory());
            var container = IoCConfig.ConfigureContainer();

            log4net.GlobalContext.Properties["AppName"] = Util.ApplicationConfiguration.AppAcronym;
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Util.GetFullPath(Util.LogConfigFilePath)));
            Web.Infrastructure.Util.ChangeLogLevels();
            //GlobalConfiguration.Configuration.DependencyResolver = (System.Web.Http.Dependencies.IDependencyResolver)IoC.GetResolver<ICustomDependencyResolver>();
            GlobalConfiguration.Configuration.Services.Replace(typeof(System.Web.Http.Dispatcher.IHttpControllerActivator), new WindsorCompositionRoot(container));

            //GlobalConfiguration.Configuration.Filters.Add(new ElmahHandleErrorApiAttribute());
            //var controllerFactory = new WindsorControllerFactory(IoC.GetContainer<IWindsorContainer>().Kernel);
            //ControllerBuilder.Current.SetControllerFactory(controllerFactory);


            //AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            //AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters();
            //FilterConfig.RegisterWebApiGlobalFilters();
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            Logger.Log(LogLevel.Info, LogType.Application.ToString(), "Application_Start event fired.");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null)
                log4net.ThreadContext.Properties["SessionId"] = HttpContext.Current.Session.SessionID;
        }

        protected void Application_BeginRequest(Object source, EventArgs e)
        {
            if ((VirtualPathUtility.ToAbsolute("~/") != Request.ApplicationPath) && (Request.ApplicationPath.ToLowerInvariant() == Request.Path.ToLowerInvariant()))
            {
                var redirectPath = VirtualPathUtility.AppendTrailingSlash(Request.Path);

                Response.RedirectPermanent(redirectPath);

                return;
            }
            if (Request.Headers["Authorization"] == null)
            {
                var bearerToken = Request.QueryString["bearerToken"];

                if (!String.IsNullOrEmpty(bearerToken))
                {
                    Request.Headers.Add("Authorization", "Bearer " + HttpUtility.UrlDecode(bearerToken));
                }
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Url.PathAndQuery.Contains("favicon.ico")) //Ignore favicon.ico not found error
                return;
            Exception exception = Server.GetLastError();
            Server.ClearError();
            Logger.Log(LogLevel.Error, exception);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            StringBuilder logMessage = new StringBuilder();
            logMessage.AppendLine("Application_End event fired.");

            HttpRuntime runtime = (HttpRuntime)typeof(System.Web.HttpRuntime).InvokeMember("_theRuntime",
                                                                               BindingFlags.NonPublic
                                                                               | BindingFlags.Static
                                                                               | BindingFlags.GetField,
                                                                               null,
                                                                               null,
                                                                               null);

            if (runtime != null)
            {
                string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage",
                                                                                 BindingFlags.NonPublic
                                                                                 | BindingFlags.Instance
                                                                                 | BindingFlags.GetField,
                                                                                 null,
                                                                                 runtime,
                                                                                 null);
                string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack",
                                                                               BindingFlags.NonPublic
                                                                               | BindingFlags.Instance
                                                                               | BindingFlags.GetField,
                                                                               null,
                                                                               runtime,
                                                                               null);
                logMessage.AppendLine(String.Format("\r\n\r\n_shutDownMessage={0}\r\n\r\n_shutDownStack={1}",
                             shutDownMessage,
                             shutDownStack));
            }
            Logger.Log(LogLevel.Info, LogType.Application.ToString(), logMessage.ToString());

            ////if (!EventLog.SourceExists(".NET Runtime"))
            ////{
            ////    EventLog.CreateEventSource(".NET Runtime", "Application");
            ////}
            ////EventLog log = new EventLog();
            ////log.Source = ".NET Runtime";
            ////log.WriteEntry(String.Format("\r\n\r\n_shutDownMessage={0}\r\n\r\n_shutDownStack={1}",
            ////                             shutDownMessage,
            ////                             shutDownStack),
            ////                             EventLogEntryType.Error);
        }
    }
}