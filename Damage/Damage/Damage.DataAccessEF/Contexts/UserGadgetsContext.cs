using System.Data.Entity;
using Damage.DataAccessEF.Models;

namespace Damage.DataAccessEF.Contexts
{
    public class UserGadgetsContext : DbContext
    {
        public UserGadgetsContext(string ConnectionString) : base(ConnectionString)
        {
        }

        public DbSet<UserGadget> UserGadgets { get; set; }

    }
}