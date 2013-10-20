using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Damage.DataAccess.Models;

namespace Damage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var t = new List<UserGadget>();
            t.Add(
                    new UserGadget()
                    {
                        Gadget = new Gadget() { GadgetName = "teest" },
                        Ordinal = 0,
                        Column = 1
                    }
                );
            t.Add(
        new UserGadget()
        {
            Gadget = new Gadget() { GadgetName = "teest2" },
            Ordinal = 1,
            Column = 1
        }
    );
            t.Add(
        new UserGadget()
        {
            Gadget = new Gadget() { GadgetName = "teest3" },
            Ordinal = 0,
            Column = 2
        }
    );

            return View(t);
        }
    }
}
