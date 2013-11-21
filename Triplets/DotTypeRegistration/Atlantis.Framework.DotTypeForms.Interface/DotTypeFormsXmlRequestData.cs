using System.Globalization;
using System.Xml.Linq;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsXmlRequestData : DotTypeFormsBaseRequestData
  {
    public DotTypeFormsXmlRequestData(string formType, int tldId, string placement, string phase, string marketId, int contextId)
      : base(formType, tldId, placement, phase, marketId, contextId)
    {
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
      return element.ToString();
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(ToXML());
    }
  }
}
