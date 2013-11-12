using System.Collections.Generic;
using System.Data.Entity;
using Damage.DataAccessEF.Models;
using System.Linq;

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