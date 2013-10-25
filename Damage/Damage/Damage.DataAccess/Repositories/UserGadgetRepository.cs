﻿using System.Collections.Generic;
using System.Linq;
using Damage.DataAccess.Models;
using NHibernate;

namespace Damage.DataAccess.Repositories
{
    public class UserGadgetRepository:BaseRepository<UserGadget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserGadgetRepository"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public UserGadgetRepository(ISession session): base(session)
        {
        }

        /// <summary>
        /// Gets all gadgets for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public IList<UserGadget> GetAllUserGadgetsForUser(int userId)
        {
            return m_Session.QueryOver<UserGadget>()
                .Where(ug => ug.User.UserId == userId)
                .Fetch(ug => ug.User).Eager
                .Fetch(ug => ug.Gadget).Eager
                .List<UserGadget>();
        }


        /// <summary>
        /// Gets the user gadgets for user by column.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <param name="displayColumn">The display column.</param>
        /// <returns></returns>
        public IList<UserGadget> GetUserGadgetsForUserByColumn(int userId, int displayColumn)
        {
            return m_Session.QueryOver<UserGadget>()
                .Where(ug => ug.User.UserId == userId)
                .And(ug => ug.DisplayColumn == displayColumn)
                .Fetch(ug => ug.User).Eager
                .Fetch(ug => ug.Gadget).Eager
                .OrderBy(ug => ug.DisplayOrdinal).Asc
                .List<UserGadget>();
        }

        /// <summary>
        /// Gets the gadget by identifier.
        /// </summary>
        /// <param name="userGadgetId">The user gadget identifier.</param>
        /// <returns></returns>
        public UserGadget GetUserGadgetById(int userGadgetId)
        {
            return m_Session.QueryOver<UserGadget>()
                .Where(ug => ug.UserGadgetId == userGadgetId)
                .Fetch(ug => ug.User).Eager
                .Fetch(ug => ug.Gadget).Eager
                .List<UserGadget>().Single();

        }
    }
}
