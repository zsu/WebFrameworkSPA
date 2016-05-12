using System;
using System.Text;
using System.Web;

namespace Web.Infrastructure.Exceptions
{
    public class JavascriptException : Exception
    {
        string message;

        public JavascriptException(string message) : base(message)
        {
            StringBuilder mes = new StringBuilder();
            mes.AppendLine(message);
            mes.Append(LogBrowserInfo());
            this.message = mes.ToString();
        }

        public override string ToString()
        {
            return message;
        }
        private string LogBrowserInfo()
        {
            StringBuilder browserMessage = new StringBuilder();

            try
            {
                browserMessage.AppendLine("**** BEGIN BROWSER INFO ****");
                browserMessage.Append("* User's IP Address: ");
                browserMessage.AppendLine(HttpContext.Current.Request.UserHostAddress);
                browserMessage.Append("* User's DNS: ");
                browserMessage.AppendLine(HttpContext.Current.Request.UserHostName);
                browserMessage.Append("* Client Platform: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.Platform);
                browserMessage.Append("* Browser: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.Type);
                browserMessage.Append("* Browser Ver: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.Version);
                browserMessage.Append("* Client CLR Ver: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.ClrVersion.ToString());
                browserMessage.Append("* ECMA Script Ver: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.EcmaScriptVersion.ToString());
                browserMessage.Append("* MS DOM Ver: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.MSDomVersion.ToString());
                browserMessage.Append("* W3C DOM Ver: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.W3CDomVersion.ToString());
                browserMessage.Append("* Using AOL: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.AOL.ToString());
                browserMessage.Append("* Supports Tables: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.Tables.ToString());
                browserMessage.Append("* Supports Cookies: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.Cookies.ToString());
                browserMessage.Append("* Supports Frames: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.Frames.ToString());
                browserMessage.Append("* Supports Java Applets: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.JavaApplets.ToString());
                browserMessage.Append("* Supports Java Script: ");
                browserMessage.AppendLine((HttpContext.Current.Request.Browser.EcmaScriptVersion.Major >= 1).ToString());
                browserMessage.Append("* Supports ActiveX: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.ActiveXControls.ToString());
                browserMessage.Append("* Browser is Crawler: ");
                browserMessage.AppendLine(HttpContext.Current.Request.Browser.Crawler.ToString());
                browserMessage.AppendLine("**** END BROWSER INFO ****");
            }
            catch
            {
                browserMessage.AppendLine("**** UNABLE TO CAPTURE BROWSER INFO ****");
            }
            return browserMessage.ToString();

        } // LogBrowserInfo
    }
}