using Pure.DI;
using System;
using System.Collections.Generic;

namespace PureDiTests
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

    public static partial class Container
    {
        private static void Setup() => DI.Setup()
            .DependsOn(nameof(Module))
            .DependsOn(Module1.Module.ModuleName)
            .DependsOn(Module2.Module.ModuleName)
            .Bind<IDependency>().To<Dependency>()
            .Bind<IDependencyFactory>().To<DependencyFactory>()
            .Bind<IService>(nameof(Service1)).As(Lifetime.Singleton).To<Service1>()
            .Bind<IService>(nameof(Service2)).As(Lifetime.Singleton).To<Service2>()
            .Bind<IService>(nameof(Service3)).As(Lifetime.Singleton).To<Service3>()
            .Bind<IServiceConsumer>().To<ServiceConsumer>();

        public static IServiceProvider ServiceProvider => Resolve<IServiceProvider>();
    }
}
