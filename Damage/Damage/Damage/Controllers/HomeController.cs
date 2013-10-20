using Damage.DataAccess.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;

namespace Damage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var t = new List<IGadget>();


            t.Add(new NotAvailableGadget()
            {

                UserGadget = new UserGadget()
                {
                    Gadget = new Damage.DataAccess.Models.Gadget() { GadgetName = "teest" },
                    Ordinal = 0,
                    Column = 1
                }
            }

                );



            t.Add(new NotAvailableGadget()
            {

                UserGadget = new UserGadget()
                {
                    Gadget = new Damage.DataAccess.Models.Gadget() { GadgetName = "teest2" },
                    Ordinal = 1,
                    Column = 1
                }
            }

                ); t.Add(new NotAvailableGadget()
                {

                    UserGadget = new UserGadget()
                    {
                        Gadget = new Damage.DataAccess.Models.Gadget() { GadgetName = "teest3" },
                        Ordinal = 0,
                        Column = 2
                    }
                }
);

            return View(t);
        }
    }
}
