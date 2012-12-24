using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TLDMLByNameRequestData : RequestData
  {
    public string TLD { get; private set; }

    public TLDMLByNameRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string tld)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      if (tld != null)
      {
        TLD = tld.ToUpperInvariant();
      }
      else
      {
        TLD = string.Empty;
      }
    }

    public override string GetCacheMD5()
    {
      return TLD;
    }

    public override string ToXML()
    {
      XElement element = new XElement("TLDMLByNameRequestData");
      element.Add(new XAttribute("tld", TLD));
      return element.ToString();
    }
  }
}
