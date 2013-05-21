using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Interface
{
  public class RegionRequestData : RequestData
  {
    public int RegionTypeId { get; private set; }
    public string RegionName { get; private set; }

    public RegionRequestData(int regionTypeId, string regionName)
    {
      RegionTypeId = regionTypeId;
      RegionName = regionName ?? string.Empty;
    }

    public override string GetCacheMD5()
    {
      return string.Concat(RegionTypeId.ToString(), ":", RegionName.ToLowerInvariant());
    }

    public override string ToXML()
    {
      XElement element = new XElement("RegionRequestData");
      element.Add(
        new XAttribute("regiontypeid", RegionTypeId.ToString()),
        new XAttribute("regionname", RegionName));

      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
