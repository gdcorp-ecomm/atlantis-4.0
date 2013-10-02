using System.Globalization;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class TLDPhaseDateListRequestData : RequestData
  {
    public int TldId { get; private set; }

    public TLDPhaseDateListRequestData(int tldId)
    {
      TldId = tldId;
    }

    public override string GetCacheMD5()
    {
      return TldId.ToString(CultureInfo.InvariantCulture);
    }

    public override string ToXML()
    {
      var element = new XElement("TLDPhaseDateListRequestData");
      element.Add(new XAttribute("TldId", TldId));
      return element.ToString();
    }
  }
}
