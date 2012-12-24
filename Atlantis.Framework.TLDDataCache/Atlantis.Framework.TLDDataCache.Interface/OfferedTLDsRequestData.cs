using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class OfferedTLDsRequestData : RequestData
  {
    public OfferedTLDProductTypes TLDProductType { get; private set; }
    public int PrivateLabelId { get; private set; }

    public OfferedTLDsRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId, OfferedTLDProductTypes tldProductType)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      this.PrivateLabelId = privateLabelId;
      this.TLDProductType = tldProductType;
    }

    public override string ToXML()
    {
      XElement element = new XElement("OfferedTLDsRequestData");
      element.Add(new XAttribute("privatelabelid", PrivateLabelId.ToString()));
      element.Add(new XAttribute("producttype", ((int)TLDProductType).ToString()));
      return element.ToString();
    }

    public override string GetCacheMD5()
    {
      return string.Concat(PrivateLabelId.ToString(), ".", ((int)TLDProductType).ToString());
    }
  }
}
