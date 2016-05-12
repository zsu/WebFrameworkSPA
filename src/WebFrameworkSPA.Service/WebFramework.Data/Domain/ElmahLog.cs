using App.Common.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebFramework.Data.Domain
{
    public class ElmahLog : Entity<Guid>
    {
        [Required]
        public virtual string Application { get; set; }
        //public NameValueCollection Cookies { get; set; }
        //public string detail { get; set; }
        //public Exception exception { get; set; }
        //public NameValueCollection form { get; set; }
        public virtual string Host { get; set; }
        public virtual string Message { get; set; }
        //public NameValueCollection queryString { get; set; }
        //public NameValueCollection serverVariables { get; set; }
        public virtual string Source { get; set; }
        public virtual int StatusCode { get; set; }
        [Required]
        public virtual DateTime Time { get; set; }
        public virtual string Type { get; set; }
        public virtual string User { get; set; }
        //public string webHostHtmlMessage { get; set; }
        public virtual string Allxml { get; set; }

    }
}
