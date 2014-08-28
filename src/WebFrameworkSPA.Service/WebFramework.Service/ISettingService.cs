using System;
using System.Collections.Generic;
using System.Linq;
using WebFramework.Data.Domain;
namespace Service
{
    public interface ISettingService:IService
    {
        IQueryable<Setting> Query();
        bool SettingExists(string name);
        void AddSetting(Setting setting, bool clearCache = true);
        void ClearCache();
        bool DeleteSetting(Guid id);
        bool DeleteSetting(Setting setting);
        IDictionary<string, KeyValuePair<Guid, string>> GetAllSettings();
        Setting GetSettingById(Guid settingId);
        Setting GetSettingByKey(string key);
        T GetSettingByKey<T>(string key, T defaultValue = default(T));
        void SetSetting<T>(string key, T value, bool clearCache = true);
        void UpdateSetting(Setting setting, bool clearCache = true);
    }
}
