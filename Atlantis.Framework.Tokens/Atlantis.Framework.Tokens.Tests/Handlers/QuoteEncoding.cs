using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.Tokens.Tests.Handlers
{
  public class QuoteEncoding : ITokenEncoding
  {
    public string DecodeTokenData(string rawTokenData)
    {
      return rawTokenData.Replace("\\\"", "\"");
    }

    public string EncodeTokenResult(string tokenResult)
    {
      return tokenResult.Replace("\"", "\\\"");
    }
  }
}
