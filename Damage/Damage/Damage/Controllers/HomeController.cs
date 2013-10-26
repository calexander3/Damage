using System.Collections.Generic;
using System.Web.Mvc;
using Damage.Gadget;
using Microsoft.Practices.ServiceLocation;
using Damage.DataAccess;
using System.Threading.Tasks;
using System.Web.Security;
using Damage.Filters;

namespace Damage.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var activeGadgets = new List<IGadget>();
            if (Request.IsAuthenticated)
            {
                using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
                {
                    var gadgetsForUser = uow.UserGadgetRepository.GetAllUserGadgetsForUser((int)Membership.GetUser().ProviderUserKey);
                    Parallel.ForEach(gadgetsForUser, g =>
                                    {
                                        if (g.Gadget.AssemblyPresent && GlobalConfig.GadgetTypes.ContainsKey(g.Gadget.GadgetName))
                                        {
                                            var newGadget = ServiceLocator.Current.GetInstance(GlobalConfig.GadgetTypes[g.Gadget.GadgetName]) as IGadget;
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
                                    });
                }
            }

            return View(activeGadgets);
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult AddGadget()
        {
            IList<Damage.DataAccess.Models.Gadget> gadgets = new List<Damage.DataAccess.Models.Gadget>();
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
