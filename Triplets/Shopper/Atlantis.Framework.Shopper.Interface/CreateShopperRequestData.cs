using Atlantis.Framework.Shopper.Interface.BaseClasses;
using System.Globalization;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Interface
{
  public class CreateShopperRequestData : ShopperRequestData
  {
    public int PrivateLabelId { get; private set; }

    public CreateShopperRequestData(int privateLabelId, string originIpAddress, string requestedBy)
    {
      PrivateLabelId = privateLabelId;
      RequestedBy = requestedBy;
      OriginIpAddress = originIpAddress;
    }

    public override string ToXML()
    {
      XElement element = new XElement("ShopperCreate");
      element.Add(
        new XAttribute("PLID", PrivateLabelId.ToString(CultureInfo.InvariantCulture)),
        new XAttribute("IPAddress", OriginIpAddress),
        new XAttribute("RequestedBy", RequestedBy));
      return element.ToString(SaveOptions.DisableFormatting);
    }

  }
}
