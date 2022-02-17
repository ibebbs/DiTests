using Jab;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JabTests
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
        public ServiceConsumer(IEnumerable<IService> services)
        {
            Services = (services ?? Array.Empty<IService>()).ToArray();
        }

        public IReadOnlyList<IService> Services { get; }
    }

    [ServiceProvider]
    [Import(typeof(Module1.IModule))]
    [Import(typeof(Module2.IModule))]
    [Transient(typeof(IDependency), typeof(Dependency))]
    [Transient(typeof(Func<IDependency>), Factory = nameof(Container.DependencyFactory))]
    [Transient(typeof(IDependencyFactory), typeof(DependencyFactory))]
    [Singleton(typeof(IService), typeof(Service1))]
    [Singleton(typeof(IService), typeof(Service2))]
    [Singleton(typeof(IService), typeof(Service3))]
    [Transient(typeof(IServiceConsumer), typeof(ServiceConsumer))]
    public partial class Container
    {
        public static readonly IServiceProvider ServiceProvider = new Container() as IServiceProvider;

        public Func<IDependency> DependencyFactory() => () => ServiceProvider.Get<IDependency>();
    }
}
