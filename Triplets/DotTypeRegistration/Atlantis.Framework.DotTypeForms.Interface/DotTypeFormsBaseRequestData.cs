using System.Globalization;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public abstract class DotTypeFormsBaseRequestData : RequestData
  {
    public int TldId { get; set; }
    public string Placement { get; set; }
    public string Phase { get; set; }
    public string MarketId { get; set; }
    public int ContextId { get; set; }
    public string FormType { get; set; }

    protected DotTypeFormsBaseRequestData(string formType, int tldId, string placement, string phase, string marketId, int contextId)
    {
      FormType = formType;
      TldId = tldId;
      Placement = placement.ToLowerInvariant();
      Phase = phase.ToLowerInvariant();
      MarketId = marketId.ToLowerInvariant();
      ContextId = contextId;
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
