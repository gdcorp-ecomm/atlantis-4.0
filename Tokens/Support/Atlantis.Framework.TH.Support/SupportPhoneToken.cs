using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Support
{
  class SupportPhoneToken : XmlToken
  {
    public string RenderType
    {
      get { return (TokenData != null) ? TokenData.Name.ToString() : string.Empty; }
    }

    public SupportPhoneToken(string key, string data, string fullTokenString) : base(key, data, fullTokenString)
    {
    }
  }
}
