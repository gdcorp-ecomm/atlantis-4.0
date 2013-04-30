using System.Globalization;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SplitTesting.Interface
{
  public class ActiveSplitTestsRequestData : RequestData
  {
    public string CategoryName { get; set; }

    public ActiveSplitTestsRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string categoryName)
    : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      CategoryName = categoryName;
    }

    public override string ToXML()
    {
      var element = new XElement("ActiveSplitTestsRequestData");
      element.Add(new XAttribute("categoryname", CategoryName));
      return element.ToString();
    }

    public override string GetCacheMD5()
    {
      return CategoryName;
    }
  }
}
