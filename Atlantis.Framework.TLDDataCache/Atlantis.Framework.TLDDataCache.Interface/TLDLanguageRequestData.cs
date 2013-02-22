using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class TLDLanguageRequestData : RequestData
  {
    public int TLDId { get; private set; }

    public TLDLanguageRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int tldId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      TLDId = tldId;
    }

    public override string GetCacheMD5()
    {
      return TLDId.ToString();
    }

    public override string ToXML()
    {
      XElement root = new XElement("GetLanguageListByTLDId");

      XElement element = new XElement("param");
      element.Add(new XAttribute("name", "tldId"));
      element.Add(new XAttribute("value", TLDId));
      root.Add(element);

      return root.ToString();
    }
  }
}
