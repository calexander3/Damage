using Damage.DataAccess;
using Damage.DataAccess.Models;
using Damage.Filters;
using Damage.Gadget;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Damage.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var hasLinkedGoogleAccount = false;
            var activeGadgets = new List<IGadget>();
            var layout = 0;

            using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
            {
                IList<UserGadget> gadgetsForUser = null;

                if (Request.IsAuthenticated)
                {
                    gadgetsForUser = uow.UserGadgetRepository.GetAllUserGadgetsForUser(User.Identity.Name);

                    //refresh oauth access token if needed
                    if (gadgetsForUser.Count > 0)
                    {
                        layout = gadgetsForUser.First().User.LayoutId;
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

                switch (layout)
                {
                    case 0:
                        ViewBag.ColumnCount = 3;
                        ViewBag.ColumnCss = "Layout0.css";
                        break;
                    case 1:
                        ViewBag.ColumnCount = 3;
                        ViewBag.ColumnCss = "Layout1.css";
                        break;
                    case 2:
                        ViewBag.ColumnCount = 2;
                        ViewBag.ColumnCss = "Layout2.css";
                        break;

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
