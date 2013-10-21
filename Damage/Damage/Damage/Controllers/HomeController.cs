using Damage.DataAccess.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Damage.DataAccess;
using System.Threading;
using System.Threading.Tasks;

namespace Damage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var activeGadgets = new List<IGadget>();

            using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
            {
                var gadgetsForUser = uow.UserGadgetRepository.GetAllUserGadgetsForUser(1);
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
                                        activeGadgets.Add(new NotAvailableGadget() {
                                            UserGadget = g
                                        });
                                    }
                                });
            }


            return View(activeGadgets);
        }
    }
}
