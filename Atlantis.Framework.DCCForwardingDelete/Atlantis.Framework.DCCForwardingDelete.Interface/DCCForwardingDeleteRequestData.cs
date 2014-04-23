using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCForwardingDelete.Interface
{
  public class DCCForwardingDeleteRequestData : RequestData
  {
    private readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public DCCForwardingDeleteRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int privateLabelID, int domainId, string applicationName)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PrivateLabelID = privateLabelID;
      DomainID = domainId;
      AppName = applicationName;
      RequestTimeout = _requestTimeout;
      MarketId = "en-US";
    }

    private string AppName { get; set; }
    public int DomainID { get; private set; }
    public int PrivateLabelID { get; private set; }
    public string MarketId { get; set; }
   
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

    public string XmlToSubmit()
    {
      var requestDoc = new XmlDocument();
      requestDoc.LoadXml("<REQUEST/>");

      XmlElement oRoot = requestDoc.DocumentElement;

      var oAction = (XmlElement)AddNode(oRoot, "ACTION");
      AddAttribute(oAction, "ActionName", "DomainForwardingDelete");
      AddAttribute(oAction, "ShopperId", ShopperID);
      AddAttribute(oAction, "UserType", "Shopper");
      AddAttribute(oAction, "UserId", ShopperID);
      AddAttribute(oAction, "PrivateLabelId", PrivateLabelID.ToString());
      AddAttribute(oAction, "RequestingApplication", AppName);
      AddAttribute(oAction, "RequestingServer", Environment.MachineName);
      AddAttribute(oAction, "RequestedByIp", System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString());
      AddAttribute(oAction, "ModifiedBy", "1");

      var oResources = (XmlElement)AddNode(oRoot, "RESOURCES");
      AddAttribute(oResources, "ResourceType", "1");

      var oID = (XmlElement)AddNode(oResources, "ID");
      oID.InnerText = DomainID.ToString();

      return requestDoc.InnerXml;
    }
    
    public void XmlToVerify(out string actionXml, out string domainXML)
    {
      var actionDoc = new XmlDocument();
      actionDoc.LoadXml("<ACTION/>");

      XmlElement oRoot = actionDoc.DocumentElement;
      AddAttribute(oRoot, "ActionName", "DomainForwardingDelete");
      AddAttribute(oRoot, "ShopperId", ShopperID);
      AddAttribute(oRoot, "UserType", "Shopper");
      AddAttribute(oRoot, "UserId", ShopperID);
      AddAttribute(oRoot, "PrivateLabelId", PrivateLabelID.ToString());
      AddAttribute(oRoot, "RequestingApplication", AppName);
      AddAttribute(oRoot, "MarketId", MarketId);

      var domainDoc = new XmlDocument();
      domainDoc.LoadXml("<DOMAINS/>");
      XmlElement oDomains = domainDoc.DocumentElement;
      var oDomain = (XmlElement)AddNode(oDomains, "DOMAIN");
      AddAttribute(oDomain, "id", DomainID.ToString());

      actionXml = actionDoc.InnerXml;
      domainXML = domainDoc.InnerXml;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("DCCSetLocking is not a cacheable request.");
    }
  }
}
