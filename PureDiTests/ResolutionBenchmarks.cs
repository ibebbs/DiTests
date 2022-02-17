using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;

namespace PureDiTests
{
    [SimpleJob(RuntimeMoniker.Net50)]
    [MinColumn, MaxColumn, MedianColumn, KurtosisColumn]
    public class ResolutionBenchmarks
    {
        private IServiceProvider serviceProvider;

        [GlobalSetup]
        public void Setup()
        {
            this.serviceProvider = Container.ServiceProvider;
        }

        [Benchmark]
        public void ResolveServiceConsumer()
        {
            serviceProvider.Get<IServiceConsumer>();
        }
    }
}
