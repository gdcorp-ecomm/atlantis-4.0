
namespace Atlantis.Framework.Interface.Tests
{
  public interface IPersonProvider : IProviderContainer
  {
    INameProvider NameProvider { get; }
  }
}
