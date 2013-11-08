using Atlantis.Framework.Interface;
using Atlantis.Framework.Sso.Interface.JsonHelperClasses;
using System;

namespace Atlantis.Framework.Sso.Interface
{
  public class SsoGetKeyRequestData : RequestData
  {
    public Token Token { get; private set; }

    public SsoGetKeyRequestData(string rawTokenData, int privateLabelId )
    {
      Token = new Token();
      Token.data = rawTokenData;
      Token.PrivateLabelId = privateLabelId;
      RequestTimeout = new TimeSpan(0, 0, 5);
    }

    public SsoGetKeyRequestData(Token token)
    {
      Token = token;
      RequestTimeout = new TimeSpan(0, 0, 5);
    }
    
    public override string GetCacheMD5()
    {
      return Token.Header.kid;
    }
  }
}
