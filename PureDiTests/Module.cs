using Pure.DI;

namespace PureDiTests
{
    public class ModuleDependency : IModuleDependency { }

    public static partial class Module
    {
        static Module() => DI.Setup()
            .Bind<IModuleDependency>().To<ModuleDependency>();

    }
}
