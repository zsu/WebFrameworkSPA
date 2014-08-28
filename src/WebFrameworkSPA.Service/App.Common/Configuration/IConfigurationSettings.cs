namespace App.Common
{
    public interface IConfigurationSettings
    {
        string RootUrl
        {
            get;
        }

        string WebmasterEmail
        {
            get;
        }

        string SupportEmail
        {
            get;
        }

        string DefaultEmailOfOpenIdUser
        {
            get;
        }

        string SiteTitle
        {
            get;
        }

        string MetaKeywords
        {
            get;
        }

        string MetaDescription
        {
            get;
        }

        int HtmlUserPerPage
        {
            get;
        }

        float MaximumAgeOfStoryInHoursToPublish
        {
            get;
        }
    }
}