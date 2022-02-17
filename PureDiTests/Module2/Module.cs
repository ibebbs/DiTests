using Pure.DI;

namespace PureDiTests.Module2
{
    public class Dependency : IModule2Dependency { }

    public static partial class Module
    {
        public const string ModuleName = "Module2";

        static Module() => DI.Setup(ModuleName)
            .Bind<IModule2Dependency>().To<Dependency>();

    }
}
