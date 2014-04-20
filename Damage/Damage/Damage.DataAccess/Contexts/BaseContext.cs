using System;
using Damage.DataAccess.Models;

namespace Damage.DataAccess.Contexts
{
    public abstract class BaseContext
    {
        internal readonly Entities Context;

        internal BaseContext(Entities context)
        {
            Context = context;
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public void SaveChangesAsync()
        {
            Context.SaveChangesAsync();
        }
    }
}