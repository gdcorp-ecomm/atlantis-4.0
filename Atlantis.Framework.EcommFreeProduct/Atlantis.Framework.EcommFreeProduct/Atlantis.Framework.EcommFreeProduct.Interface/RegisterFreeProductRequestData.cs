
namespace Atlantis.Framework.EcommFreeProduct.Interface
{
  using System;
  using System.Collections.Generic;
  using System.Xml.Linq;
  using Atlantis.Framework.Interface;

  public class RegisterFreeProductRequestData : RequestData
  {
    private readonly XElement _itemRequestElement;
    private readonly XDocument _requestDoc = new XDocument();
    private static TimeSpan _DEFAULTIMEOUT = TimeSpan.FromSeconds(15d);

    public RegisterFreeProductRequestData(string shopperId, string clientIP)
    {
      ShopperID = shopperId;
      RequestTimeout = _DEFAULTIMEOUT;
      _itemRequestElement = new XElement("freeItemRequest");
      _requestDoc.Add(_itemRequestElement);

      if (!string.IsNullOrEmpty(clientIP))
      {
        SetItemRequestAttribute("addClientIP", clientIP);
      }
    }

    public void AddItem(string unifiedProductId, string quantity)
    {
      AddItem(unifiedProductId, quantity, string.Empty, null);
    }

    public void AddItem(string unifiedProductId, string quantity, string renewalShopperProfileID)
    {
      AddItem(unifiedProductId, quantity, renewalShopperProfileID, null);
    }

    public void AddItem(string unifiedProductId, string quantity, string renewalShopperProfileID, IEnumerable<KeyValuePair<string, string>> otherAttributes)
    {
      _itemRequestElement.Add(CreateItemElement(CreateAttributes(unifiedProductId, quantity, otherAttributes), null));
      if (!string.IsNullOrEmpty(renewalShopperProfileID))
      {
        SetItemRequestAttribute("renewalShopperProfileID", renewalShopperProfileID);
      }
    }


    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in RegFreeProductRequestData");
    }

    public void SetItemRequestAttribute(string name, string value)
    {
      var attribute = _itemRequestElement.Attribute(name);
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
      var returnValue = new List<KeyValuePair<string, string>>
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
      var returnValue = new XElement("item");
      foreach (var attr in attributes)
      {               
        returnValue.Add(new XAttribute(attr.Key, attr.Value));
      }

      if (!ReferenceEquals(null, customXmlElement))
      {
        returnValue.Add(customXmlElement);
      }

      return returnValue;
    }
  }
}
