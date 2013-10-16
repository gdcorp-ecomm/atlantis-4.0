using Atlantis.Framework.Interface;
using System.Xml.Linq;
using Atlantis.Framework.Sso.Interface.JsonHelperClasses;

namespace Atlantis.Framework.Sso.Interface
{
  public class SsoValidateShopperAndGetTokenResponseData : IResponseData
  {
    private AtlantisException _aex;

    readonly Token _token = new Token();
    public Token Token
    {
      get { return _token; }
    }


    // Sample static method for creating the response (ref Clean Code)
    public SsoValidateShopperAndGetTokenResponseData (Token token)
    {
      _token = token;
    }

    public SsoValidateShopperAndGetTokenResponseData(AtlantisException aex)
    {
      _aex = aex;
    }

    public virtual string ToXML()
    {
      // Use this method to output small debug xml.
      XElement element = new XElement("SsoGetTokenResponseData");
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return _aex;
    }
  }
}
