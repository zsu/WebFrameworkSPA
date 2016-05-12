using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using Web.Infrastructure;

namespace Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //config.Formatters.Clear();
            //config.Formatters.Add(new JsonMediaTypeFormatter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Services.Add(typeof(IExceptionLogger), new WebApiExceptionLogger());
            if(!string.IsNullOrWhiteSpace(App.Common.Util.ApplicationConfiguration.GetProperty("CorsOrigin")))
                config.EnableCors(new EnableCorsAttribute(App.Common.Util.ApplicationConfiguration.GetProperty("CorsOrigin"), "*", "*"));
        }
    }
}