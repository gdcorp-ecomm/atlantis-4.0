
namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderProvider
  {
    IPlaceHolderData GetPlaceHolderData(string type);

    string ReplacePlaceHolders(string content);
  }
}
