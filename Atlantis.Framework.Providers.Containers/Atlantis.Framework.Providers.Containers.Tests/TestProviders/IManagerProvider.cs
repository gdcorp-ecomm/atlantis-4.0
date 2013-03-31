using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Containers.Tests
{
  public interface IManagerProvider : IProviderContainer
  {
    INameProvider NameProvider { get; }

    IEmployeeProvider EmployeeProvider { get; }
  }
}
