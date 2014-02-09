using System;
using System.IO;
using Damage.DataAccess;
using Damage.Gadget;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;

namespace Damage
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
            GlobalConfig.Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            var container = new Container();
            SimpleInjectorInitializer.InitializeInjector(container);
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            GlobalConfig.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            GlobalConfig.GadgetTypes = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Type>();

            LoadGadgets();
        }

        private void LoadGadgets()
        {

            var gadgetInstances = ((SimpleInjectorDependencyResolver)DependencyResolver.Current).Container.GetAllInstances<IGadget>();

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
                        currentGadget.RequiresValidGoogleAccessToken = gadget.RequiresValidGoogleAccessToken;
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
                                RequiresValidGoogleAccessToken = gadget.RequiresValidGoogleAccessToken,
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