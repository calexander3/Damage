using System;
using Damage.DataAccess.Models;
using Damage.DataAccess.Repositories;

namespace Damage.DataAccess
{
    public class UnitOfWork : IDisposable
    {
        private readonly Entities _context;
        public UnitOfWork(string connectionString)
        {
            _context = new Entities(connectionString);
        }

        private UserGadgetRepository _userGadgetsRepository;
        public UserGadgetRepository UserGadgetRepository
        {
            get { return _userGadgetsRepository ?? (_userGadgetsRepository = new UserGadgetRepository(_context)); }
        }

        private GadgetRepository _gadgetsRepository;
        public GadgetRepository GadgetRepository
        {
            get { return _gadgetsRepository ?? (_gadgetsRepository = new GadgetRepository(_context)); }
        }

        private UserRepository _usersRepository;
        public UserRepository UserRepository
        {
            get { return _usersRepository ?? (_usersRepository = new UserRepository(_context)); }
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
                    _userGadgetsRepository = null;
                    _usersRepository = null;
                    _gadgetsRepository = null;
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
