using System.Data.Entity;
using Damage.DataAccessEF.Models;

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