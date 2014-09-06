#What is WebFrameworkSPA

WebFrameworkSPA is a Single Page Application(SPA) framework built on top of AngularJS, Web Api.

Some of the features of WebFrameworkSPA are:

  * User managment(authentication, authorization)
  * Claim based authorization with two level Role/Permission management
  * Configuration management(system settings, email template)
  * Logging system(server side, client side, activities log, authentication audit)
  * Generic repository pattern with Nhibernate and Entity Framework Provider
  * Server side multiple fields sorting, paging and filtering with different operators
  * Messaging
  * Web Server management(Refresh Cache, Restart AppPool, Application offline scheduler)  

###Live Demo: https://webframeworkspa.azurewebsites.net/
Login: user1/Abc123

#Getting started with WebFrameworkSPA
There are two web applications: WebFramework is the GUI web application using AngularJS; WebFramework.Service is the Web Service application using asp.net Web Api 2.
##Configure WebFramework.Service
  * Restore Nuget package
  * Build solution
  * Copy the encrypted string inside \<EncryptedData\> in web.config; 
     Run slib.crytoutil.exe under slib.crytoutil\bin\debug;
     Pasted it in slib.crytoutil.exe and decrypt it; 
     Change the  decrypted connectionstring as needed to point to your own sql server;
     Encrypt the new connectionstring and copy back to \<EncryptedData\> section.
  * Find the following line in web.config; Change it to your email
```XML
	<add key="AdminEmail" value="xxx" />
```

  * Find the following line in web.config; Change it to your smtp server
```XML
      <smtp from="noreply-adminweb@xxx.xxx">
        <network host="smtp.gmail.com" userName="aaa@aaa.aaa" password="aaa" port="587" enableSsl="true" />
      </smtp>
```
  * Find the following in WebFramework\Web\Configurations\log.config;
    Change it to your email address
```XML
<appender name="SmtpAppender" type="log4net.Appender.SmtpAppender">
     <to value="aaa@aaa.aaa" />
```
  * Change CORS Origin setting in configurations\application.config if needed
```XML
<add key="CorsOrigin" value="YourGuiWebAppDomain" />
```
  * Run the application; 

## WebFramework Configuration
WebFramework allows for a lot of flexibility in configing the system. 

### Change configuration on the fly through the setting page. It has these properties:

* logsettings.loglevel.NHibernate.SQL	Warn
* logsettings.loglevel.Root	Info
* maintenance.message	The site is under maintenance. Please contact Customer Support for more information.
* maintenance.start	06/21/2014 03:15:00
* maintenance.warninglead	31536000
* maintenance.end	06/30/2014 03:15:00
* maintenance.warningmessage	The site is going to be down for maintenance at {0}.

### Change log config on the fly through log.config file

Cofigurations\log.config is the configuration file for log4net. You can add different loggers or change what get logged here and the application will pick up the changes immediately. The log level will be overwritten by the logsettings.loglevel.Root/ logsettings.loglevel.NHibernate.SQL in the In app config if specified.

### Change application related information on the fly through application.config file

Configurations\application.config contains application specific information that can be changed on the fly

### Change Database setting

Configurations\hibernate.cfg.config is the default configuration file for nhibernate. 
There are three sets of tables: Security, Log and App; 
You can have hibernate.security.cfg.config, hibernate.log.cfg.config and hibernate.app.cfg.config files to override the default settings for different table sets so they can be on different database servers if needed. 

The encrypted connectionstrings in web.config also has three corresponding connectionstrings:
```XML
<connectionStrings>
	<add name="SecurityDB" connectionString="Data Source=|DataDirectory|WebFramework.sdf;Enlist=false;" providerName="System.Data.SqlServerCe.4.0" />
	<add name="LogDB" connectionString="Data Source=|DataDirectory|WebFramework.sdf;Enlist=false;" providerName="System.Data.SqlServerCe.4.0" />
	<add name="AppDB" connectionString="Data Source=|DataDirectory|WebFramework.sdf;Enlist=false;" providerName="System.Data.SqlServerCe.4.0" />
</connectionStrings>
```
##Configure WebFrameworkSPA
Find the following in src\Web\app\infrastructure\tool.js:
ttTools.cloudUrl = "WebFrameworkService/";
Change the url to the one you used to host the WebFramework.Service;

#Screenshots
![WebFramework](screenshots/home.jpg?raw=true "home")
![WebFramework](screenshots/login.jpg?raw=true "login")
![WebFramework](screenshots/menu.jpg?raw=true "menu")
![WebFramework](screenshots/log.jpg?raw=true "log")
![WebFramework](screenshots/user.jpg?raw=true "user")
![WebFramework](screenshots/filter.jpg?raw=true "filter")
![WebFramework](screenshots/maintenance.jpg?raw=true "maintenance")
![WebFramework](screenshots/activitylog.jpg?raw=true "activitylog")
![WebFramework](screenshots/authenticationaudit.jpg?raw=true "authenticationaudit")
![WebFramework](screenshots/role.jpg?raw=true "role")
![WebFramework](screenshots/permission.jpg?raw=true "permission")
![WebFramework](screenshots/userrole.jpg?raw=true "userrole")
![WebFramework](screenshots/rolepermission.jpg?raw=true "rolepermission")
![WebFramework](screenshots/roleuserlist.jpg?raw=true "roleuserlist")
![WebFramework](screenshots/setting.jpg?raw=true "setting")
![WebFramework](screenshots/messagetemplate.jpg?raw=true "messagetemplate")
