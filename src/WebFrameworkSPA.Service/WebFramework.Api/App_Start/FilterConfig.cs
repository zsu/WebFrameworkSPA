using Web.FilterAttributes;


namespace Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters()
        {
            System.Web.Http.GlobalConfiguration.Configuration.Filters.Add(new AjaxMessagesFilterAttribute());
            System.Web.Http.GlobalConfiguration.Configuration.Filters.Add(new MaintenanceMessagesFilterAttribute());
        }
    }
}