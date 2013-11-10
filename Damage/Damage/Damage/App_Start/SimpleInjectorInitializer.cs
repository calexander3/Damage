using Damage;
using Damage.Gadget;
using SimpleInjector;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

public static class SimpleInjectorInitializer
{
    /// <summary>Initialize the container and register it as MVC Dependency Resolver.</summary>
    public static void InitializeInjector()
    {
        var container = new Container();

        InitializeContainer(container);

        //container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

        //container.RegisterMvcAttributeFilterProvider();

        container.Verify();

        //DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        GlobalConfig.DependencyResolver = container;
    }

    private static void InitializeContainer(Container container)
    {
        string pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Bin\Gadgets");

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