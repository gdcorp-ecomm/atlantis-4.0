using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class ExtendedTLDDataRequestData : RequestData
  {
    public string TLD { get; private set; }

    public ExtendedTLDDataRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string tld)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      TLD = tld;
    }

    public override string GetCacheMD5()
    {
      return TLD;
    }

    public override string ToXML()
    {
      XElement element = new XElement("ExtendedTLDDataRequestData");
      element.Add(new XAttribute("tld", TLD));
      return element.ToString();
    }
  }
}
