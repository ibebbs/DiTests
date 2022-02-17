
using Microsoft.Extensions.DependencyInjection;

namespace MediTests.Module2
{
    public class Dependency : IModule2Dependency { }

    public class Module : IModule
    {
        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IModule2Dependency, Dependency>();

            return services;
        }
    }
}
