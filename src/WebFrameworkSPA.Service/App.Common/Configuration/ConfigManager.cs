/// Author: Zhicheng Su
using System;
using System.Collections.Generic;
using App.Common.Caching;
using App.Common.InversionOfControl;
using System.Runtime.Caching;

namespace App.Common.Configuration
{
    public class ConfigManager<TInterface, TImplementation> : IConfigManager<TInterface>
    {
        #region Fields
        private string _configFilePath;
        #endregion
        #region Properties
        public string ConfigFilePath
        { get { return _configFilePath; } }
        #endregion Properties
        #region Methods
        public ConfigManager(string configFilePath)
        {
            Check.IsNotEmpty(configFilePath, "configFilePath");
            _configFilePath = configFilePath;
        }
        public void Clear()
        {
            ICacheManager cache = IoC.GetService<ICacheManager>(Util.ApplicationCacheKey);
            cache.Remove(_configFilePath);
        }

        public TInterface GetConfig()
        {
            TInterface config = default(TInterface);
            ICacheManager cache = IoC.GetService<ICacheManager>(Util.ApplicationCacheKey);
            config=cache.Get<TInterface>(_configFilePath);
            if (cache.Get<TInterface>(_configFilePath) == null)
            {
                object[] args = new object[] { _configFilePath };
                config = (TInterface)Activator.CreateInstance(typeof(TImplementation), args);
                CacheItemPolicy policy = new CacheItemPolicy();
                List<string> filePaths = new List<string>();
                filePaths.Add(_configFilePath);
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));
                cache.Set(_configFilePath, config, policy);
            }
            return config;
        }

        #endregion
    }
}
