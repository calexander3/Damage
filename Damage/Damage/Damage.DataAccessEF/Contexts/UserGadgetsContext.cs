using System.Data.Entity;
using Damage.DataAccessEF.Models;

namespace Damage.DataAccessEF.Contexts
{
    public class UserGadgetsContext : DbContext
    {
        public UserGadgetsContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserGadget> UserGadgets { get; set; }

    }
}