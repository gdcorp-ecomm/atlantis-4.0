using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetDomainContacts.Interface
{
  public class DCCGetDomainContactsRequestData : RequestData
  {
    public string DomainName { get; private set; }

    public string DccDomainUser { get; private set; }

    public DCCGetDomainContactsRequestData( string shopperId,
                                            string sourceUrl,
                                            string orderId,
                                            string pathway,
                                            int pageCount,
                                            string domainName,
                                            string dccDomainUser)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      DomainName = domainName;
      DccDomainUser = dccDomainUser;
    }

    private XmlNode AddNode(XmlNode parentNode, string sChildNodeName)
    {
      XmlNode childNode = parentNode.OwnerDocument.CreateElement(sChildNodeName);
      parentNode.AppendChild(childNode);
      return childNode;
    }

    private void AddAttribute(XmlNode node, string sAttributeName, string sAttributeValue)
    {
      XmlAttribute attribute = node.OwnerDocument.CreateAttribute(sAttributeName);
      node.Attributes.Append(attribute);
      attribute.Value = sAttributeValue;
    }

    public override string ToXML()
    {
      XmlDocument requestDoc = new XmlDocument();
      requestDoc.LoadXml("<request/>");
      
      XmlElement oRoot = requestDoc.DocumentElement;

      XmlElement oUserName = (XmlElement)AddNode(oRoot, "username");
      oUserName.InnerText = DccDomainUser;

      XmlElement oSort = (XmlElement)AddNode(oRoot, "domain");
      AddAttribute(oSort, "domainname", DomainName);
      AddAttribute(oSort, "shopperid", ShopperID);

      return requestDoc.InnerXml;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("DCCGetDomainContacts is not a cacheable request.");
    }
  }
}
