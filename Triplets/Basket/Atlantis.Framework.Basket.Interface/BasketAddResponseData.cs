using Atlantis.Framework.Interface;
using System.Globalization;
using System.Xml.Linq;

namespace Atlantis.Framework.Basket.Interface
{
  public class BasketAddResponseData : IResponseData
  {
    public static BasketAddResponseData FromResponseXml(string responseXml)
    {
      var responseElement = XElement.Parse(responseXml);
      var status = BasketResponseStatus.FromResponseElement(responseElement);
      return new BasketAddResponseData(status);
    }

    public BasketResponseStatus Status { get; private set; }

    private BasketAddResponseData(BasketResponseStatus status)
    {
      Status = status;
    }

    public string ToXML()
    {
      XElement element = new XElement("BasketAddResponseData");
      element.Add(new XAttribute("haserrors", Status.HasErrors.ToString(CultureInfo.InvariantCulture)));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
