using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using System.Xml.Linq;
using System.Linq;
using Atlantis.Framework.Providers.DomainContactValidation.Interface;

namespace Atlantis.Framework.AddItem.Interface
{
  public class AddItemRequestData : RequestData
  {
    private readonly XDocument _requestDoc = new XDocument();
    private readonly XElement _itemRequestElement;

    public AddItemRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string clientIP)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _itemRequestElement = new XElement("itemRequest");
      _requestDoc.Add(_itemRequestElement);

      SetDefaults(clientIP);
    }

    public AddItemRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string initialItemRequestXml, string clientIP)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _requestDoc = XDocument.Parse(initialItemRequestXml);

      if (_requestDoc.Root.Name == "itemRequest")
      {
        _itemRequestElement = _requestDoc.Root;
      }
      else
      {
        _itemRequestElement = _requestDoc.Descendants("itemRequest").FirstOrDefault();
      }

      SetDefaults(clientIP);
    }

    private void SetDefaults(string clientIP)
    {
      RequestTimeout = TimeSpan.FromSeconds(2d);

      if (!string.IsNullOrEmpty(clientIP))
      {
        SetItemRequestAttribute("addClientIP", clientIP);
      }
    }

    public void AddSplitTestItem(XElement testElement)
    {
      if (testElement == null)
      {
        throw new ArgumentNullException("testElement can not be NULL");
      }

      XElement splitElement = _itemRequestElement.Element("splitTest");
      if (splitElement == null)
      {
        splitElement = new XElement("splitTest");
        _itemRequestElement.Add(splitElement);
      }
      else
      {
        XElement existingTest = splitElement.Descendants(testElement.Name).FirstOrDefault();
        if (existingTest != null)
        {
          existingTest.Remove();
        }
      }
      splitElement.Add(testElement);
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

    public void AddContactInfo(IDomainContactGroup contactGroup)
    {
      if (!contactGroup.IsValid)
        throw new ArgumentException("Domain contact group has not been validated.");

      string xmlContactInfo = contactGroup.GetContactXml();
      XElement contactElement = XElement.Parse(xmlContactInfo);
      if (contactElement.Name != "contactInfo")
      {
        contactElement = contactElement.Descendants("contactInfo").FirstOrDefault();
      }

      _itemRequestElement.Add(contactElement);
    }

    private void AddItemInt(IEnumerable<KeyValuePair<string, string>> attributes, XElement customXmlElement)
    {
      XElement itemElement = new XElement("item");
      foreach (KeyValuePair<string, string> attr in attributes)
      {
        itemElement.Add(new XAttribute(attr.Key, attr.Value));
      }

      if (customXmlElement != null)
      {
        itemElement.Add(customXmlElement);
      }

      _itemRequestElement.Add(itemElement);
    }

    public void AddItem(IEnumerable<KeyValuePair<string, string>> attributes)
    {
      AddItemInt(attributes, null);
    }

    public void AddItem(IEnumerable<KeyValuePair<string, string>> attributes, XElement customXmlElement)
    {
      AddItemInt(attributes, customXmlElement);
    }

    public void AddItem(IEnumerable<KeyValuePair<string, string>> attributes, string customXml)
    {
      XElement customXmlElement = null;
      if (!string.IsNullOrEmpty(customXml))
      {
        customXmlElement = XElement.Parse(customXml);
      }
      AddItemInt(attributes, customXmlElement);
    }

    private IEnumerable<KeyValuePair<string, string>> GetAttributes(string unifiedProductId, string quantity, IEnumerable<KeyValuePair<string, string>> otherAttributes)
    {
      var result = new List<KeyValuePair<string, string>>
        {
          new KeyValuePair<string, string>("unifiedProductID", unifiedProductId),
          new KeyValuePair<string, string>("quantity", quantity)
        };

      if (otherAttributes != null)
      {
        result.AddRange(otherAttributes);
      }

      return result;
    }

    public void AddItem(string unifiedProductId, string quantity)
    {
      var attributes = GetAttributes(unifiedProductId, quantity, null);
      AddItemInt(attributes, null);
    }

    public void AddItem(string unifiedProductId, string quantity, string customXml)
    {
      var attributes = GetAttributes(unifiedProductId, quantity, null);
      AddItem(attributes, customXml);
    }

    public void AddItem(string unifiedProductId, string quantity, IEnumerable<KeyValuePair<string, string>> attributes, string customXml)
    {
      var allAttributes = GetAttributes(unifiedProductId, quantity, attributes);
      AddItem(allAttributes, customXml);
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("AddItem is not a cacheable request.");
    }

    public override string ToXML()
    {
      return _itemRequestElement.ToString(SaveOptions.DisableFormatting);
    }

    #endregion
  }
}
