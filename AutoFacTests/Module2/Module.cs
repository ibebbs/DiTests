using Autofac;

namespace AutoFacTests.Module2
{
    public class Dependency : IModule2Dependency { }

    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Dependency>().As<IModule2Dependency>();
        }
    }
}
