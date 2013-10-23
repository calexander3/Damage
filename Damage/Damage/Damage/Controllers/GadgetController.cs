using Damage.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;

namespace Damage.Controllers
{
    public class GadgetController : ApiController
    {

        /// <summary>
        /// Gets the settings for the instance of a users gadget.
        /// </summary>
        /// <param name="userGadgetId">The user gadget identifier.</param>
        /// <returns></returns>
        public string getGadgetSettings(int userGadgetId)
        {
            using (var uow = new UnitOfWork("GlobalConfig.ConnectionString"))
            {
                var userGadget = uow.UserGadgetRepository.GetUserGadgetById(userGadgetId);

                if (userGadget.User.UserId == (int)Membership.GetUser().ProviderUserKey)
                {
                    if (userGadget.GadgetSettings != null && userGadget.GadgetSettings.Length > 0)
                    {
                        return userGadget.GadgetSettings;
                    }
                    else
                    {
                        return userGadget.Gadget.DefaultSettings;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Updates the gadget settings.
        /// </summary>
        /// <param name="userGadgetId">The user gadget identifier.</param>
        /// <param name="newSettings">The new settings.</param>
        public void updateGadgetSettings(int userGadgetId, string newSettings)
        {
            using (var uow = new UnitOfWork("GlobalConfig.ConnectionString"))
            {
                var userGadget = uow.UserGadgetRepository.GetUserGadgetById(userGadgetId);
                if (userGadget.User.UserId == (int)Membership.GetUser().ProviderUserKey)
                {
                    userGadget.GadgetSettings = newSettings;
                    uow.UserGadgetRepository.Save(userGadget, DataAccess.Repositories.BaseRepository<DataAccess.Models.UserGadget>.SaveOperation.Update);
                }
            }
        }

        /// <summary>
        /// Updates the gadget position.
        /// </summary>
        /// <param name="userGadgetId">The user gadget identifier.</param>
        /// <param name="displayColumn">The display column.</param>
        /// <param name="displayOrdinal">The display ordinal.</param>
        public void updateGadgetPosition(int userGadgetId, int displayColumn, int displayOrdinal)
        {
            using (var uow = new UnitOfWork("GlobalConfig.ConnectionString"))
            {
                var userGadgets = uow.UserGadgetRepository.GetAllUserGadgetsForUser(userGadgetId);
                var userGadgetToUpdate = userGadgets.Single(g => g.UserGadgetId == userGadgetId);
                if (userGadgetToUpdate.User.UserId == (int)Membership.GetUser().ProviderUserKey)
                {


                    //TODO: Change all other gadgets ordinal in affected columns


                    userGadgetToUpdate.DisplayColumn = displayColumn;
                    userGadgetToUpdate.DisplayOrdinal = displayOrdinal;



                    uow.UserGadgetRepository.Save(userGadgets, DataAccess.Repositories.BaseRepository<DataAccess.Models.UserGadget>.SaveOperation.Update);
                }
            }
        }
    }
}
