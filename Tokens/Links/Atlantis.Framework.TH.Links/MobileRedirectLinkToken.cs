using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Links
{
  public class MobileRedirectLinkToken : XmlToken
  {
    public string RedirectKey
    {
      get { return GetAttributeText("redirectKey", string.Empty); }
    }

    public MobileRedirectLinkToken(string key, string data, string fullTokenString) : base(key, data, fullTokenString)
    {
    }
  }
}
