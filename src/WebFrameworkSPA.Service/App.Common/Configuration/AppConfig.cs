/// Author: Zhicheng Su
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Threading;
using System.Xml;
using System.Data.SqlClient;

namespace App.Common
{
	public class AppConfigManager
	{

		#region Fields

		private static string _configFile = null;
        private static IAppConfig _appConfig = null;
		private static readonly object _syncRoot = new object();

		#endregion

		#region Methods

		public static void Clear() {
			lock (_syncRoot) {
				_appConfig = null;
				_configFile = null;
			}
		}

        public static IAppConfig GetConfig(string configFilePath)
        {
			if (_appConfig != null && !string.IsNullOrEmpty( _configFile ) && _configFile.Trim().ToLower() == configFilePath.Trim().ToLower())
				return _appConfig;
			lock (_syncRoot) {
				if (_appConfig == null || string.IsNullOrEmpty( _configFile ) || _configFile.Trim().ToLower() != configFilePath.Trim().ToLower()) {
					_appConfig = new AppConfig(configFilePath);
					_configFile = configFilePath;
				}
			}
			return _appConfig;
		}

		#endregion

	}
	public class AppConfig : IAppConfig, IConfigurable, IDisposable
	{

		#region Fields

        private bool disposed = false; 
        private ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();
        private ReadOnlyNameValueCollection _readonlyCollection = null;
        private const string ApplicationNameKey = "ApplicationName",
                                    ApplicationAcronymKey = "ApplicationAcronym",
                                    ApplicationVersionKey = "ApplicationVersion",
                                    CopyRightKey = "Copyright",
                                    DateTimeFormatKey = "DateTimeFormat",
                                    SupportOrganizationKey = "SupportOrganization",
                                    SupportAddress1Key = "SupportAddress1",
                                    SupportAddress2Key = "SupportAddress2",
                                    SupportPhoneKey = "SupportPhone",
                                    SupportHoursKey = "SupportHours",
                                    SupportEmailKey = "SupportEmail",
                                    SupportUrlKey = "SupportURL",
                                    SupportTurnAroundKey = "SupportTurnAround",
                                    ProductProviderKey = "ProductProvider",
                                    ProductProviderPhoneKey = "ProductProviderPhone",
                                    ProductProviderEmailKey = "ProductProviderEmail",
                                    ProductProviderUrlKey = "ProductProviderURL",
                                    OfflineFilePathKey = "OfflineFilePath",
                                    SecurityCacheEnableKey = "SecurityCacheEnable",
                                    PrivacyPolicyUrlKey = "PrivacyPolicyURL",
                                    CommandTimeoutKey="CommandTimeout";


		#endregion

		#region Constructors

		public AppConfig( string configFilePath ) {

            Configure(configFilePath);
		}

		#endregion

		#region Properties

		public string AppAcronym {
			get { return GetProperty( ApplicationAcronymKey ); }
		}

		public string AppFullName {
			get { return GetProperty( ApplicationNameKey ); }
		}

		public string AppVersion {
			get { return GetProperty( ApplicationVersionKey ); }
		}

		public string Copyright {
			get { return GetProperty( CopyRightKey ); }
		}

		public string DateTimeFormat {
			get { return GetProperty( DateTimeFormatKey ); }
		}

		public string ProductProvider {
			get { return GetProperty( ProductProviderKey ); }
		}

		public string ProductProviderEmail {
			get { return GetProperty( ProductProviderEmailKey ); }
		}

		public string ProductProviderPhone {
			get { return GetProperty( ProductProviderPhoneKey ); }
		}

		public string ProductProviderURL {
			get { return GetProperty( ProductProviderUrlKey ); }
		}

		public string SupportAddress1 {
			get { return GetProperty( SupportAddress1Key ); }
		}

		public string SupportAddress2 {
			get { return GetProperty( SupportAddress2Key ); }
		}

		public string SupportEmail {
			get { return GetProperty( SupportEmailKey ); }
		}

