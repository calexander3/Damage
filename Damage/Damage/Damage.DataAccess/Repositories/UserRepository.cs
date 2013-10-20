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
    public class UserRepository:BaseRepository<User>
    {
        public UserRepository(ISession Session): base(Session)
        {
        }
    }
}
