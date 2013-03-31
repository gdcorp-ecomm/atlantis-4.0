using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Containers.Tests
{
  public interface IPersonProvider : IProviderContainer
  {
    INameProvider NameProvider { get; }
  }
}
