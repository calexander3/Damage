using Damage.DataAccess.Models;
using System.Data.Entity;
using System.Linq;

namespace Damage.DataAccess.Contexts
{
    public class UsersContext : DbContext
    {
        public UsersContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }


        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public UserProfile GetUserById(int userId)
        {
            return UserProfiles.SingleOrDefault(u => u.UserId == userId);
        }

        /// <summary>
        /// Gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public UserProfile GetUserByUsername(string username)
        {
            return UserProfiles.SingleOrDefault(u => u.UserName.ToLower() == username.ToLower());
        }

        /// <summary>
        /// Gets the user by emailAddress.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        public UserProfile GetUserByEmailAddress(string emailAddress)
        {
            return UserProfiles.SingleOrDefault(u => u.EmailAddress.ToLower() == emailAddress.ToLower());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>().HasMany(u => u.UserGadgets).WithOptional().HasForeignKey(ug => ug.UserId);
            base.OnModelCreating(modelBuilder);
        }
    }
}