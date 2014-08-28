using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using App.Common.InversionOfControl;
using System.Threading;

namespace App.Common.Tasks
{
    /// <summary>
    /// Represents schedule task manager
    /// </summary>
    public partial class TaskManager
    {
        private static volatile TaskManager _instance;
        private static object _syncRoot = new object();
        private ReaderWriterLockSlim listLock = new ReaderWriterLockSlim();
        private readonly List<TaskThread> _taskThreads = new List<TaskThread>();

        private TaskManager()
        {
        }
        public void Refresh()//TODO: change to async System.Threading.Tasks.Task when upgrade to .net 4.5
        {
            Stop();
            Initialize();
            Start();
        }
        /// <summary>
        /// Initializes the task manager.
        /// </summary>
        public void Initialize()
        {
            listLock.EnterWriteLock();
            try
            {
                this._taskThreads.Clear();

                var taskService = IoC.GetService<IScheduleTaskService>();
                var scheduleTasks = taskService
                    .GetAllTasks()
                    .OrderBy(x => x.Seconds)
                    .ToList();

                //group by threads with the same seconds
                foreach (var scheduleTaskGrouped in scheduleTasks.GroupBy(x => x.Seconds))
                {
                    //create a thread
                    var taskThread = new TaskThread();
                    taskThread.Seconds = scheduleTaskGrouped.Key;
                    this._taskThreads.Add(taskThread);
                    foreach (var scheduleTask in scheduleTaskGrouped)
                    {
                        var task = new Task(scheduleTask);
                        taskThread.AddTask(task);
                    }
                }
            }
            finally
            { listLock.ExitWriteLock(); }

            //one thread, one task
            //foreach (var scheduleTask in scheduleTasks)
            //{
            //    var taskThread = new TaskThread(scheduleTask);
            //    this._taskThreads.Add(taskThread);
            //    var task = new Task(scheduleTask);
            //    taskThread.AddTask(task);
            //}
        }

        /// <summary>
        /// Starts the task manager
        /// </summary>
        public void Start()
        {
            listLock.EnterReadLock();
            try
            {
                foreach (var taskThread in this._taskThreads)
                {
                    taskThread.InitTimer();
                }
            }
            finally
            {
                listLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Stops the task manager
        /// </summary>
        public void Stop()
        {
            listLock.EnterReadLock();
            try
            {
                foreach (var taskThread in this._taskThreads)
                {
                    taskThread.Dispose();
                }
            }
            finally
            {
                listLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Gets the task mamanger instance
        /// </summary>
        public static TaskManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new TaskManager();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Gets a list of task threads of this task manager
        /// </summary>
        public IList<TaskThread> TaskThreads
        {
            get
            {
                listLock.EnterReadLock();
                try
                {
                    return new ReadOnlyCollection<TaskThread>(this._taskThreads);
                }
                finally
                {
                    listLock.ExitReadLock();
                }
            }
        }
    }
}
