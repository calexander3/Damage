using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Damage.Controllers
{
    public abstract class BaseController : Controller

    {   //Handle pages that are not found
        public ActionResult NotFound()
        {
            return View("NotFound");
        }
        protected override void HandleUnknownAction(string actionName)
        {
            View("NotFound").ExecuteResult(this.ControllerContext);
        }

        //Handle application exceptions
        protected override void OnException(ExceptionContext filterContext)
        {
            //Send error to the user interface.
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = View("Error", filterContext.Exception);
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(500, filterContext.Exception.Message);
            }
            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }
    }
}
