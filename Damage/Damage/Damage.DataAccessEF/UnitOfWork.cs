using Damage.DataAccessEF.Contexts;
using System;

namespace Damage.DataAccessEF
{
    public class UnitOfWork : IDisposable
    {
        private string _connectionString = "DefaultConnection";
        public UnitOfWork(string ConnectionString)
        {
            _connectionString = ConnectionString;
        }

        private UserGadgetsContext _userGadgetsContext = null;
        public UserGadgetsContext UserGadgetsContext
        {
            get
            {
                if (_userGadgetsContext == null)
                {
                    _userGadgetsContext = new UserGadgetsContext(_connectionString);
                }
                return _userGadgetsContext;
            }
        }

        private GadgetsContext _gadgetsContext = null;
        public GadgetsContext GadgetsContext
        {
            get
            {
                if (_gadgetsContext == null)
                {
                    _gadgetsContext = new GadgetsContext(_connectionString);
                }
                return _gadgetsContext;
            }
        }

        private UsersContext _usersContext = null;
        public UsersContext UsersContext
        {
            get
            {
                if (_usersContext == null)
                {
                    _usersContext = new UsersContext(_connectionString);
                }
                return _usersContext;
            }
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
                    _gadgetsContext = null;
                    _userGadgetsContext = null;
                    _usersContext = null;
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
