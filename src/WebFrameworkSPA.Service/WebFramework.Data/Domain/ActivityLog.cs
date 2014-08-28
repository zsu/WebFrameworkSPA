using App.Common;
using App.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebFramework.Data.Domain
{

    public class ActivityLog
    {
        public ActivityLog()
        {

        }
        public ActivityLog(string activity,string detail)
        {
            Application = Util.ApplicationConfiguration.AppAcronym;
            CreatedDate = DateTime.UtcNow;
            Activity = activity;
            Detail = detail;
            if(HttpContext.Current!=null)
            {
                if(HttpContext.Current.User!=null)
                    UserName = HttpContext.Current.User.Identity.Name;
                if(HttpContext.Current.Request!=null)
                    ClientIP = HttpContext.Current.Request.UserHostAddress;
            }
        }
        public virtual Guid Id { get; set; }
        public virtual string Application { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string Activity { get; set; }
        public virtual string Detail { get; set; }
        public virtual string ClientIP { get; set; }
    }
}
