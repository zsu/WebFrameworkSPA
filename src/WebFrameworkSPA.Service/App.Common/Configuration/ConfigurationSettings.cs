namespace App.Common
{
    public class ConfigurationSettings : IConfigurationSettings
    {
        public string RootUrl
        {
            get;
            set;
        }

        public string WebmasterEmail
        {
            get;
            set;
        }

        public string SupportEmail
        {
            get;
            set;
        }

        public string DefaultEmailOfOpenIdUser
        {
            get;
            set;
        }

        public string SiteTitle
        {
            get;
            set;
        }

        public string MetaKeywords
        {
            get;
            set;
        }

        public string MetaDescription
        {
            get;
            set;
        }

        public int HtmlUserPerPage
        {
            get;
            set;
        }

        public float MaximumAgeOfStoryInHoursToPublish
        {
            get;
            set;
        }
    }
}