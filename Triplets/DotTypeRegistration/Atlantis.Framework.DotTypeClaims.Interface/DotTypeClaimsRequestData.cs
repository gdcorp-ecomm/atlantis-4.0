using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeClaims.Interface
{
  public class DotTypeClaimsRequestData : RequestData
  {
    public string[] Domains { get; set; }

    public DotTypeClaimsRequestData(string[] domains)
    {
      Domains = domains;
    }

    public override string ToXML()
    {
      var element = new XElement("DotTypeClaimsRequestData");
      element.Add(new XAttribute("domains", string.Join(",", Domains)));
      return element.ToString();
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(string.Join(",", Domains).ToLowerInvariant());
    }
  }
}
