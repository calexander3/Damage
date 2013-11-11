using System.Data.Entity;
using Damage.DataAccessEF.Models;

namespace Damage.DataAccessEF.Contexts
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}