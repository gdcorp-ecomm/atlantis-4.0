using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Interface
{
  public class VerifyShopperResponseData : IResponseData
  {
    public static VerifyShopperResponseData NotVerified { get; private set; }

    static VerifyShopperResponseData()
    {
      NotVerified = new VerifyShopperResponseData();
    }

    public int PrivateLabelId { get; private set; }
    public bool IsVerified { get; private set; }

    public static VerifyShopperResponseData FromPrivateLabelId(int privateLabelId)
    {
      if (privateLabelId > 0)
      {
        return new VerifyShopperResponseData(privateLabelId);
      }

      return NotVerified;
    }

    private VerifyShopperResponseData(int privateLabelId)
    {
      PrivateLabelId = privateLabelId;
      IsVerified = true;
    }

    private VerifyShopperResponseData()
    {
      PrivateLabelId = 0;
      IsVerified = false;
    }

    public AtlantisException GetException()
    {
      return null;
    }

    public string ToXML()
    {
      XElement element = new XElement("VerifyShopperResponseData");
      element.Add(
        new XAttribute("privatelabelid", PrivateLabelId.ToString()),
        new XAttribute("isverified", IsVerified.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
