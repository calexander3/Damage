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
    class UserGadgetRepository:BaseRepository<UserGadget>
    {
        public UserGadgetRepository(ISession Session) : base(Session)
        {
        }

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
