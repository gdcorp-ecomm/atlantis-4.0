
namespace Atlantis.Framework.RegFreeProduct.Interface
{
  using System;
  using System.Collections.Generic;
  using System.Xml.Linq;
  using Atlantis.Framework.Interface;

  public class RegisterFreeProductRequestData : RequestData
  {
    private readonly XElement _itemRequestElement;
    private readonly XDocument _requestDoc = new XDocument();
    private static TimeSpan _DEFAULTIMEOUT = TimeSpan.FromSeconds(15.0);

    public RegisterFreeProductRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string clientIP)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = _DEFAULTIMEOUT;
      _itemRequestElement = new XElement("freeItemRequest");
      _requestDoc.Add(_itemRequestElement);

      SetDefaults(clientIP);
    }

    public void AddItem(string unifiedProductId, string quantity)
    {
      this._itemRequestElement.Add(CreateItemElement(CreateAttributes(unifiedProductId, quantity, null), null));
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in RegFreeProductRequestData");
    }

    public void SetItemRequestAttribute(string name, string value)
    {
      XAttribute attribute = _itemRequestElement.Attribute(name);
      if (attribute != null)
      {
        attribute.Value = value;
      }
      else
      {
        _itemRequestElement.Add(new XAttribute(name, value));
      }
    }

    public override string ToXML()
    {
      return _itemRequestElement.ToString(SaveOptions.DisableFormatting);
    }

    private IEnumerable<KeyValuePair<string, string>> CreateAttributes(string unifiedProductId, string quantity, IEnumerable<KeyValuePair<string, string>> otherAttributes)
    {
      List<KeyValuePair<string, string>> returnValue = new List<KeyValuePair<string, string>>
              {
                new KeyValuePair<string, string>("unifiedProductID", unifiedProductId),
                new KeyValuePair<string, string>("quantity", quantity)
              };

      if (!ReferenceEquals(null, otherAttributes))
      {
        returnValue.AddRange(otherAttributes);
      }

      return returnValue;
    }

    private XElement CreateItemElement(IEnumerable<KeyValuePair<string, string>> attributes, XElement customXmlElement)
    {
      XElement returnValue = new XElement("item");
      foreach (KeyValuePair<string, string> attr in attributes)
      {
        returnValue.Add(new XAttribute(attr.Key, attr.Value));
      }

      if (!ReferenceEquals(null, customXmlElement))
      {
        returnValue.Add(customXmlElement);
      }

      return returnValue;
    }

    private void SetDefaults(string clientIP)
    {
      RequestTimeout = TimeSpan.FromSeconds(2d);

      if (!string.IsNullOrEmpty(clientIP))
      {
        SetItemRequestAttribute("addClientIP", clientIP);
      }
    }

  }
}
