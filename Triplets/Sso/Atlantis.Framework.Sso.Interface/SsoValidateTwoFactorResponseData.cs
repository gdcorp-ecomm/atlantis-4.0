using Atlantis.Framework.Interface;
using System.Xml.Linq;
using Atlantis.Framework.Sso.Interface.JsonHelperClasses;

namespace Atlantis.Framework.Sso.Interface
{
  public class SsoValidateTwoFactorResponseData : SsoValidateShopperAndGetTokenResponseData
  {
    public SsoValidateTwoFactorResponseData(Token token) : base(token) { }

    public SsoValidateTwoFactorResponseData(AtlantisException aex)
      : base(aex)
    {
    }

    public override string ToXML()
    {
      // Use this method to output small debug xml.
      XElement element = new XElement("SsoValidateTwoFactorResponseData");
      return element.ToString(SaveOptions.DisableFormatting);
    }

  }
}
