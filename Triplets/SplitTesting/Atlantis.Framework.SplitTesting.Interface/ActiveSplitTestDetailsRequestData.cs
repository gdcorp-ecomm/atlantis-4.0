using System.Globalization;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SplitTesting.Interface
{
  public class ActiveSplitTestDetailsRequestData : RequestData
  {
    public int SplitTestId { get; set; }

    public ActiveSplitTestDetailsRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int splitTestId)
    : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      SplitTestId = splitTestId;
    }

    public override string ToXML()
    {
      var element = new XElement("ActiveSplitTestDetailsRequestData");
      element.Add(new XAttribute("splittestid", SplitTestId.ToString(CultureInfo.InvariantCulture)));
      return element.ToString();
    }

    public override string GetCacheMD5()
    {
      return SplitTestId.ToString(CultureInfo.InvariantCulture);
    }
  }
}
