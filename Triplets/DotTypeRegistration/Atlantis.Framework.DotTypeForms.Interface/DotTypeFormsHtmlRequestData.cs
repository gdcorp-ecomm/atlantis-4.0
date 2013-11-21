using System.Globalization;
using System.Xml.Linq;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsHtmlRequestData : DotTypeFormsBaseRequestData
  {
    public string Domain { get; set; }

    public DotTypeFormsHtmlRequestData(string formType, int tldId, string placement, string phase, string marketId, int contextId, string domain)
      : base(formType, tldId, placement, phase, marketId, contextId)
    {
      Domain = domain;
    }

    public override string ToXML()
    {
      var element = new XElement("parameters");
      element.Add(new XAttribute("formtype", FormType.ToString(CultureInfo.InvariantCulture)));
      element.Add(new XAttribute("tldid", TldId.ToString(CultureInfo.InvariantCulture)));
      element.Add(new XAttribute("placement", Placement));
      element.Add(new XAttribute("phase", Phase));
      element.Add(new XAttribute("marketId", MarketId));
      element.Add(new XAttribute("contextid", ContextId.ToString(CultureInfo.InvariantCulture)));
      element.Add(new XAttribute("domain", Domain));
      return element.ToString();
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(ToXML());
    }
  }
}
