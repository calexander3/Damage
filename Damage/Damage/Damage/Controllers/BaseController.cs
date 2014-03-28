using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Damage.Controllers
{
    public abstract class BaseController : Controller

    {
        //Handle pages that are not found
        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        protected override void HandleUnknownAction(string actionName)
        {
            View("NotFound").ExecuteResult(ControllerContext);
        }

        //Handle application exceptions
        protected override void OnException(ExceptionContext filterContext)
        {
            //Send error to the user interface.
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = View("Error", filterContext);
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(500, filterContext.Exception.Message);
            }
            GlobalConfig.Log.Error("Controller Error", filterContext.Exception);
            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }

        /// <summary>
        ///     Executes the action but limits the execution to the given timespan.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="codeBlock">The code block.</param>
        /// <returns>True if the action was able to be executed within the timespan, otherwise false</returns>
        protected static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            try
            {
                var task = Task.Factory.StartNew(() => codeBlock());
                task.Wait(timeSpan);
                return task.IsCompleted;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerExceptions[0];
            }
        }
    }
}