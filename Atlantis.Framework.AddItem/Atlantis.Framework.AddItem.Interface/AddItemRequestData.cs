using System;
using System.Collections.Generic;
using System.Xml;

using Atlantis.Framework.Interface;
using Atlantis.Framework.DomainContactCheck.Interface;

namespace Atlantis.Framework.AddItem.Interface
{
  public class AddItemRequestData : RequestData
  {
    private readonly XmlDocument m_xdRequest = new XmlDocument();
    private readonly XmlElement m_xlItemRequest;

    public AddItemRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      m_xlItemRequest = m_xdRequest.CreateElement("itemRequest");
      m_xdRequest.AppendChild(m_xlItemRequest);
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public AddItemRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string initialItemRequestXml)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      m_xdRequest = new XmlDocument();
      m_xdRequest.LoadXml(initialItemRequestXml);

      m_xlItemRequest = m_xdRequest.SelectSingleNode("//itemRequest") as XmlElement;

      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public void SetItemRequestAttribute(string sName, string sValue)
    {
      m_xlItemRequest.SetAttribute(sName, sValue);
    }

    public void AddContactInfo(DomainContactGroup oContactGroup)
    {
      if (!oContactGroup.IsValid)
        throw new ArgumentException("Domain contact group has not been validated.");

      string xmlContactInfo = oContactGroup.GetContactXml();
      var xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(xmlContactInfo);
      XmlNode m_ContactInfoNode = xmlDoc.SelectSingleNode("//" + DomainContactGroup.ContactInfoElementName);
      m_ContactInfoNode = m_xdRequest.ImportNode(m_ContactInfoNode, true);
      m_xlItemRequest.AppendChild(m_ContactInfoNode);
    }

    public void AddItem(IEnumerable<KeyValuePair<string, string>> oAttributes)
    {
      AddItem(oAttributes, string.Empty);
    }

    public void AddItem(IEnumerable<KeyValuePair<string, string>> attributes, string customXml)
    {
      XmlElement xlItem = m_xdRequest.CreateElement("item");
      foreach (KeyValuePair<string, string> attr in attributes)
      {
        xlItem.SetAttribute(attr.Key, attr.Value);
      }

      if (customXml.Length > 0)
      {
        xlItem.InnerXml = customXml;
      }

      m_xlItemRequest.AppendChild(xlItem);
    }

    public void AddItem(string unifiedProductId, string quantity)
    {
      AddItem(unifiedProductId, quantity, null, string.Empty);
    }

    public void AddItem(string unifiedProductId, string quantity, string customXml)
    {
      AddItem(unifiedProductId, quantity, null, customXml);
    }

    public void AddItem(string unifiedProductId, string quantity, IEnumerable<KeyValuePair<string, string>> attributes, string customXml)
    {
      var lstAttributes = new List<KeyValuePair<string, string>>
                            {
                              new KeyValuePair<string, string>("unifiedProductID", unifiedProductId),
                              new KeyValuePair<string, string>("quantity", quantity)
                            };

      if (attributes != null)
      {
        lstAttributes.AddRange(attributes);
      }

      AddItem(lstAttributes, customXml);
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("AddItem is not a cacheable request.");
    }

    public override string ToXML()
    {
      return m_xdRequest.InnerXml;
    }

    #endregion
  }
}
