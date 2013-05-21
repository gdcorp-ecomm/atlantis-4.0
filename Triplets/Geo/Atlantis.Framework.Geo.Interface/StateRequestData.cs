using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Interface
{
  public class StateRequestData : RequestData
  {
    public int CountryId { get; private set; }

    public StateRequestData(int countryId)
    {
      CountryId = countryId;
    }

    public override string GetCacheMD5()
    {
      return CountryId.ToString();
    }

    public override string ToXML()
    {
      XElement element = new XElement("StateRequestData");
      element.Add(new XAttribute("countryid", CountryId.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
