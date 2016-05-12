/// Author: Zhicheng Su
using System;
using System.IO;
using System.Threading;

namespace App.Common
{
    public sealed class FileWatchHandler : IDisposable
    {
        /// <summary>
        /// Holds the FileInfo used to configure the XmlConfigurator
        /// </summary>
        private FileInfo _configFileInfo;

        /// <summary>
        /// Holds the repository being configured.
        /// </summary>
        private IConfigurable _repository;

        /// <summary>
        /// The timer used to compress the notification events.
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// The default amount of time to wait after receiving notification
        /// before reloading the config file.
        /// </summary>
        private const int TimeoutMillis = 500;

        /// <summary>
        /// Watches file for changes. This object should be disposed when no longer
        /// needed to free system handles on the watched resources.
        /// </summary>
        private FileSystemWatcher _watcher;
        
        /// <summary>
        /// Watch a specified config file used to configure an object
        /// </summary>
        /// <param name="repository">The repository to configure.</param>
        /// <param name="configFile">The configuration file to watch.</param>
        /// <remarks>
        /// <para>
        /// Watch a specified config file used to configure an object
        /// </para>
        /// </remarks>
        public void StartWatching()
        {
            // Create a new FileSystemWatcher and set its properties.
            _watcher = new FileSystemWatcher();
            _watcher.Path = _configFileInfo.DirectoryName;
            _watcher.Filter = _configFileInfo.Name;

            // Set the notification filters
            _watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName;

            // Add event handlers. OnChanged will do for all event handlers that fire a FileSystemEventArgs
            _watcher.Changed += new FileSystemEventHandler(FileWatchHandler_OnChanged);
            _watcher.Created += new FileSystemEventHandler(FileWatchHandler_OnChanged);
            _watcher.Deleted += new FileSystemEventHandler(FileWatchHandler_OnChanged);
            _watcher.Renamed += new RenamedEventHandler(FileWatchHandler_OnRenamed);

            // Begin watching.
            _watcher.EnableRaisingEvents = true;

            // Create the timer that will be used to deliver events. Set as disabled
            _timer = new Timer(new TimerCallback(OnWatchedFileChange), null, Timeout.Infinite, Timeout.Infinite);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileWatchHandler" /> class.
        /// </summary>
        /// <param name="repository">The repository to configure.</param>
        /// <param name="configFile">The configuration file to watch.</param>
        /// <remarks>
        /// <para>
        /// Initializes a new instance of the <see cref="FileWatchHandler" /> class.
        /// </para>
        /// </remarks>
        public FileWatchHandler(IConfigurable repository, FileInfo configFile)
        {
            _repository = repository;
            _configFileInfo = configFile;

        }

        /// <summary>
        /// Event handler used by <see cref="FileWatchHandler_OnChanged"/>.
        /// </summary>
        /// <param name="source">The <see cref="FileSystemWatcher"/> firing the event.</param>
        /// <param name="e">The argument indicates the file that caused the event to be fired.</param>
        /// <remarks>
        /// <para>
        /// This handler reloads the configuration from the file when the event is fired.
        /// </para>
        /// </remarks>
        private void FileWatchHandler_OnChanged(object source, FileSystemEventArgs e)
        {
            // Deliver the event in TimeoutMillis time
            // timer will fire only once
            _timer.Change(TimeoutMillis, Timeout.Infinite);
        }

        /// <summary>
        /// Event handler used by <see cref="FileWatchHandler"/>.
        /// </summary>
        /// <param name="source">The <see cref="FileSystemWatcher"/> firing the event.</param>
        /// <param name="e">The argument indicates the file that caused the event to be fired.</param>
        /// <remarks>
        /// <para>
        /// This handler reloads the configuration from the file when the event is fired.
        /// </para>
        /// </remarks>
        private void FileWatchHandler_OnRenamed(object source, RenamedEventArgs e)
        {

            // Deliver the event in TimeoutMillis time
            // timer will fire only once
            _timer.Change(TimeoutMillis, Timeout.Infinite);
        }

        /// <summary>
        /// Called by the timer when the configuration has been updated.
        /// </summary>
        /// <param name="state">null</param>
        private void OnWatchedFileChange(object state)
        {
            try
            {
                _repository.Configure(_configFileInfo.FullName);
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// Release the handles held by the watcher and timer.
        /// </summary>
        public void Dispose()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
            }
            if(_timer!=null)
                _timer.Dispose();
        }

    }
}
