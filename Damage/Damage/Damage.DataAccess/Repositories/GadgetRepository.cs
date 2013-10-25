using System.Collections.Generic;
using Damage.DataAccess.Models;
using NHibernate;

namespace Damage.DataAccess.Repositories
{
    public class GadgetRepository : BaseRepository<Gadget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GadgetRepository"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public GadgetRepository(ISession session): base(session)
        {
        }

        /// <summary>
        /// Gets all gadgets registered in database.
        /// </summary>
        /// <returns></returns>
        public IList<Gadget> GetAllGadgets()
        {
            return m_Session.QueryOver<Gadget>().List<Gadget>();
        }

        /// <summary>
        /// Gets all gadgets registered in database that have a corresponding assembly loaded.
        /// </summary>
        /// <returns></returns>
        public IList<Gadget> GetAllAvailableGadgets()
        {
            return m_Session.QueryOver<Gadget>().Where(g => g.AssemblyPresent).List<Gadget>();
        }
    }
}
