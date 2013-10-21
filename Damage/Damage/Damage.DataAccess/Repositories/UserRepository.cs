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
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="Session">The session.</param>
        public UserRepository(ISession Session): base(Session)
        {
        }
    }
}
