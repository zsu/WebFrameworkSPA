using System;
using System.Web;
using BrockAllen.MembershipReboot;

namespace Web.Infrastructure
{
    public class MembershipRebootAppInfo : RelativePathApplicationInformation
    {
        public MembershipRebootAppInfo(
        string appName,
        string emailSig,
        string relativeLoginUrl,
        string relativeConfirmChangeEmailUrl,
        string relativeCancelVerificationUrl,
        string relativeConfirmPasswordResetUrl
        )
        {
            this.ApplicationName = appName;
            this.EmailSignature = emailSig;
            this.RelativeLoginUrl = relativeLoginUrl;
            this.RelativeConfirmChangeEmailUrl = relativeConfirmChangeEmailUrl;
            this.RelativeCancelVerificationUrl = relativeCancelVerificationUrl;
            this.RelativeConfirmPasswordResetUrl = relativeConfirmPasswordResetUrl;
        }
        protected override string GetApplicationBaseUrl()
        {
            string baseUrl=System.Configuration.ConfigurationManager.AppSettings[Constants.APPSETTING_KEY_BASE_URL];
            if(string.IsNullOrWhiteSpace(baseUrl))
                baseUrl = (HttpContext.Current.Request.Url.GetComponents(
                    UriComponents.SchemeAndServer, UriFormat.Unescaped).TrimEnd('/')
                 + HttpContext.Current.Request.ApplicationPath.Substring(0,HttpContext.Current.Request.ApplicationPath.LastIndexOf('/')).TrimEnd('/')); //HttpContext.Current.GetApplicationUrl();
            return baseUrl;
        }
    }
}