using System;
using App.Common.InversionOfControl;
using System.Collections.Generic;
using App.Common.Logging;


namespace App.Common.Tasks
{
    /// <summary>
    /// Task
    /// </summary>
    public partial class Task
    {
        private bool _enabled;
        private readonly string _type;
        private readonly string _name;
        private readonly bool _stopOnError;
        private DateTime? _lastStartUtc;
        private DateTime? _lastSuccessUtc;
        private DateTime? _lastEndUtc;
        private bool _isRunning;

        /// <summary>
        /// Ctor for Task
        /// </summary>
        private Task()
        {
            this._enabled = true;
        }

        /// <summary>
        /// Ctor for Task
        /// </summary>
        /// <param name="task">Task </param>
        public Task(ScheduleTask task)
        {
            this._type = task.Type;
            this._enabled = task.Enabled;
            this._stopOnError = task.StopOnError;
            this._name = task.Name;
        }

        private ITask CreateTask()
        {
            ITask task = null;
            if (this.Enabled)
            {
                var type2 = System.Type.GetType(this._type);
                if (type2 != null)
                {
                    object instance = IoC.GetService(type2);
                    if (instance==null)
                    {
                        //not resolved
                        try
                        {
                            instance = Activator.CreateInstance(type2);
                        }
                        catch (MissingMethodException)
                        {
                            instance = ResolveUnregistered(type2);
                        }
                    }
                    task = instance as ITask;
                }
            }
            return task;
            //ITask task = null;
            //if (this.Enabled)
            //{
            //    var type2 = System.Type.GetType(this._type);
            //    if (type2 != null)
            //    {
            //        object instance=null;
            //        bool success = false;
            //        try
            //        {
            //            instance=IoC.GetService(type2);
            //            success = true;
            //        }
            //        catch
            //        {
            //        }
            //        if (!success)
            //        {
            //            //not resolved
            //            try
            //            {
            //                instance = Activator.CreateInstance(type2);
            //            }
            //            catch (MissingMethodException)
            //            {
            //                instance = ResolveUnregistered(type2);
            //            }
            //        }
            //        task = instance as ITask;
            //    }
            //}
            //return task;
        }
        
        /// <summary>
        /// Executes the task
        /// </summary>
        public void Execute()
        {
            this._isRunning = true;
            try
            {
                var task = this.CreateTask();
                if (task != null)
                {
                    this._lastStartUtc = DateTime.UtcNow;
                    task.Execute();
                    this._lastEndUtc = this._lastSuccessUtc = DateTime.UtcNow;
                }
            }
            catch (Exception exc)
            {
                this._enabled = !this.StopOnError;
                this._lastEndUtc = DateTime.UtcNow;
                
                //log error
                Logger.Log(LogLevel.Error,string.Format("Error while running the '{0}' schedule task. {1}", this._name, exc.Message), exc);
            }
            
            try
            {
                //find current schedule task
                var scheduleTaskService = IoC.GetService<IScheduleTaskService>();
                var scheduleTask = scheduleTaskService.GetTaskByType(this._type);
                if (scheduleTask != null)
                {
                    scheduleTask.LastStartUtc = this.LastStartUtc;
                    scheduleTask.LastEndUtc = this.LastEndUtc;
                    scheduleTask.LastSuccessUtc = this.LastSuccessUtc;
                    scheduleTaskService.UpdateTask(scheduleTask);
                }
            }
            catch (Exception exc)
            {
                Logger.Log(LogLevel.Error,string.Format("Error saving schedule task datetimes. Exception: {0}", exc));
            }
            this._isRunning = false;
        }

        /// <summary>
        /// A value indicating whether a task is running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this._isRunning;
            }
        }

        /// <summary>
        /// Datetime of the last start
        /// </summary>
        public DateTime? LastStartUtc
        {
            get
            {
                return this._lastStartUtc;
            }
        }

        /// <summary>
        /// Datetime of the last end
        /// </summary>
        public DateTime? LastEndUtc
        {
            get
            {
                return this._lastEndUtc;
            }
        }

        /// <summary>
        /// Datetime of the last success
        /// </summary>
        public DateTime? LastSuccessUtc
        {
            get
            {
                return this._lastSuccessUtc;
            }
        }

        /// <summary>
        /// A value indicating type of the task
        /// </summary>
        public string Type
        {
            get
            {
                return this._type;
            }
        }

        /// <summary>
        /// A value indicating whether to stop task on error
        /// </summary>
        public bool StopOnError
        {
            get
            {
                return this._stopOnError;
            }
        }

        /// <summary>
        /// Get the task name
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
        }

        /// <summary>
        /// A value indicating whether the task is enabled
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this._enabled;
            }
        }
        public object ResolveUnregistered(Type type)
        {
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = IoC.GetService(parameter.ParameterType);
                        if (service == null) throw new Exception("Unkown dependency");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (Exception)
                {

                }
            }
            throw new Exception("No contructor was found that had all the dependencies satisfied.");
        }
    }
}
