using Autofac;

namespace AutoFacTests.Module1
{
    public class Dependency : IModule1Dependency { }

    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Dependency>().As<IModule1Dependency>();
        }
    }
}
