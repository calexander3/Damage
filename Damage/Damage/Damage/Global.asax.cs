using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Damage.Gadget;
using Microsoft.Practices.ServiceLocation;
using Damage.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System;

namespace Damage
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.IsSecureConnection.Equals(false) && HttpContext.Current.Request.IsLocal.Equals(false))
            {
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl);
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            GlobalConfig.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            GlobalConfig.GadgetTypes = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Type>();

            LoadGadgets();
        }

        private void LoadGadgets()
        {
            var gadgetInstances = ServiceLocator.Current.GetAllInstances<IGadget>();

            using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
            {
                var currentGadgets = uow.GadgetRepository.GetAllGadgets();
                var newGadgets = new List<Damage.DataAccess.Models.Gadget>();

                foreach(var currentGadget in currentGadgets) {currentGadget.AssemblyPresent = false;};

                foreach (IGadget gadget in gadgetInstances)
                {
                    var currentGadget = currentGadgets.FirstOrDefault(g => g.GadgetName == gadget.GetType().Name);
                    if (currentGadget != null)
                    {
                        currentGadget.AssemblyPresent = true;
                        currentGadget.GadgetTitle = gadget.Title;
                        currentGadget.GadgetDescription = gadget.Description;
                        currentGadget.InBeta = gadget.InBeta;
                        currentGadget.GadgetVersion = gadget.GetType().Assembly.GetName().Version.ToString();
                        currentGadget.DefaultSettings = gadget.DefaultSettings;
                        currentGadget.SettingsSchema = JsonConvert.SerializeObject(gadget.SettingsSchema);
                    }
                    else
                    {
                        newGadgets.Add(
                            new DataAccess.Models.Gadget()
                            {
                                GadgetName = gadget.GetType().Name,
                                AssemblyPresent = true,
                                GadgetTitle = gadget.Title,
                                GadgetDescription = gadget.Description,
                                InBeta = gadget.InBeta,
                                GadgetVersion = gadget.GetType().Assembly.GetName().Version.ToString(),
                                DefaultSettings = gadget.DefaultSettings,
                                SettingsSchema = JsonConvert.SerializeObject(gadget.SettingsSchema)
                            }
                        );
                    }

                    GlobalConfig.GadgetTypes.TryAdd(gadget.GetType().Name, gadget.GetType());
                }

                uow.GadgetRepository.Save(currentGadgets, DataAccess.Repositories.BaseRepository<DataAccess.Models.Gadget>.SaveOperation.Update);
                if (newGadgets.Count > 0)
                {
                    uow.GadgetRepository.Save(newGadgets, DataAccess.Repositories.BaseRepository<DataAccess.Models.Gadget>.SaveOperation.SaveNew);
                }
            }

        }
    }
}