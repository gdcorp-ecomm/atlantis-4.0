using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetAutoRenew.Interface
{
  public class DCCSetAutoRenewRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(12);

    private bool IsManager { get; set; }
    private string ManagerUserId { get; set; }

    public DCCSetAutoRenewRequestData(string shopperId,
                                            int privateLabelID,
                                            int domainId,
                                            int autoRenewValue,
                                            string applicationName, bool isManager, string managerUserId)

    {
      ShopperID = shopperId;
      _privateLabelID = privateLabelID;
      _domainId = domainId;
      _autoRenewValue = autoRenewValue;
      RequestTimeout = _requestTimeout;
      AppName = applicationName;
      IsManager = isManager;
      ManagerUserId = managerUserId;
    }

    
    private int _domainId;
    public int DomainID
    {
      get { return _domainId; }
    }

    private int _autoRenewValue;
    public int AutoRenewValue
    {
      get { return _autoRenewValue; }
    }

    private int _privateLabelID;
    public int PrivateLabelID
    {
      get { return _privateLabelID; }
    }

    private string AppName { get; set;}

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

    public void XmlToVerify(out string actionXml, out string domainXML)
    {
      XmlDocument actionDoc = new XmlDocument();
      actionDoc.LoadXml("<ACTION/>");

      XmlElement oRoot = actionDoc.DocumentElement;
      AddAttribute(oRoot, "ActionName", "DomainSetAutoRenew");
      AddAttribute(oRoot, "ShopperId", ShopperID);
      AddAttribute(oRoot, "UserType", IsManager ? "Administrator" : "Shopper");
      AddAttribute(oRoot, "UserId", IsManager ? ManagerUserId : ShopperID);
      AddAttribute(oRoot, "PrivateLabelId", _privateLabelID.ToString());
      AddAttribute(oRoot, "RequestingApplication", AppName);

      XmlElement oAutoRenew = (XmlElement)AddNode(oRoot, "AUTORENEW");
      AddAttribute(oAutoRenew, "Status", _autoRenewValue.ToString());

      XmlDocument domainDoc = new XmlDocument();
      domainDoc.LoadXml("<DOMAINS/>");
      XmlElement oDomains = domainDoc.DocumentElement;
      XmlElement oDomain = (XmlElement)AddNode(oDomains, "DOMAIN");
      AddAttribute(oDomain, "id", _domainId.ToString());

      actionXml = actionDoc.InnerXml;
      domainXML = domainDoc.InnerXml;
    }

    public override string ToXML()
    {
      XmlDocument requestDoc = new XmlDocument();
      requestDoc.LoadXml("<REQUEST/>");

      XmlElement oRoot = requestDoc.DocumentElement;

      XmlElement oAction = (XmlElement)AddNode(oRoot, "ACTION");
      AddAttribute(oAction, "ActionName", "DomainSetAutoRenew");
      AddAttribute(oAction, "ShopperId", ShopperID);
      AddAttribute(oAction, "UserType", IsManager ? "Administrator" : "Shopper");
      AddAttribute(oAction, "UserId", IsManager ? ManagerUserId : ShopperID);
      AddAttribute(oAction, "PrivateLabelId", _privateLabelID.ToString());
      AddAttribute(oAction, "RequestingServer", Environment.MachineName);
      AddAttribute(oAction, "RequestingApplication", AppName);
      AddAttribute(oAction, "RequestedByIp", System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString());
      AddAttribute(oAction, "ModifiedBy", "1");

      XmlElement oAutoRenew = (XmlElement)AddNode(oAction, "AUTORENEW");
      AddAttribute(oAutoRenew, "Status", _autoRenewValue.ToString());

      XmlElement oResources = (XmlElement)AddNode(oRoot, "RESOURCES");
      AddAttribute(oResources, "ResourceType", "1");

      XmlElement oID = (XmlElement)AddNode(oResources, "ID");
      oID.InnerText = _domainId.ToString();

      return requestDoc.InnerXml;
    }


    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("DCCSetAutoRenew is not a cacheable request.");
    }

    #endregion
  }
}
