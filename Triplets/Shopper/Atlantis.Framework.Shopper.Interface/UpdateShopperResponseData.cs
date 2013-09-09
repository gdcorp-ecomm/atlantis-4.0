using Atlantis.Framework.Shopper.Interface.BaseClasses;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Interface
{
  public class UpdateShopperResponseData : ShopperResponseData
  {
    public static UpdateShopperResponseData FromShopperXml(string shopperXml)
    {
      var shopperElement = XElement.Parse(shopperXml);
      ShopperResponseStatus status = ShopperResponseStatus.FromResponseElement(shopperElement);

      var result = new UpdateShopperResponseData(status);
      return result;
    }

    private UpdateShopperResponseData(ShopperResponseStatus status)
      : base(status)
    {
    }

    public override string ToXML()
    {
      var element = new XElement("UpdateShopperResponseData");
      element.Add(new XAttribute("status", Status.Status.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
