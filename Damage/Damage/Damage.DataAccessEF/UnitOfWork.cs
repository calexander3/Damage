using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Damage.DataAccessEF
{
    public class UnitOfWork : IDisposable
    {



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
                    //m_UserGadgetRepository = null;
                    //m_GadgetRepository = null;
                    //m_Session.Dispose();
                    //m_SessionFactory.Dispose();
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
