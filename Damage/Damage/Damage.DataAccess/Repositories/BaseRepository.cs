using Damage.DataAccess.Models;

namespace Damage.DataAccess.Repositories
{
    public abstract class BaseRepository
    {
        internal readonly Entities Context;

        internal BaseRepository(Entities context)
        {
            Context = context;
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}