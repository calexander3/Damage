using Damage.DataAccess;
using Damage.DataAccess.Models;
using Damage.Filters;
using Damage.Gadget;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

// ReSharper disable PossibleNullReferenceException
namespace Damage.Controllers
{
    [InitializeSimpleMembership]
    public class GadgetController : BaseController
    {
        /// <summary>
        ///     Gets the settings for the instance of a users gadget.
        /// </summary>
        /// <param name="userGadgetId">The user gadget identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetGadgetSettings(int userGadgetId)
        {
            var gadgetSettings = "";
            var settingSchema = "";
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var userGadget = uow.UserGadgetsContext.GetUserGadgetById(userGadgetId);

                    settingSchema = userGadget.Gadget.SettingsSchema;
                    if (userGadget.User.UserId == (int) Membership.GetUser().ProviderUserKey)
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
            }
            return Json(new {GadgetSettings = gadgetSettings, SettingsSchema = settingSchema},
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Updates the gadget settings.
        /// </summary>
        /// <param name="userGadgetId">The user gadget identifier.</param>
        /// <param name="newSettings">The new settings.</param>
        [HttpPost]
        public void UpdateGadgetSettings(int userGadgetId, string newSettings)
        {
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var userGadget = uow.UserGadgetsContext.GetUserGadgetById(userGadgetId);
                    if (userGadget.User.UserId == (int) Membership.GetUser().ProviderUserKey)
                    {
                        userGadget.GadgetSettings = newSettings;
                        uow.UserGadgetsContext.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        ///     Updates the gadget position.
        /// </summary>
        [HttpPost]
        public void UpdateGadgetPositions(List<GadgetPosition> gadgetPositions)
        {
            if (Request.IsAuthenticated)
            {
                if (gadgetPositions != null && gadgetPositions.Count > 0)
                {
                    using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                    {
                        var userGadgets = uow.UserGadgetsContext.GetAllUserGadgetsForUser(User.Identity.Name);

                        foreach (var gadgetPosition in gadgetPositions)
                        {
                            var userGadgetToUpdate =
                                userGadgets.Single(g => g.UserGadgetId == gadgetPosition.UserGadgetId);
                            userGadgetToUpdate.DisplayColumn = gadgetPosition.DisplayColumn;
                            userGadgetToUpdate.DisplayOrdinal = gadgetPosition.DisplayOrdinal;
                        }

                        uow.UserGadgetsContext.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        ///     Adds the new gadget.
        /// </summary>
        /// <param name="gadgetId">The gadget unique identifier.</param>
        [HttpPost]
        public JsonResult AddNewGadget(int gadgetId)
        {
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var gadget = uow.GadgetsContext.GetGadgetById(gadgetId);

                    if (gadget != null)
                    {
                        var userGadget = new UserGadget
                        {
                            User = uow.UsersContext.GetUserById((int) Membership.GetUser().ProviderUserKey),
                            Gadget = gadget,
                            GadgetSettings = gadget.DefaultSettings,
                            DisplayColumn = 1
                        };
                        userGadget.DisplayOrdinal = uow.UserGadgetsContext.GetNextOrdinal(userGadget.User.UserId,
                            userGadget.DisplayColumn);

                        uow.UserGadgetsContext.UserGadgets.Add(userGadget);

                        uow.UserGadgetsContext.SaveChanges();
                        return Json(true);
                    }
                }
            }
            return Json(false);
        }

        /// <summary>
        ///     Deletes the gadget.
        /// </summary>
        /// <param name="userGadgetId">The user gadget identifier.</param>
        [HttpPost]
        public void DeleteGadget(int userGadgetId)
        {
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var userGadget = uow.UserGadgetsContext.GetUserGadgetById(userGadgetId);
                    if (userGadget.User.UserId == (int) Membership.GetUser().ProviderUserKey)
                    {
                        uow.UserGadgetsContext.UserGadgets.Remove(userGadget);
                        uow.UserGadgetsContext.SaveChanges();
                    }
                }
            }
        }
    }
}
// ReSharper restore PossibleNullReferenceException