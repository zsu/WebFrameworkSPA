using System;
namespace App.Common
{
    public interface IAppConfig
    {
        string AppAcronym { get; }
        string AppFullName { get; }
        string AppVersion { get; }
        string Copyright { get; }
        string DateTimeFormat { get; }
        string GetProperty(string key);
        string OfflineFilePath { get; }
        string PrivacyPolicyURL { get; }
        string ProductProvider { get; }
        string ProductProviderEmail { get; }
        string ProductProviderPhone { get; }
        string ProductProviderURL { get; }
        string SecurityCacheEnable { get; }
        string SupportAddress1 { get; }
        string SupportAddress2 { get; }
        string SupportEmail { get; }
        string SupportHours { get; }
        string SupportOrganization { get; }
        string SupportPhone { get; }
        string SupportTurnAround { get; }
        string SupportURL { get; }
        int CommandTimeout { get; }
    }
}
