using Damage.DataAccess.Models;
using System.Data.Entity;
using System.Linq;

namespace Damage.DataAccess.Repositories
{
    public class UserRepository : BaseRepository
    {
        public DbSet<User> Users { get; set; }
        public UserRepository(Entities context): base(context)
        {
            Users = Context.Users;
        }

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public User GetUserById(int userId)
        {
            return Users.SingleOrDefault(u => u.UserId == userId);
        }

        /// <summary>
        /// Gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public User GetUserByUsername(string username)
        {
            return Users.SingleOrDefault(u => u.UserName.ToLower() == username.ToLower());
        }

        /// <summary>
        /// Gets the user by emailAddress.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        public User GetUserByEmailAddress(string emailAddress)
        {
            return Users.SingleOrDefault(u => u.EmailAddress.ToLower() == emailAddress.ToLower());
        }
    }
}