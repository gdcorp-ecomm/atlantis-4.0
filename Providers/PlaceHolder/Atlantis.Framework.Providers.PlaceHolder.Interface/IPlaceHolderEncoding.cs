
namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderEncoding
  {
    string DecodePlaceHolderData(string rawPlaceHolderData);

    string EncodePlaceHolderResult(string placeHolderResult);
  }
}
