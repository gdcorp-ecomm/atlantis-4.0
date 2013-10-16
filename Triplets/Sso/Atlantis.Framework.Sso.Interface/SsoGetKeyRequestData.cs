using Atlantis.Framework.Interface;
using Atlantis.Framework.Sso.Interface.JsonHelperClasses;

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
    }

    public SsoGetKeyRequestData(Token token)
    {
      Token = token;
    }
  }
}
