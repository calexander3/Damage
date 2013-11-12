using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Damage.DataAccessEF.Models;

namespace Damage.DataAccessEF.Contexts
{
    public class UserGadgetsContext : DbContext
    {
        public UserGadgetsContext(string ConnectionString) : base(ConnectionString)
        {
        }

        private DbSet<UserGadget> UserGadgets { get; set; }

        /// <summary>
        /// Gets all gadgets for a user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public ICollection<UserGadget> GetAllUserGadgetsForUser(string username)
        {
            return (from ug in UserGadgets.Include(ug => ug.User).Include(ug => ug.Gadget)
                    where ug.User.UserName.ToLower() == username.ToLower()
                    select ug).ToList();
        }

        /// <summary>
        /// Gets the user gadgets for user by column.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <param name="displayColumn">The display column.</param>
        /// <returns></returns>
        public ICollection<UserGadget> GetUserGadgetsForUserByColumn(int userId, int displayColumn)
        {
            return (from ug in UserGadgets.Include(ug => ug.User).Include(ug => ug.Gadget)
                    where ug.User.UserId == userId &&
                    ug.DisplayColumn == displayColumn
                    select ug). OrderBy(ug => ug.DisplayOrdinal).ToList();
        }

        /// <summary>
        /// Gets the gadget by identifier.
        /// </summary>
        /// <param name="userGadgetId">The user gadget identifier.</param>
        /// <returns></returns>
        public UserGadget GetUserGadgetById(int userGadgetId)
        {
            return (from ug in UserGadgets.Include(ug => ug.User).Include(ug => ug.Gadget)
                    where ug.UserGadgetId == userGadgetId
                    select ug).Single();
        }

        /// <summary>
        /// Gets the next ordinal in the display column.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="displayColumnId">The display column identifier.</param>
        /// <returns></returns>
        //public int GetNextOrdinal(int userId, int displayColumnId)
        //{
        //    int? currentMaxDisplayOrdinal = m_Session.QueryOver<UserGadget>()
        //                     .Where(ug => ug.User.UserId == userId)
        //                     .And(ug => ug.DisplayColumn == displayColumnId)
        //                    .SelectList(x => x.SelectMax(y => y.DisplayOrdinal))
        //                    .Take(1)
        //                    .SingleOrDefault<int>();

        //    if (!currentMaxDisplayOrdinal.HasValue)
        //    {
        //        currentMaxDisplayOrdinal = 0;
        //    }

        //    return currentMaxDisplayOrdinal.Value + 1;
        //}

    }
}