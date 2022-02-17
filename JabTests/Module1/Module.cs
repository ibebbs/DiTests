using Jab;

namespace JabTests.Module1
{
    public class Dependency : IModule1Dependency { }

    [ServiceProviderModule]
    [Transient(typeof(IModule1Dependency), typeof(Dependency))]
    public interface IModule { }
}
