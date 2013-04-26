using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.Providers.CDS
{
  internal class CDSWidgetTokenEncoding : ITokenEncoding
  {
    private const string _ENCODEDQUOTE = "\\\\\\\"";
    private const string _QUOTE = "\"";

    public string DecodeTokenData(string rawTokenData)
    {
      if (!string.IsNullOrEmpty(rawTokenData))
      {
        return rawTokenData.Replace(_ENCODEDQUOTE, _QUOTE);
      }
      else
      {
        return rawTokenData;
      }
    }

    public string EncodeTokenResult(string tokenResult)
    {
      if (!string.IsNullOrEmpty(tokenResult))
      {
        return tokenResult.Replace(_QUOTE, _ENCODEDQUOTE);
      }
      else
      {
        return tokenResult;
      }
    }
  }
}
