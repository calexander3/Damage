using System.Data.Entity;
using Damage.DataAccessEF.Models;

namespace Damage.DataAccessEF.Contexts
{
    public class GadgetsContext: DbContext
    {
        public GadgetsContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Gadget> Gadgets { get; set; }

    }
}