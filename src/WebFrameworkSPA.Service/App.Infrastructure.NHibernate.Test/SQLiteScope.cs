using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Data.SQLite;
using NHibernateCfg = NHibernate.Cfg;
public class SQLiteScope<TClassFromMappingAssembly> : IDisposable
{


    private const string ConnectionString = "Data Source=:memory:;Version=3;New=True;";
    public SQLiteScope()
    {
        BuildConfiguration();
    }

    private SQLiteConnection _connection;

    public ISessionFactory SessionFactory { get; private set; }
    private void BuildConfiguration()
    {
        var cnf= Fluently.Configure().Database(GetDBConfig()).Mappings(GetMappings).ExposeConfiguration(BuildSchema);
        var config = cnf.BuildConfiguration()
                        .SetProperty(NHibernateCfg.Environment.ReleaseConnections, "on_close");
        SessionFactory = config.BuildSessionFactory();
    }

    private FluentNHibernate.Cfg.Db.IPersistenceConfigurer GetDBConfig()
    {
        return SQLiteConfiguration.Standard.ConnectionString((ConnectionStringBuilder cs) => cs.Is(ConnectionString));
    }

    private void GetMappings(MappingConfiguration x)
    {
        x.FluentMappings.AddFromAssemblyOf<TClassFromMappingAssembly>().ExportTo(".");
    }

    private void BuildSchema(NHibernate.Cfg.Configuration Cfg)
    {
        SchemaExport SE = new SchemaExport(Cfg);
        SE.Execute(false, true, false, GetConnection(), null);
    }

    private System.Data.SQLite.SQLiteConnection GetConnection()
    {
        if (_connection == null)
        {
            _connection = new SQLiteConnection(ConnectionString);
            _connection.Open();
        }
        return _connection;
    }

    public ISession OpenSession()
    {
        return SessionFactory.OpenSession(GetConnection());
    }

    // To detect redundant calls
    private bool disposedValue = false;

    // IDisposable
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                // TODO: free other state (managed objects).
                if (_connection != null)
                    _connection.Close();
                _connection = null;
            }

            // TODO: free your own state (unmanaged objects).
            // TODO: set large fields to null.
        }
        this.disposedValue = true;
    }

    #region " IDisposable Support "
    // This code added by Visual Basic to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion

}
