using App.Common.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WebFramework.Data.Domain;

namespace Web.Infrastructure
{
    public class ModelEventConsumer :
        IConsumer<EntityInserted<Setting>>,
        IConsumer<EntityUpdated<Setting>>,
        IConsumer<EntityDeleted<Setting>>
    {
        public void HandleEvent(EntityInserted<Setting> eventMessage)
        {
            HandleLogLevelSettingEvent(eventMessage.Entity);
        }
        public void HandleEvent(EntityUpdated<Setting> eventMessage)
        {
            HandleLogLevelSettingEvent(eventMessage.Entity);
        }
        public void HandleEvent(EntityDeleted<Setting> eventMessage)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(App.Common.Util.GetFullPath(App.Common.Util.LogConfigFilePath)));
            Util.ChangeLogLevels();
        }
        private void HandleLogLevelSettingEvent(Setting item)
        {
            string logLevelSettingPrefix = Util.SETTING_KEYS_LOG_LEVEL;
            string pattern = logLevelSettingPrefix + "(.+)";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = rgx.Match(item.Name);
            if (match.Success)
                Util.ChangeLogLevels(item);
        }
    }
}