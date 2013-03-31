using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Interface.ProviderContainer
{
  public static class HttpProviderContainer
  {
    public static IProviderContainer Instance
    {
      get { return Providers.Containers.HttpProviderContainer.Instance; }
    }
  }
}
