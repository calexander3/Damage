using System.Linq;
using Damage.DataAccessEF.Models;
using System.Data.Entity;

namespace Damage.DataAccessEF.Contexts
{
    public class UsersContext : DbContext
    {
        public UsersContext(string ConnectionString) : base(ConnectionString)
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
            return UserProfiles.SingleOrDefault(u => u.UserName == username);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>().HasMany(u => u.UserGadgets).WithOptional().HasForeignKey(ug => ug.UserId);
            base.OnModelCreating(modelBuilder);
        }
    }
}