using Damage.DataAccess.Contexts;
using System;
using Damage.DataAccess.Models;

namespace Damage.DataAccess
{
    public class UnitOfWork : IDisposable
    {
        private readonly string _connectionString = "DefaultConnection";
        private readonly Entities _context;
        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
            _context = new Entities(_connectionString);
        }

        private UserGadgetsContext _userGadgetsContext;
        public UserGadgetsContext UserGadgetsContext
        {
            get { return _userGadgetsContext ?? (_userGadgetsContext = new UserGadgetsContext(_context)); }
        }

        private GadgetsContext _gadgetsContext;
        public GadgetsContext GadgetsContext
        {
            get { return _gadgetsContext ?? (_gadgetsContext = new GadgetsContext(_context)); }
        }

        private UsersContext _usersContext;
        public UsersContext UsersContext
        {
            get { return _usersContext ?? (_usersContext = new UsersContext(_context)); }
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
                    _userGadgetsContext = null;
                    _usersContext = null;
                    _gadgetsContext = null;
                    if (_context != null)
                    {
                        _context.Dispose(); 
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