		public string SupportHours {
			get { return GetProperty( SupportHoursKey ); }
		}

		public string SupportOrganization {
			get { return GetProperty( SupportOrganizationKey ); }
		}

		public string SupportPhone {
			get { return GetProperty( SupportPhoneKey ); }
		}

		public string SupportTurnAround {
			get { return GetProperty( SupportTurnAroundKey ); }
		}

		public string SupportURL {
			get { return GetProperty( SupportUrlKey ); }
		}

		public string OfflineFilePath {
			get { return GetProperty( OfflineFilePathKey ); }
		}

		public string SecurityCacheEnable {
			get { return GetProperty( SecurityCacheEnableKey ); }
		}

		public string PrivacyPolicyURL {
			get { return GetProperty( PrivacyPolicyUrlKey ); }
		}
        public int CommandTimeout{
            get{
                string timeout=GetProperty(CommandTimeoutKey);
                int iTimeout;
                if(timeout==null || timeout.Trim()==string.Empty || !int.TryParse(timeout,out iTimeout))
                {
                    SqlCommand command = new SqlCommand(); 
                    command.Dispose();
                    iTimeout= command.CommandTimeout;             
                }
                return iTimeout;
            }
        }
		#endregion

		#region Methods

		public string GetProperty( string key ) {
            _locker.EnterReadLock();
            try
            {
                if (_readonlyCollection != null && !string.IsNullOrEmpty(_readonlyCollection[key]))
                    return _readonlyCollection[key];
                else
                    return string.Empty;
            }
            finally
            {
                _locker.ExitReadLock();
            } 
		}

        public void Configure(string configFilePath)
        {
            string keyAttriuteName = "key", valueAttributeName = "value";
            Check.IsNotEmpty(configFilePath, "configFilePath");
            configFilePath=Util.GetFullPath(configFilePath);
            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException(String.Format(AppCommon.File_Not_Found, configFilePath));
            }
            ConfigXmlDocument document1 = new ConfigXmlDocument();
            try
            {
                document1.Load(configFilePath);
            }
            catch (XmlException exception1)
            {
                throw new ConfigurationErrorsException(exception1.Message, exception1, configFilePath, exception1.LineNumber);
            }
            XmlNode section = document1.DocumentElement;
            _locker.EnterWriteLock();
            try
            {
                _readonlyCollection = new ReadOnlyNameValueCollection(StringComparer.OrdinalIgnoreCase);
                HandlerBase.CheckForUnrecognizedAttributes(section);
                foreach (XmlNode node1 in section.ChildNodes)
                {
                    if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(node1))
                    {
                        if (node1.Name == "add")
                        {
                            string text1 = HandlerBase.RemoveRequiredAttribute(node1, keyAttriuteName);
                            string text2 = HandlerBase.RemoveRequiredAttribute(node1, valueAttributeName, true);
                            HandlerBase.CheckForUnrecognizedAttributes(node1);
                            _readonlyCollection[text1] = text2;
                        }
                        else
                        {
                            if (node1.Name == "remove")
                            {
                                string text3 = HandlerBase.RemoveRequiredAttribute(node1, keyAttriuteName);
                                HandlerBase.CheckForUnrecognizedAttributes(node1);
                                _readonlyCollection.Remove(text3);
                                continue;
                            }
                            if (node1.Name.Equals("clear"))
                            {
                                HandlerBase.CheckForUnrecognizedAttributes(node1);
                                _readonlyCollection.Clear();
                                continue;
                            }
                            HandlerBase.ThrowUnrecognizedElement(node1);
                        }
                    }
                }
                _readonlyCollection.SetReadOnly();
            }
            finally
            {
                _locker.ExitWriteLock(); 
            }
            FileWatchHandler fileWatchHandler = new FileWatchHandler(this, new FileInfo(configFilePath));
            fileWatchHandler.StartWatching();
        }
		#endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if(_locker!=null)
                        _locker.Dispose();
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.


                // Note disposing has been done.
                disposed = true;
            }
        }
        #endregion
    }
}
