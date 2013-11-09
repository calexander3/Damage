using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Damage.DataAccess.Models;
using Damage.Gadget;
using Damage.DataAccess;
using System.Web.Security;
using Damage.Filters;
using System.Linq;
using Microsoft.Web.WebPages.OAuth;

namespace Damage.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var hasLinkedGoogleAccount = false;
            var activeGadgets = new List<IGadget>();

            using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
            {
                IList<UserGadget> gadgetsForUser = null;

                if (Request.IsAuthenticated)
                {
                    gadgetsForUser = uow.UserGadgetRepository.GetAllUserGadgetsForUser(User.Identity.Name);

                    //refresh oauth access token if needed
                    if (gadgetsForUser.Count > 0)
                    {
                        if (OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 0)
                        {
                            hasLinkedGoogleAccount = true;
                            if (DateTime.Compare(DateTime.Now, gadgetsForUser.First().User.OAuthAccessTokenExpiration) > 0 &&
                                gadgetsForUser.Any(g => g.Gadget.RequiresValidGoogleAccessToken))
                            {
                                return new Damage.Controllers.AccountController.ExternalLoginResult("google", Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = "" }));
                            }
                        }
                    }

                }
                else
                {
                    gadgetsForUser = uow.GadgetRepository.GetDefaultGadgets();
                }

                foreach (var g in gadgetsForUser)
                {
                    if (g.Gadget.RequiresValidGoogleAccessToken && !hasLinkedGoogleAccount)
                    {
                        activeGadgets.Add(new RequiresOAuthGadget()
                        {
                            UserGadget = g
                        });
                    }
                    else if (g.Gadget.AssemblyPresent && GlobalConfig.GadgetTypes.ContainsKey(g.Gadget.GadgetName))
                    {
                        var newGadget = DependencyResolver.Current.GetService(GlobalConfig.GadgetTypes[g.Gadget.GadgetName]) as IGadget;
                        newGadget.UserGadget = g;
                        activeGadgets.Add(newGadget);
                    }
                    else
                    {
                        activeGadgets.Add(new NotAvailableGadget()
                        {
                            UserGadget = g
                        });
                    }
                }
            }

            return View(activeGadgets);
        }

        public string healthCheck()
        {
            return "ok";
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult AddGadget()
        {
            IList<DataAccess.Models.Gadget> gadgets = new List<Damage.DataAccess.Models.Gadget>();
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    gadgets = uow.GadgetRepository.GetAllAvailableGadgets();
                }
            }
            return View(gadgets);
        }
    }
}
