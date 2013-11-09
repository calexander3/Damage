[assembly: WebActivator.PostApplicationStartMethod(typeof(Damage.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace Damage.App_Start
{
    using System.Reflection;
    using System.Web.Mvc;

    using SimpleInjector;
    using SimpleInjector.Integration.Web.Mvc;
    using System;
    using System.IO;
    using System.Linq;
    using Damage.Gadget;
    
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            
            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            
            container.RegisterMvcAttributeFilterProvider();
       
            container.Verify();
            
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
     
        private static void InitializeContainer(Container container)
        {
            string pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Gadgets");

            var pluginAssemblies =
                from file in new DirectoryInfo(pluginDirectory).GetFiles()
                where file.Extension == ".dll"
                select Assembly.LoadFile(file.FullName);

            var pluginTypes =
                from dll in pluginAssemblies
                from type in dll.GetExportedTypes()
                where typeof(IGadget).IsAssignableFrom(type)
                where !type.IsAbstract
                where !type.IsGenericTypeDefinition
                select type;

            container.RegisterAll<IGadget>(pluginTypes);

            // For instance:
            // container.Register<IUserRepository, SqlUserRepository>();
        }
    }
}