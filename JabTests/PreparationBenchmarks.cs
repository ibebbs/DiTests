using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;

namespace JabTests
{
    [SimpleJob(RuntimeMoniker.Net50, launchCount: 1, warmupCount: 3, targetCount: 5, invocationCount: 100)]
    [MinColumn, MaxColumn, MedianColumn, KurtosisColumn]
    public class PreparationBenchmarks
    {
        [Benchmark]
        public void PrepareServiceProvider()
        {
            var dependency = Container.ServiceProvider.Get<IDependency>();

            Assert.IsNotNull(dependency);
        }
    }
}
