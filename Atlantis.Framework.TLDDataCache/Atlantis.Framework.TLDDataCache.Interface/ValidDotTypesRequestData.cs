using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class ValidDotTypesRequestData : RequestData
  {
    public ValidDotTypesRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
    }

    public override string GetCacheMD5()
    {
      return "0";
    }

    public override string ToXML()
    {
      XElement element = new XElement("ValidDotTypesRequestData");
      return element.ToString();
    }
  }
}
