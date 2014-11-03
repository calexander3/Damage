using System.Security.Cryptography;
using Damage.DataAccess;
using Damage.Gadget;
using Damage.Utilities;
using Newtonsoft.Json;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.IO;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Damage
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
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
            GlobalConfig.GadgetTypes = new System.Collections.Concurrent.ConcurrentDictionary<string, Type>();

            LoadGadgets();

            var aesCsp = new AesCryptoServiceProvider();
            aesCsp.GenerateKey();
            aesCsp.GenerateIV();
            GlobalConfig.Encryptor = new SimpleAES(aesCsp.Key, aesCsp.IV);
        }

        private void LoadGadgets()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var gadgetInstances =((SimpleInjectorDependencyResolver)DependencyResolver.Current).Container.GetAllInstances<IGadget>();

            using (var uow = new UnitOfWork(GlobalConfig.ConnectionString))
            {
                foreach (var currentGadget in uow.GadgetRepository.Gadgets)
                {
                    currentGadget.AssemblyPresent = false;
                }

                foreach (var gadget in gadgetInstances)
                {
                    var currentGadget = uow.GadgetRepository.GetGadgetByName(gadget.GetType().Name);
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
                        uow.GadgetRepository.Gadgets.Add(
                            new DataAccess.Models.Gadget
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
                            });
                    }

                    GlobalConfig.GadgetTypes.TryAdd(gadget.GetType().Name, gadget.GetType());
                }

                uow.UserGadgetRepository.SaveChanges();

            }
        }
    }
}