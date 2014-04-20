using Damage.DataAccessEF.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Damage.DataAccessEF.Contexts
{
    public class UserGadgetsContext : DbContext
    {
        public UserGadgetsContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<UserGadget> UserGadgets { get; set; }

        /// <summary>
        /// Gets all gadgets for a user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public IList<UserGadget> GetAllUserGadgetsForUser(string username)
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
        public IList<UserGadget> GetUserGadgetsForUserByColumn(int userId, int displayColumn)
        {
            return (from ug in UserGadgets.Include(ug => ug.User).Include(ug => ug.Gadget)
                    where ug.User.UserId == userId &&
                    ug.DisplayColumn == displayColumn
                    select ug).OrderBy(ug => ug.DisplayOrdinal).ToList();
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
        public int GetNextOrdinal(int userId, int displayColumnId)
        {
            var bottomUserGadget =
                UserGadgets.Where(ug => ug.User.UserId == userId && ug.DisplayColumn == displayColumnId)
                    .OrderByDescending(ug => ug.DisplayOrdinal)
                    .FirstOrDefault();
            if (bottomUserGadget != null)
            {
                return bottomUserGadget.DisplayOrdinal + 1;
            }
            return 1;
        }
    }
}