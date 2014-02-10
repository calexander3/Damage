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
    }
}