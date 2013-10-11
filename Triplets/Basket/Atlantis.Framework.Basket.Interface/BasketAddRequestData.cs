using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Basket.Interface
{
  public class BasketAddRequestData : RequestData
  {
    private readonly XElement _addRequest;

    public BasketAddRequestData(string shopperId)
    {
      ShopperID = shopperId;
      _addRequest = new XElement("itemRequest");
    }

    public void SetItemRequestAttribute(string name, string value)
    {
      var attribute = _addRequest.Attribute(name);
      if (attribute != null)
      {
        attribute.Value = value;
      }
      else
      {
        _addRequest.Add(new XAttribute(name, value));
      }
    }

    public void AddRequestElement(XElement element)
    {
      _addRequest.Add(element);
    }

    public override string ToXML()
    {
      return _addRequest.ToString(SaveOptions.DisableFormatting);
    }
  }
}
