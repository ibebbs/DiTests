using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacTests
{
    public interface IDependency { }
    public class Dependency : IDependency { }

    public interface IDependencyFactory
    {
        IDependency Create();
    }

    public class DependencyFactory : IDependencyFactory
    {
        private readonly Func<IDependency> factory;

        public DependencyFactory(Func<IDependency> factory)
        {
            this.factory = factory;
        }

        public IDependency Create()
        {
            return this.factory();
        }
    }

    public interface IDependencyWithParameter
    {
        Guid Value { get; }
    }

    public class DependencyWithParameter : IDependencyWithParameter
    {
        public DependencyWithParameter(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }
    }

    public interface IDependencyWithParameterFactory
    {
        IDependencyWithParameter Create(Guid value);
    }

    public class DependencyWithParameterFactory : IDependencyWithParameterFactory
    {
        private readonly Func<Guid, IDependencyWithParameter> factory;

        public DependencyWithParameterFactory(Func<Guid, IDependencyWithParameter> factory)
        {
            this.factory = factory;
        }

        public IDependencyWithParameter Create(Guid value)
        {
            return this.factory(value);
        }
    }

    public interface IModuleDependency { }
    public interface IModule1Dependency { }
    public interface IModule2Dependency { }

    public interface IService { }

    public class Service1 : IService { }

    public class Service2 : IService { }

    public class Service3 : IService { }

    public interface IServiceConsumer
    {
        IReadOnlyList<IService> Services { get; }
    }

    public class ServiceConsumer : IServiceConsumer
    {
        public ServiceConsumer(IReadOnlyList<IService> services)
        {
            Services = services;
        }

        public IReadOnlyList<IService> Services { get; }
    }

    internal class Container : IServiceProvider
    {
        public static readonly Lazy<IServiceProvider> ServiceProvider = new Lazy<IServiceProvider>(() => new Container());

        private readonly IContainer container;

        public Container()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new Module1.Module());
            builder.RegisterModule(new Module2.Module());

            builder.RegisterType<Dependency>().As<IDependency>();
            builder.RegisterType<DependencyFactory>().As<IDependencyFactory>();
            builder.RegisterType<Service1>().As<IService>().SingleInstance();
            builder.RegisterType<Service2>().As<IService>().SingleInstance();
            builder.RegisterType<Service3>().As<IService>().SingleInstance();
            builder.RegisterType<ServiceConsumer>().As<IServiceConsumer>();

            this.container = builder.Build();
        }

        public object GetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }
    }
}
