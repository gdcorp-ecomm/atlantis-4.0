
namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderProvider
  {
    IPlaceHolderData GetPlaceHolderData(string id);

    string ReplacePlaceHolders(string content);
  }
}
