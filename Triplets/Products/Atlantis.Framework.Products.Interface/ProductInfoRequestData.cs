using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Products.Interface
{
  public class ProductInfoRequestData : RequestData
  {
    public int UnifiedProductId { get; set; }
    public int PrivateLabelId { get; set; }

    public ProductInfoRequestData(int unifiedProductId, int privateLabelId)
    {
      UnifiedProductId = unifiedProductId;
      PrivateLabelId = privateLabelId;
    }

    public override string GetCacheMD5()
    {
      return string.Concat(UnifiedProductId.ToString(), "|", PrivateLabelId.ToString());
    }

    public override string ToXML()
    {
      XElement element = new XElement("ProductInfoRequestData");
      element.Add(new XAttribute("productid", UnifiedProductId.ToString()), new XAttribute("privatelabelid", PrivateLabelId.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
