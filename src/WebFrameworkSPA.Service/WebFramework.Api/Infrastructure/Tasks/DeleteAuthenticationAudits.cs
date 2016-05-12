using WebFramework.Data.Domain;
using App.Common.InversionOfControl;
using App.Common.Tasks;
using Service;
using System;
using System.Text;

namespace Web.Infrastructure.Tasks
{
    public partial class DeleteAuthenticationAuditsTask : ITask
    {
        private enum ActivityType { StartScheduledTask, EndScheduledTask};
        public DeleteAuthenticationAuditsTask()
        {
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            var settingService = IoC.GetService<ISettingService>();
            var activityLogService = IoC.GetService<IActivityLogService>();
            StringBuilder message = new StringBuilder();
            message.AppendLine("Scheduled task DeleteAuthenticationAuditsTask is started.");
            ActivityLog activityItem = new ActivityLog(ActivityType.StartScheduledTask.ToString(), message.ToString());
            activityLogService.Add(activityItem);
            var olderThanMinutes = settingService.GetSettingByKey<int>(Constants.SETTING_KEYS_SCHEDULEDTASK_AUTHENTICATION_AUDIT_EXPIRATION, 60 * 24 * 90); 
            Util.DeleteAuthenticationAudits(DateTime.UtcNow.AddMinutes(-olderThanMinutes).Date);
            message.Clear();
            message.AppendLine("Scheduled task DeleteAuthenticationAuditsTask is finished successfully.");
            activityItem = new ActivityLog(ActivityType.EndScheduledTask.ToString(), message.ToString());
            activityLogService.Add(activityItem);
        }
    }
}