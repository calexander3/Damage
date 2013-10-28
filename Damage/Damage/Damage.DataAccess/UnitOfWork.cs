using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using Damage.DataAccess.Models;
using Damage.DataAccess.Repositories;

namespace Damage.DataAccess
{
    public class UnitOfWork : IDisposable
    {


        protected ISessionFactory m_SessionFactory = null;

        protected ISession m_Session = null;

        #region "Repositories"
        private UserGadgetRepository m_UserGadgetRepository = null;
        public UserGadgetRepository UserGadgetRepository
        {
            get
            {
                if (m_UserGadgetRepository == null)
                {
                    m_UserGadgetRepository = new UserGadgetRepository(m_Session);
                }
                return m_UserGadgetRepository;
            }
        }

        private GadgetRepository m_GadgetRepository = null;
        public GadgetRepository GadgetRepository
        {
            get
            {
                if (m_GadgetRepository == null)
                {
                    m_GadgetRepository = new GadgetRepository(m_Session);
                }
                return m_GadgetRepository;
            }
        }

        private UserRepository m_UserRepository = null;
        public UserRepository UserRepository
        {
            get
            {
                if (m_UserRepository == null)
                {
                    m_UserRepository = new UserRepository(m_Session);
                }
                return m_UserRepository;
            }
        }

        #endregion

        public UnitOfWork(string ConnectionString)
        {
            //Configure a new NHivebernate Session
            m_SessionFactory = ConfigureNHibernate(ConnectionString);
            m_Session = m_SessionFactory.OpenSession();
            m_Session.FlushMode = FlushMode.Commit;
        }

        private ISessionFactory ConfigureNHibernate(string ConnectionString)
        {
            //HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
            return Fluently.Configure().Database(new SqlServerConfiguration(ConnectionString)).Mappings(m => m.FluentMappings.AddFromAssemblyOf<BaseModel>()).BuildSessionFactory();
        }

        #region "IDisposable Support"
        // To detect redundant calls
        private bool disposedValue;

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    //Set repositories to null
                    m_UserGadgetRepository = null;
                    m_GadgetRepository = null;
                    m_Session.Dispose();
                    m_SessionFactory.Dispose();
                }
            }
            this.disposedValue = true;
        }


        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class SqlServerConfiguration : PersistenceConfiguration<SqlServerConfiguration, MsSqlConnectionStringBuilder>
    {
        public SqlServerConfiguration(string conString)
        {
            Driver<NHibernate.Driver.SqlClientDriver>();
            ConnectionString(conString);
            Dialect<NHibernate.Dialect.MsSql2012Dialect>();
            //ShowSql();
            IsolationLevel(System.Data.IsolationLevel.ReadCommitted);
        }
    }
}
