using Damage.DataAccessEF.Contexts;
using System;

namespace Damage.DataAccessEF
{
    public class UnitOfWork : IDisposable
    {
        private readonly string _connectionString = "DefaultConnection";
        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
        }

        private UserGadgetsContext _userGadgetsContext;
        public UserGadgetsContext UserGadgetsContext
        {
            get { return _userGadgetsContext ?? (_userGadgetsContext = new UserGadgetsContext(_connectionString)); }
        }

        private GadgetsContext _gadgetsContext;
        public GadgetsContext GadgetsContext
        {
            get { return _gadgetsContext ?? (_gadgetsContext = new GadgetsContext(_connectionString)); }
        }

        private UsersContext _usersContext;
        public UsersContext UsersContext
        {
            get { return _usersContext ?? (_usersContext = new UsersContext(_connectionString)); }
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
                    //Set contexts to null
                    if (_gadgetsContext != null)
                    {
                        _gadgetsContext.Dispose();
                        _gadgetsContext = null;
                    }
                    if (_userGadgetsContext != null)
                    {
                        _userGadgetsContext.Dispose();
                        _userGadgetsContext = null;
                    }
                    if (_usersContext != null)
                    {
                        _usersContext.Dispose();
                        _usersContext = null;
                    }
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
}
