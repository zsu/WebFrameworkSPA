using App.Common;
using App.Common.Caching;
using App.Common.Data;
using App.Common.Events;
using App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFramework.Data.Domain;

namespace Service
{
    public class SettingService : ISettingService
    {
        #region Constants
        private const string SETTINGS_ALL_KEY = "Setting.all";
        #endregion

        #region Fields

        private readonly IRepository<Setting, Guid> _settingRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private IActivityLogService _activityLogService;
        #endregion
        private enum ActivityType { AddSetting, DeleteSetting, UpdateSetting};

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="settingRepository">Setting repository</param>
        public SettingService(ICacheManager cacheManager, IRepository<Setting, Guid> settingRepository, IEventPublisher eventPublisher, IActivityLogService activityLogService)
        {
            this._cacheManager = cacheManager;
            this._settingRepository = settingRepository;
            this._eventPublisher = eventPublisher;
            _activityLogService = activityLogService;
        }

        #endregion

        #region Methods
        public IQueryable<Setting> Query()
        {
            return _settingRepository.Query;
        }
        public virtual bool SettingExists(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            int count = _settingRepository.Query.Where(x => x.Name == name.Trim()).Count();
            return count > 0 ? true : false;
        }
        /// <summary>
        /// Adds a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void AddSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");
            using (var scope = new UnitOfWorkScope())
            {
                _settingRepository.Add(setting);
                scope.Commit();
            }
            //cache
            if (clearCache)
                _cacheManager.RemoveByPattern(SETTINGS_ALL_KEY);

            //event notification
            _eventPublisher.EntityInserted(setting);
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Setting {0} is created.", setting.Name);
            ActivityLog item = new ActivityLog(ActivityType.AddSetting.ToString(), message.ToString());
            _activityLogService.Add(item);
        }

        /// <summary>
        /// Updates a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void UpdateSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");
            using (var scope = new UnitOfWorkScope())
            {
                _settingRepository.Update(setting);
                scope.Commit();
            }

            //cache
            if (clearCache)
                _cacheManager.RemoveByPattern(SETTINGS_ALL_KEY);

            //event notification
            _eventPublisher.EntityUpdated(setting);
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Setting {0} is updated.", setting.Name);
            ActivityLog item = new ActivityLog(ActivityType.UpdateSetting.ToString(), message.ToString());
            _activityLogService.Add(item);
        }

        /// <summary>
        /// Gets a setting by identifier
        /// </summary>
        /// <param name="settingId">Setting identifier</param>
        /// <returns>Setting</returns>
        public virtual Setting GetSettingById(Guid settingId)
        {
            if (settingId == default(Guid))
                return null;

            var setting = _settingRepository.GetById(settingId);
            return setting;
        }

        /// <summary>
        /// Get setting by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Setting object</returns>
        public virtual Setting GetSettingByKey(string key)
        {
            if (String.IsNullOrEmpty(key))
                return null;

            key = key.Trim().ToLowerInvariant();

            var settings = GetAllSettings();
            if (settings.ContainsKey(key))
            {
                var id = settings[key].Key;
                return GetSettingById(id);
            }

            return null;
        }


        /// <summary>
        /// Get setting value by key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Setting value</returns>
        public virtual T GetSettingByKey<T>(string key, T defaultValue = default(T))
        {
            if (String.IsNullOrEmpty(key))
                return defaultValue;

            key = key.Trim().ToLowerInvariant();

            var settings = GetAllSettings();
            if (settings.ContainsKey(key))
                return Util.ConvertTo<T>(settings[key].Value);

            return defaultValue;
        }

        /// <summary>
        /// Set setting value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void SetSetting<T>(string key, T value, bool clearCache = true)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            key = key.Trim().ToLowerInvariant();

            var settings = GetAllSettings();

            Setting setting = null;
            string valueStr = Util.ConvertTo<string>(value);
            if (settings.ContainsKey(key))
            {
                //update
                var settingId = settings[key].Key;
                setting = GetSettingById(settingId);
                setting.Value = valueStr;
                UpdateSetting(setting, clearCache);
            }
            else
            {
                //insert
                setting = new Setting()
                {
                    Name = key,
                    Value = valueStr,
                };
                AddSetting(setting, clearCache);
            }
        }

        public virtual bool DeleteSetting(Guid id)
        {
            if (id == default(Guid))
                throw new ArgumentNullException("id");
            Setting setting = _settingRepository.Query.FirstOrDefault(x => x.Id == id);
            if (setting == null)
                return false;
            using (var scope = new UnitOfWorkScope())
            {
                _settingRepository.Delete(setting);
                scope.Commit();
            }

            //cache
            _cacheManager.RemoveByPattern(SETTINGS_ALL_KEY);

            //event notification
            _eventPublisher.EntityDeleted(setting);
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Setting {0} is deleted.", setting.Name);
            ActivityLog item = new ActivityLog(ActivityType.DeleteSetting.ToString(), message.ToString());
            _activityLogService.Add(item);
            return true;
        }
        /// <summary>
        /// Deletes a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        public virtual bool DeleteSetting(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");
            using (var scope = new UnitOfWorkScope())
            {
                _settingRepository.Delete(setting);
                scope.Commit();
            }

            //cache
            _cacheManager.RemoveByPattern(SETTINGS_ALL_KEY);

            //event notification
            _eventPublisher.EntityDeleted(setting);
            return true;
        }

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Setting collection</returns>
        /// Changed by: Zhicheng Su
        public virtual IDictionary<string, KeyValuePair<Guid, string>> GetAllSettings()
        {
            //cache
            string key = string.Format(SETTINGS_ALL_KEY);
            return _cacheManager.Get(key, () =>
            {
                var query = from s in _settingRepository.Query
                            orderby s.Name
                            select s;
                var settings = query.ToList();
                //format: <name, <id, value>>
                var dictionary = new Dictionary<string, KeyValuePair<Guid, string>>(StringComparer.InvariantCultureIgnoreCase);
                foreach (var s in settings)
                {
                    var resourceName = s.Name.ToLowerInvariant();
                    if (!dictionary.ContainsKey(resourceName))
                        dictionary.Add(s.Name, new KeyValuePair<Guid, string>(s.Id, s.Value));
                }
                return dictionary;
            });
        }

        ///// <summary>
        ///// Save settings object
        ///// </summary>
        ///// <typeparam name="T">Type</typeparam>
        ///// <param name="settingInstance">Setting instance</param>
        //public virtual void SaveSetting<T>(T settingInstance) where T : ISettings, new()
        //{
        //    //We should be sure that an appropriate ISettings object will not be cached in IoC tool after updating (by default cached per HTTP request)
        //    EngineContext.Current.Resolve<IConfigurationProvider<T>>().SaveSettings(settingInstance);
        //}

        ///// <summary>
        ///// Delete all settings
        ///// </summary>
        ///// <typeparam name="T">Type</typeparam>
        //public virtual void DeleteSetting<T>() where T : ISettings, new()
        //{
        //    EngineContext.Current.Resolve<IConfigurationProvider<T>>().DeleteSettings();
        //}

        /// <summary>
        /// Clear cache
        /// </summary>
        public virtual void ClearCache()
        {
            _cacheManager.RemoveByPattern(SETTINGS_ALL_KEY);
        }
        #endregion Methods
    }
}
