using Damage.DataAccessEF.Models;
using System.Data.Entity;

namespace Damage.DataAccessEF.Contexts
{
    public class GadgetsContext: DbContext
    {
        public GadgetsContext(string ConnectionString) : base(ConnectionString)
        {
        }

        private DbSet<Gadget> Gadgets { get; set; }
    }
}