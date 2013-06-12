using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Containers.Tests
{
  public interface INameProvider : IProviderContainer
  {
    string FirstName { get; set; }
  }
}