using Damage.DataAccess.Models;
using Damage.DataAccess.Repositories;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;

namespace Damage.DataAccess
{
    public class UnitOfWork : IDisposable
    {


        protected ISessionFactory SessionFactory = null;

        protected ISession Session = null;

        #region "Repositories"
        private UserGadgetRepository _userGadgetRepository;
        public UserGadgetRepository UserGadgetRepository
        {
            get
            {
                if (_userGadgetRepository == null)
                {
                    _userGadgetRepository = new UserGadgetRepository(Session);
                }
                return _userGadgetRepository;
            }
        }

        private GadgetRepository _gadgetRepository;
        public GadgetRepository GadgetRepository
        {
            get
            {
                if (_gadgetRepository == null)
                {
                    _gadgetRepository = new GadgetRepository(Session);
                }
                return _gadgetRepository;
            }
        }

        private UserRepository _userRepository;
        public UserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(Session);
                }
                return _userRepository;
            }
        }

        #endregion

        public UnitOfWork(string connectionString)
        {
            //Configure a new NHivebernate Session
            SessionFactory = ConfigureNHibernate(connectionString);
            Session = SessionFactory.OpenSession();
            Session.FlushMode = FlushMode.Commit;
        }

        private ISessionFactory ConfigureNHibernate(string connectionString)
        {
            //HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
            return Fluently.Configure().Database(new SqlServerConfiguration(connectionString)).Mappings(m => m.FluentMappings.AddFromAssemblyOf<BaseModel>()).ExposeConfiguration(cfg => cfg.SetProperty("hbm2ddl.keywords", "none")).BuildSessionFactory();
        }

        #region "IDisposable Support"
        // To detect redundant calls
        private bool _disposedValue;

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    //Set repositories to null
                    _userGadgetRepository = null;
                    _userGadgetRepository = null;
                    _gadgetRepository = null;
                    Session.Dispose();
                    SessionFactory.Dispose();
                }
            }
            _disposedValue = true;
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
