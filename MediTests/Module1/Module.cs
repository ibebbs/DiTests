
using Microsoft.Extensions.DependencyInjection;

namespace MediTests.Module1
{
    public class Dependency : IModule1Dependency { }

    public class Module : IModule
    {
        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IModule1Dependency, Dependency>();

            return services;
        }
    }
}
