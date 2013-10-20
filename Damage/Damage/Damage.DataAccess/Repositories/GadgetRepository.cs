using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damage.DataAccess.Models;
using NHibernate;
using NHibernate.Criterion;

namespace Damage.DataAccess.Repositories
{
    class GadgetRepository : BaseRepository<Gadget>
    {
        public GadgetRepository(ISession Session): base(Session)
        {
        }

        public IList<Gadget> GetAllGadgets()
        {
            return m_Session.QueryOver<Gadget>().List<Gadget>();
        }

        //public IList<Gadget> GetAllAvailableGadgets()
        //{
        //    return m_Session.QueryOver<Gadget>().Where(g => g.).List<Gadget>();
        //}
    }
}
