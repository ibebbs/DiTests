using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using NUnit.Framework;
using System.Linq;

namespace JabTests
{
    public class Tests
    {
        [Test]
        public void GetBasicDependency()
        {
            var dependency = Container.ServiceProvider.Get<IDependency>();

            Assert.That(dependency, Is.Not.Null);
            Assert.That(dependency, Is.InstanceOf<Dependency>());
        }

        [Test]
        public void GetDependencyFactory()
        {
            var dependencyFactory = Container.ServiceProvider.Get<IDependencyFactory>();

            var dependency = dependencyFactory.Create();

            Assert.That(dependency, Is.Not.Null);
            Assert.That(dependency, Is.InstanceOf<Dependency>());
        }

        [Test]
        public void GetNestedModuleDependency()
        {
            var dependency1 = Container.ServiceProvider.Get<IModule1Dependency>();
            var dependency2 = Container.ServiceProvider.Get<IModule2Dependency>();

            Assert.That(dependency1, Is.Not.Null);
            Assert.That(dependency1, Is.InstanceOf<Module1.Dependency>());

            Assert.That(dependency2, Is.Not.Null);
            Assert.That(dependency2, Is.InstanceOf<Module2.Dependency>());
        }

        [Test]
        public void GetServiceConsumer()
        {
            var serviceConsumer1 = Container.ServiceProvider.Get<IServiceConsumer>();

            Assert.AreEqual(3, serviceConsumer1.Services.Count);
            Assert.AreEqual(1, serviceConsumer1.Services.OfType<Service1>().Count());
            Assert.AreEqual(1, serviceConsumer1.Services.OfType<Service2>().Count());
            Assert.AreEqual(1, serviceConsumer1.Services.OfType<Service3>().Count());

            var serviceConsumer2 = Container.ServiceProvider.Get<IServiceConsumer>();

            Assert.AreNotEqual(serviceConsumer1, serviceConsumer2);
            Assert.AreEqual(3, serviceConsumer2.Services.Count);
            Assert.IsTrue(Enumerable
                .Zip(
                    serviceConsumer1.Services.OfType<Service1>(),
                    serviceConsumer2.Services.OfType<Service1>())
                .All(tuple => object.Equals(tuple.First, tuple.Second))
            );
            Assert.IsTrue(Enumerable
                .Zip(
                    serviceConsumer1.Services.OfType<Service2>(),
                    serviceConsumer2.Services.OfType<Service2>())
                .All(tuple => object.Equals(tuple.First, tuple.Second))
            );
            Assert.IsTrue(Enumerable
                .Zip(
                    serviceConsumer1.Services.OfType<Service3>(),
                    serviceConsumer2.Services.OfType<Service3>())
                .All(tuple => object.Equals(tuple.First, tuple.Second))
            );
        }

        private static readonly IConfig Config = ManualConfig
            .Create(DefaultConfig.Instance)
            .WithOptions(ConfigOptions.DisableOptimizationsValidator);

        [Test]
        public void BenchmarkServiceConsumerResolution()
        {
            var summary = BenchmarkRunner.Run<ResolutionBenchmarks>(Config);

            foreach (var report in summary.Reports)
            {
                TestContext.WriteLine(report);
            }
        }

        [Test]
        public void BenchmarkServiceProviderCreation()
        {
            var summary = BenchmarkRunner.Run<PreparationBenchmarks>(Config);

            foreach (var report in summary.Reports)
            {
                TestContext.WriteLine(report);
            }
        }
    }
}