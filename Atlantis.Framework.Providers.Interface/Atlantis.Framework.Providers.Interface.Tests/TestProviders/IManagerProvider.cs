
namespace Atlantis.Framework.Interface.Tests
{
  public interface IManagerProvider : IProviderContainer
  {
    INameProvider NameProvider { get; }

    IEmployeeProvider EmployeeProvider { get; }
  }
}
