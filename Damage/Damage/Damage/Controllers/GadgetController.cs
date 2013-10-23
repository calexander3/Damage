using System.Threading.Tasks;
using Damage.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using Damage.Gadget;

namespace Damage.Controllers
{
    public class GadgetController : ApiController
    {

        /// <summary>
        /// Gets the settings for the instance of a users gadget.
        /// </summary>
        /// <param name="userGadgetId">The user gadget identifier.</param>
        /// <returns></returns>
        public string GetGadgetSettings(int userGadgetId)
        {
            using (var uow = new UnitOfWork("GlobalConfig.ConnectionString"))
            {
                var userGadget = uow.UserGadgetRepository.GetUserGadgetById(userGadgetId);

                if (userGadget.User.UserId == (int)Membership.GetUser().ProviderUserKey)
                {
                    if (!string.IsNullOrEmpty(userGadget.GadgetSettings))
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
        public void UpdateGadgetSettings(int userGadgetId, string newSettings)
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
        public void UpdateGadgetPositions(List<GadgetPosition> gadgetPositions)
        {
            var userId = (int) Membership.GetUser().ProviderUserKey;
            using (var uow = new UnitOfWork("GlobalConfig.ConnectionString"))
            {
                var userGadgets = uow.UserGadgetRepository.GetAllUserGadgetsForUser(userId);

                Parallel.ForEach(gadgetPositions, gadgetPosition =>
                {
                    var userGadgetToUpdate = userGadgets.Single(g => g.UserGadgetId == gadgetPosition.UserGadgetId);
                        userGadgetToUpdate.DisplayColumn = gadgetPosition.DisplayColumn;
                        userGadgetToUpdate.DisplayOrdinal = gadgetPosition.DisplayOrdinal;
                });
                uow.UserGadgetRepository.Save(userGadgets, DataAccess.Repositories.BaseRepository<DataAccess.Models.UserGadget>.SaveOperation.Update);
            }
        }
    }
}
