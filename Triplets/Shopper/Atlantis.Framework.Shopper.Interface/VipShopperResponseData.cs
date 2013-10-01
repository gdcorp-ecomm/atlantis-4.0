using Atlantis.Framework.Interface;
using System.Xml.Linq;
using System.Linq;

namespace Atlantis.Framework.Shopper.Interface
{
  public class VipShopperResponseData : IResponseData
  {
    public static VipShopperResponseData None { get; private set; }

    static VipShopperResponseData()
    {
      None = new VipShopperResponseData();
    }

    // <data count="1"><item shopper_id="832652" PhoneExt="57005" FirstName="Todd" LastName="Redfoot" Email="tredfoot@godaddy.com"/></data>
    public static VipShopperResponseData FromCacheXml(string cacheXml)
    {
      if (string.IsNullOrEmpty(cacheXml))
      {
        return None;
      }

      var dataElement = XElement.Parse(cacheXml);
      var itemElement = dataElement.DescendantsAndSelf("item").FirstOrDefault();

      if (itemElement == null)
      {
        return None;
      }

      var result = new VipShopperResponseData();
      result.RepFirstName = GetAttributeValue(itemElement, "FirstName");
      result.RepLastName = GetAttributeValue(itemElement, "LastName");
      result.RepPhone = GetAttributeValue(itemElement, "PhoneExt");
      result.RepEmail = GetAttributeValue(itemElement, "Email");
      result.IsVipShopper = true;

      return result;
    }

    private static string GetAttributeValue(XElement element, string name)
    {
      XAttribute attribute = element.Attribute(name);
      if (attribute == null)
      {
        return string.Empty;
      }
      else
      {
        return attribute.Value;
      }
    }

    public string RepFirstName { get; private set; }
    public string RepLastName { get; private set; }
    public string RepPhone { get; private set; }
    public string RepEmail { get; private set; }
    public bool IsVipShopper { get; private set; }

    private VipShopperResponseData()
    {
      IsVipShopper = false;
      RepFirstName = string.Empty;
      RepLastName = string.Empty;
      RepPhone = string.Empty;
      RepEmail = string.Empty;
    }

    public string ToXML()
    {
      XElement element = new XElement("VipShopperResponseData");
      element.Add(
        new XAttribute("firstname", RepFirstName),
        new XAttribute("lastname", RepLastName),
        new XAttribute("phone", RepPhone),
        new XAttribute("email", RepEmail)
      );

      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
