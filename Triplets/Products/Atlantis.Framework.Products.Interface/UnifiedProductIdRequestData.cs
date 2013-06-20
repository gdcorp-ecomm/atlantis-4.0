using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Products.Interface
{
  public class UnifiedProductIdRequestData : RequestData
  {
    public int NonUnifiedPfid { get; private set; }
    public int PrivateLabelId { get; private set; }

    public UnifiedProductIdRequestData(int nonUnifiedPfid, int privateLabelId)
    {
      NonUnifiedPfid = nonUnifiedPfid;
      PrivateLabelId = privateLabelId;
    }

    public override string GetCacheMD5()
    {
      return string.Concat(NonUnifiedPfid.ToString(), "|", PrivateLabelId.ToString());
    }

    public override string ToXML()
    {
      XElement element = new XElement("UnifiedProductIdRequestData");
      element.Add(new XAttribute("pfid", NonUnifiedPfid.ToString()), new XAttribute("privatelabelid", PrivateLabelId.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
