using Atlantis.Framework.Shopper.Interface.BaseClasses;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Interface
{
  public class CreateShopperResponseData : ShopperResponseData
  {
    public static CreateShopperResponseData FromShopperXml(string shopperXml)
    {
      var shopperElement = XElement.Parse(shopperXml);
      ShopperResponseStatus status = ShopperResponseStatus.FromResponseElement(shopperElement);

      var result = new CreateShopperResponseData(status);

      if (result.Status.Status != ShopperResponseStatusType.Success)
      {
        return result;
      }

      var idAttribute = shopperElement.Attribute("ID");
      if (idAttribute == null)
      {
        return new CreateShopperResponseData(ShopperResponseStatus.UnknownError);
      }
        
      result.ShopperId = idAttribute.Value;
      return result;
    }

    public string ShopperId { get; private set; }

    private CreateShopperResponseData(ShopperResponseStatus status)
      : base(status)
    {
      ShopperId = string.Empty;
    }

    public override string ToXML()
    {
      var element = new XElement("CreateShopperResponseData");
      element.Add(
        new XAttribute("ID", ShopperId),
        new XAttribute("status", Status.Status.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
