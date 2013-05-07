using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderHandler
  {
    string Name { get; }

    string GetPlaceHolderContent(string name, string data, IProviderContainer providerContainer);
  }
}
