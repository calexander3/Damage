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
        /// <param name="session">The session.</param>
        public UserRepository(ISession session): base(session)
        {
        }

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public User GetUserById(int userId)
        {
            return m_Session.QueryOver<User>().Where(u => u.UserId == userId).SingleOrDefault();
        }

        /// <summary>
        /// Gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public User GetUserByUsername(string username)
        {
            return m_Session.QueryOver<User>().Where(u => u.UserName == username).SingleOrDefault();
        }
    }
}
