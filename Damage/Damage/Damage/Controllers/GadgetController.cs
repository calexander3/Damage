using System.Threading.Tasks;
using Damage.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Damage.Gadget;
using Damage.Filters;

namespace Damage.Controllers
{
    [InitializeSimpleMembership]
    public class GadgetController : Controller
    {

        /// <summary>
        /// Gets the settings for the instance of a users gadget.
        /// </summary>
        /// <param name="userGadgetId">The user gadget identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetGadgetSettings(int userGadgetId)
        {
            var gadgetSettings = "";
            var settingSchema = "";

            using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
            {
                var userGadget = uow.UserGadgetRepository.GetUserGadgetById(userGadgetId);

                settingSchema = userGadget.Gadget.SettingsSchema;
                if (userGadget.User.UserId == (int)Membership.GetUser().ProviderUserKey)
                {
                    if (!string.IsNullOrEmpty(userGadget.GadgetSettings))
                    {
                        gadgetSettings = userGadget.GadgetSettings;
                    }
                    else
                    {
                        gadgetSettings = userGadget.Gadget.DefaultSettings;
                    }
                }
            }
            return Json(new {GadgetSettings = gadgetSettings, SettingsSchema = settingSchema}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates the gadget settings.
        /// </summary>
        /// <param name="userGadgetId">The user gadget identifier.</param>
        /// <param name="newSettings">The new settings.</param>
        [HttpPost]
        public void UpdateGadgetSettings(int userGadgetId, string newSettings)
        {
            using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
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
        [HttpPost]
        public void UpdateGadgetPositions(List<GadgetPosition> gadgetPositions)
        {
            if (gadgetPositions != null && gadgetPositions.Count > 0)
            {
                var userId = (int)Membership.GetUser().ProviderUserKey;
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
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

        /// <summary>
        /// Adds the new gadget.
        /// </summary>
        /// <param name="gadgetId">The gadget unique identifier.</param>
        [HttpPost]
        public void AddNewGadget(int gadgetId)
        {
            
        }
    }
}
