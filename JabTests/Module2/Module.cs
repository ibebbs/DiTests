using Jab;

namespace JabTests.Module2
{
    public class Dependency : IModule2Dependency { }

    [ServiceProviderModule]
    [Transient(typeof(IModule2Dependency), typeof(Dependency))]
    public interface IModule { }
}
