using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Containers.Tests
{
  public class NameProviderDoesNotImplementInterface : ProviderBase
  {
    public NameProviderDoesNotImplementInterface(IProviderContainer providerContainer) : base(providerContainer)
    {
    }
  }
}
