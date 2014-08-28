using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Common.InversionOfControl;
using App.Common.TypeFinder;

namespace App.Common.Tasks
{
    public class StartupTaskManager
    {
        private static readonly StartupTaskManager _taskManager = new StartupTaskManager();

        private StartupTaskManager()
        {
        }

        /// <summary>
        /// Gets the task mamanger instance
        /// </summary>
        public static StartupTaskManager Instance
        {
            get
            {
                return _taskManager;
            }
        }

        public void Start()
        {
            var typeFinder = IoC.GetService<ITypeFinder>();
            var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
            var startUpTasks = new List<IStartupTask>();
            foreach (var startUpTaskType in startUpTaskTypes)
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            //sort
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            foreach (var startUpTask in startUpTasks)
                startUpTask.Execute();
        }
    }
}
