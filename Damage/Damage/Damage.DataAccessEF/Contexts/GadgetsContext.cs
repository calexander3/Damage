using System.Data.Entity;
using Damage.DataAccessEF.Models;

namespace Damage.DataAccessEF.Contexts
{
    public class GadgetsContext: DbContext
    {
        public GadgetsContext(string ConnectionString) : base(ConnectionString)
        {
        }

        public DbSet<Gadget> Gadgets { get; set; }

    }
}