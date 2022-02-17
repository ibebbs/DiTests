using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediTests
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

    internal class Container : IServiceProvider
    {
        public static readonly Lazy<IServiceProvider> ServiceProvider = new Lazy<IServiceProvider>(() => new Container() as IServiceProvider);

        private readonly ServiceProvider container;

        public Container()
        {
            var builder = new ServiceCollection();

            builder.AddModule(new Module1.Module());
            builder.AddModule(new Module2.Module());

            builder.AddTransient<IDependency, Dependency>();
            builder.AddTransient<IDependencyFactory, DependencyFactory>();
            builder.AddSingleton<IService, Service1>();
            builder.AddSingleton<IService, Service2>();
            builder.AddSingleton<IService, Service3>();
            builder.AddTransient<IServiceConsumer, ServiceConsumer>();

            this.container = builder.BuildServiceProvider();
        }

        public object GetService(Type serviceType)
        {
            return container.GetService(serviceType);
        }
    }

    public interface IModule
    {
        IServiceCollection ConfigureServices(IServiceCollection serviceCollection);
    }

    public static class ContainerExtensions
    {
        public static IServiceCollection AddModule(this IServiceCollection serviceCollection, IModule module)
        {
            return module.ConfigureServices(serviceCollection);
        }
    }
}
