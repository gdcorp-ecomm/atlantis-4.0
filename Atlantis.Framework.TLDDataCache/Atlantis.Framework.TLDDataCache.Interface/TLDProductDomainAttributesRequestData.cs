using System.Text;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class TLDProductDomainAttributesRequestData : RequestData
  {
    public int TldId { get; private set; }
    public string TldPhase { get; private set; }
    public int PrivateLabelResellerTypeId { get; private set; }
    public int ProductTypeId { get; private set; }

    public TLDProductDomainAttributesRequestData(int tldId, string tldPhase, int privateLabelResellerTypeId, int productTypeId)
    {
      TldId = tldId;
      TldPhase = tldPhase;
      PrivateLabelResellerTypeId = privateLabelResellerTypeId;
      ProductTypeId = productTypeId;
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(TldId.ToString(), TldPhase, PrivateLabelResellerTypeId.ToString(),
                                  ProductTypeId.ToString());
    }

    public override string ToXML()
    {
      var element = new XElement("TLDProductDomainAttributesRequestData");
      element.Add(new XAttribute("TldId", TldId));
      element.Add(new XAttribute("TldPhase", TldPhase));
      element.Add(new XAttribute("PrivateLabelResellerTypeId", PrivateLabelResellerTypeId));
      element.Add(new XAttribute("ProductTypeId", ProductTypeId));
      return element.ToString();
    }
  }
}
