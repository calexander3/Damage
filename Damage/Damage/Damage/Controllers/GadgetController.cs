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
    }
}
