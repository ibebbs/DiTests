using Pure.DI;

namespace PureDiTests.Module1
{
    public class Dependency : IModule1Dependency { }

    public static partial class Module
    {
        public const string ModuleName = "Module1";

        static Module() => DI.Setup(ModuleName)
            .Bind<IModule1Dependency>().To<Dependency>();

    }
}
