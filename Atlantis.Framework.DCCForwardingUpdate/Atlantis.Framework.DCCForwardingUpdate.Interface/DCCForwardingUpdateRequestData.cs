using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCForwardingUpdate.Interface
{
  public class DCCForwardingUpdateRequestData : RequestData
  {
    private readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(30);
    
    string AppName { get; set; }
    public int DomainId { get; private set; }
    public int PrivateLabelId { get; private set;}
    public string DomainName { get; set; }
    public string RedirectURL { get; set;}
    public RedirectType RedirectType { get; set; }
    public bool UpdateNameServers { get; set; }
    public bool HasMasking { get; set; }
    public string MaskingMetaTagTitle { get; set; }
    public string MaskingMetaTagDescription { get; set; }
    public string MaskingMetaTagKeyword { get; set; }
    public string MarketId { get; set; }

    public DCCForwardingUpdateRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int privateLabelId, int domainId, string applicationName,
      string redirectUrl, RedirectType redirectType, bool updateNameServers) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      DomainId = domainId;
      AppName = applicationName;
      RedirectURL = redirectUrl;
      RedirectType = redirectType;
      UpdateNameServers = updateNameServers;
      RequestTimeout = _requestTimeout;
      MarketId = "en-US";
    }

    private static XmlNode AddNode(XmlNode parentNode, string sChildNodeName)
    {
      XmlNode childNode = parentNode.OwnerDocument.CreateElement(sChildNodeName);
      parentNode.AppendChild(childNode);
      return childNode;
    }

    private static void AddAttribute(XmlNode node, string sAttributeName, string sAttributeValue)
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
      AddAttribute(oAction, "ActionName", "DomainForwardingUpdate");
      AddAttribute(oAction, "ShopperId", ShopperID);
      AddAttribute(oAction, "UserType", "Shopper");
      AddAttribute(oAction, "UserId", ShopperID);
      AddAttribute(oAction, "PrivateLabelId", PrivateLabelId.ToString());
      AddAttribute(oAction, "RequestingServer", Environment.MachineName);
      AddAttribute(oAction, "RequestingApplication", AppName);
      AddAttribute(oAction, "RequestedByIp", System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString());
      AddAttribute(oAction, "ModifiedBy", "1");

      var oForward = (XmlElement)AddNode(oAction, "FORWARDING");
      AddAttribute(oForward, "RedirectUrl", RedirectURL);
      AddAttribute(oForward, "RedirectType", ((int)RedirectType).ToString());

      AddAttribute(oForward, "HasMasking", HasMasking ? "1" : "0");
      AddAttribute(oForward, "MaskingMetatagTitle", MaskingMetaTagTitle);
      AddAttribute(oForward, "MaskingMetatagDescription", MaskingMetaTagDescription);
      AddAttribute(oForward, "MaskingMetatagKeyword", MaskingMetaTagDescription);
      AddAttribute(oForward, "UpdateNameservers", UpdateNameServers ? "1" : "0");

      var oResources = (XmlElement)AddNode(oRoot, "RESOURCES");
      AddAttribute(oResources, "ResourceType", "1");

      var oID = (XmlElement)AddNode(oResources, "ID");
      oID.InnerText = DomainId.ToString();

      return requestDoc.InnerXml;
    }

    public void XmlToVerify(out string actionXml, out string domainXML)
    {
      var actionDoc = new XmlDocument();
      actionDoc.LoadXml("<ACTION/>");

      XmlElement oRoot = actionDoc.DocumentElement;
      AddAttribute(oRoot, "ActionName", "DomainForwardingUpdate");
      AddAttribute(oRoot, "ShopperId", ShopperID);
      AddAttribute(oRoot, "UserType", "Shopper");
      AddAttribute(oRoot, "UserId", ShopperID);
      AddAttribute(oRoot, "PrivateLabelId", PrivateLabelId.ToString());
      AddAttribute(oRoot, "RequestingApplication", AppName);
      AddAttribute(oRoot, "MarketId", MarketId);

      var oForward = (XmlElement)AddNode(oRoot, "FORWARDING");
      AddAttribute(oForward, "RedirectUrl", RedirectURL);
      AddAttribute(oForward, "RedirectType", ((int)RedirectType).ToString());

      AddAttribute(oForward, "HasMasking", HasMasking ? "1" : "0");
      AddAttribute(oForward, "MaskingMetatagTitle", MaskingMetaTagTitle);
      AddAttribute(oForward, "MaskingMetatagDescription", MaskingMetaTagDescription);
      AddAttribute(oForward, "MaskingMetatagKeyword", MaskingMetaTagDescription);
      AddAttribute(oForward, "UpdateNameservers", UpdateNameServers ? "1" : "0");

      var domainDoc = new XmlDocument();
      domainDoc.LoadXml("<DOMAINS/>");
      XmlElement oDomains = domainDoc.DocumentElement;
      var oDomain = (XmlElement)AddNode(oDomains, "DOMAIN");
      AddAttribute(oDomain, "id", DomainId.ToString());

      actionXml = actionDoc.InnerXml;
      domainXML = domainDoc.InnerXml;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("DCCForwardingUpdate is not a cacheable request.");
    }
  }

  public enum RedirectType
  {
    Permanent = 301,
    Temporary = 302
  }
}
