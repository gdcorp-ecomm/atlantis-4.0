using System.Xml.Linq;
using Atlantis.Framework.Interface;
using System.Globalization;

namespace Atlantis.Framework.DotTypeClaims.Interface
{
  public class DotTypeClaimsRequestData : RequestData
  {
    public string Domain { get; set; }
    public int TldId { get; set; }
    public string Placement { get; set; }
    public string Phase { get; set; }
    public string MarketId { get; set; }

    public DotTypeClaimsRequestData(int tldId, string placement, string phase, string marketId, string domain)
    {
      TldId = tldId;
      Placement = placement.ToLowerInvariant();
      Phase = phase.ToLowerInvariant();
      MarketId = marketId.ToLowerInvariant();
      Domain = domain;
    }

    public override string ToXML()
    {
      var element = new XElement("DotTypeClaimsRequestData");
      element.Add(new XAttribute("tldid", TldId.ToString(CultureInfo.InvariantCulture)));
      element.Add(new XAttribute("placement", Placement));
      element.Add(new XAttribute("phase", Phase));
      element.Add(new XAttribute("marketId", MarketId));
      element.Add(new XAttribute("domain", Domain));
      return element.ToString();
    }
  }
}
