using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  class TestPlaceHolderEncoding : IPlaceHolderEncoding
  {
    private const string ENCODED_QUOTE = "\\\"";
    private const string QUOTE = "\"";

    public string DecodePlaceHolderData(string rawPlaceHolderData)
    {
      if (!string.IsNullOrEmpty(rawPlaceHolderData))
      {
        return rawPlaceHolderData.Replace(ENCODED_QUOTE, QUOTE);
      }
      else
      {
        return rawPlaceHolderData;
      }
    }

    public string EncodePlaceHolderResult(string placeHolderResult)
    {
      if (!string.IsNullOrEmpty(placeHolderResult))
      {
        return placeHolderResult.Replace(QUOTE, ENCODED_QUOTE);
      }
      else
      {
        return placeHolderResult;
      }
    }
  }
}
