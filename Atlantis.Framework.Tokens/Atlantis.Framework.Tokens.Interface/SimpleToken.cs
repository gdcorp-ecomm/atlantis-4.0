using System;

namespace Atlantis.Framework.Tokens.Interface
{
  public class SimpleToken : TokenBase<string>
  {
    public SimpleToken(string key, string data, string fullTokenString)
      : base(key, data, fullTokenString)
    {
    }

    protected override string DeserializeTokenData(string data)
    {
      return data;
    }
  }
}
