using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Products.Interface
{
  public class NonUnifiedPfidRequestData : RequestData
  {
    public int UnifiedProductId { get; private set; }
    public int PrivateLabelId { get; private set; }

    public NonUnifiedPfidRequestData(int unifiedProductId, int privateLabelId)
    {
      UnifiedProductId = unifiedProductId;
      PrivateLabelId = privateLabelId;
    }

    public override string GetCacheMD5()
    {
      return string.Format("{0}:{1}", UnifiedProductId.ToString(), PrivateLabelId.ToString());
    }

    public override string ToXML()
    {
      XElement element = new XElement("NonUnifiedPfidRequestData");
      element.Add(
        new XAttribute("unifiedproductid", UnifiedProductId.ToString()),
        new XAttribute("privatelabelid", PrivateLabelId.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
