
namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderData
  {
    bool TryGetParameter(string key, out string value);
  }
}
