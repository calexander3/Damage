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
    public class UserGadgetRepository:BaseRepository<UserGadget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserGadgetRepository"/> class.
        /// </summary>
        /// <param name="Session">The session.</param>
        public UserGadgetRepository(ISession Session) : base(Session)
        {
        }

        /// <summary>
        /// Gets all gadgets for a user.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        public IList<UserGadget> GetAllUserGadgetsForUser(int UserId)
        {
            return m_Session.QueryOver<UserGadget>()
                .Where(ug => ug.User.UserId == UserId)
                .Fetch(ug => ug.User).Eager
                .Fetch(ug => ug.Gadget).Eager
                .List<UserGadget>();
        }
    }
}
