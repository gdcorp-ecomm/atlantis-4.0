using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Rules.Interface
{
  public class RulesMainRequestData : RequestData
  {
    public string RuleName { get; private set; }

    public RulesMainRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string ruleName)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RuleName = ruleName;
    }

    public override string ToXML()
    {
      var element = new XElement("RulesMainRequestData");
      element.Add(new XAttribute("ruleName", RuleName));
      return element.ToString();
    }

    public override string GetCacheMD5()
    {
      return RuleName;
    }
  }
}
