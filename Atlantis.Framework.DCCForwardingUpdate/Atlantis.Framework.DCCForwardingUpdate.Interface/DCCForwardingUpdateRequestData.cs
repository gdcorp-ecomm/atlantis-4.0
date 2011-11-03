using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCForwardingUpdate.Interface
{
  public class DCCForwardingUpdateRequestData : RequestData
  {
    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(30);
    
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

    public DCCForwardingUpdateRequestData(string shopperId,
                                    string sourceUrl,
                                    string orderId,
                                    string pathway,
                                    int pageCount,
                                    int privateLabelId,
                                    int domainId,
                                    string applicationName,
                                    string redirectUrl,
                                    RedirectType redirectType,
                                    bool updateNameServers)

      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      DomainId = domainId;
      AppName = applicationName;
      RedirectURL = redirectUrl;
      RedirectType = redirectType;
      UpdateNameServers = updateNameServers;
      RequestTimeout = _requestTimeout;
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

    /*
<REQUEST>
 <ACTION ActionName="ContactUpdate" ShopperId="111111" UserType="Shopper" UserId="111111" PrivateLabelId="1" RequestingServer="SERVERNAME" RequestedByIp="1.2.3.4" ModifiedBy="1" >
  <FORWARDING RedirectUrl="www.godaddy.com" RedirectType="302" HasMasking="1" MaskingMetatagTitle="title" MaskingMetatagDescription="description" MaskingMetatagKeyword="keyword" />
 </ACTION>
 <RESOURCES ResourceType="1">
   <ID>12345</ID>
 </RESOURCES>
</REQUEST>
   */
    public string XmlToSubmit()
    {
      XmlDocument requestDoc = new XmlDocument();
      requestDoc.LoadXml("<REQUEST/>");

      XmlElement oRoot = requestDoc.DocumentElement;

      XmlElement oAction = (XmlElement)AddNode(oRoot, "ACTION");
      AddAttribute(oAction, "ActionName", "DomainForwardingUpdate");
      AddAttribute(oAction, "ShopperId", ShopperID);
      AddAttribute(oAction, "UserType", "Shopper");
      AddAttribute(oAction, "UserId", ShopperID);
      AddAttribute(oAction, "PrivateLabelId", PrivateLabelId.ToString());
      AddAttribute(oAction, "RequestingServer", Environment.MachineName);
      AddAttribute(oAction, "RequestingApplication", AppName);
      AddAttribute(oAction, "RequestedByIp", System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString());
      AddAttribute(oAction, "ModifiedBy", "1");

      XmlElement oForward = (XmlElement)AddNode(oAction, "FORWARDING");
      AddAttribute(oForward, "RedirectUrl", RedirectURL);
      AddAttribute(oForward, "RedirectType", ((int)RedirectType).ToString());

      AddAttribute(oForward, "HasMasking", HasMasking ? "1" : "0");
      AddAttribute(oForward, "MaskingMetatagTitle", MaskingMetaTagTitle);
      AddAttribute(oForward, "MaskingMetatagDescription", MaskingMetaTagDescription);
      AddAttribute(oForward, "MaskingMetatagKeyword", MaskingMetaTagDescription);
      AddAttribute(oForward, "UpdateNameservers", UpdateNameServers ? "1" : "0");

      XmlElement oResources = (XmlElement)AddNode(oRoot, "RESOURCES");
      AddAttribute(oResources, "ResourceType", "1");

      XmlElement oID = (XmlElement)AddNode(oResources, "ID");
      oID.InnerText = DomainId.ToString();

      return requestDoc.InnerXml;
    }

    /*
<ACTION ActionName="DomainForwardingUpdate" ShopperId="111111" PrivateLabelId="1" >
<FORWARDING RedirectUrl="www.godaddy.com" RedirectType="302" HasMasking="1" MaskingMetatagTitle="title" MaskingMetatagDescription="description" MaskingMetatagKeyword="keyword" />
</ACTION>
*/

    public void XmlToVerify(out string actionXml, out string domainXML)
    {
      XmlDocument actionDoc = new XmlDocument();
      actionDoc.LoadXml("<ACTION/>");

      XmlElement oRoot = actionDoc.DocumentElement;
      AddAttribute(oRoot, "ActionName", "DomainForwardingUpdate");
      AddAttribute(oRoot, "ShopperId", ShopperID);
      AddAttribute(oRoot, "UserType", "Shopper");
      AddAttribute(oRoot, "UserId", ShopperID);
      AddAttribute(oRoot, "PrivateLabelId", PrivateLabelId.ToString());
      AddAttribute(oRoot, "RequestingApplication", AppName);

      XmlElement oForward = (XmlElement)AddNode(oRoot, "FORWARDING");
      AddAttribute(oForward, "RedirectUrl", RedirectURL);
      AddAttribute(oForward, "RedirectType", ((int)RedirectType).ToString());

      AddAttribute(oForward, "HasMasking", HasMasking ? "1" : "0");
      AddAttribute(oForward, "MaskingMetatagTitle", MaskingMetaTagTitle);
      AddAttribute(oForward, "MaskingMetatagDescription", MaskingMetaTagDescription);
      AddAttribute(oForward, "MaskingMetatagKeyword", MaskingMetaTagDescription);
      AddAttribute(oForward, "UpdateNameservers", UpdateNameServers ? "1" : "0");

      XmlDocument domainDoc = new XmlDocument();
      domainDoc.LoadXml("<DOMAINS/>");
      XmlElement oDomains = domainDoc.DocumentElement;
      XmlElement oDomain = (XmlElement)AddNode(oDomains, "DOMAIN");
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
