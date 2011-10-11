using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetLocking.Interface
{
  public class DCCSetLockingRequestData: RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(12);

    private string _dccDomainUser;

    public DCCSetLockingRequestData(string shopperId,
                                    string sourceUrl,
                                    string orderId,
                                    string pathway,
                                    int pageCount,
                                    int privateLabelID,
                                    int domainId,
                                    int lockingValue,
                                    string dccDomainUser)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _privateLabelID = privateLabelID;
      _domainId = domainId;
      _lockingValue = lockingValue;
      _dccDomainUser = dccDomainUser;
      RequestTimeout = _requestTimeout;
    }
    
    private int _domainId;
    public int DomainID
    {
      get { return _domainId; }
    }

    private int _lockingValue;
    public int LockingValue
    {
      get { return _lockingValue; }
    }

    private int _privateLabelID;
    public int PrivateLabelID
    {
      get { return _privateLabelID; }
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

    public void XmlToVerify(out string actionXml, out string domainXML)
    {
        XmlDocument actionDoc = new XmlDocument();
        actionDoc.LoadXml("<ACTION/>");

        XmlElement oRoot = actionDoc.DocumentElement;
        AddAttribute(oRoot, "ActionName", "DomainSetLock");
        AddAttribute(oRoot, "ShopperId", ShopperID);
        AddAttribute(oRoot, "UserType", "Shopper");
        AddAttribute(oRoot, "UserId", ShopperID);
        AddAttribute(oRoot, "PrivateLabelId", _privateLabelID.ToString());

        XmlElement oLock = (XmlElement)AddNode(oRoot, "LOCK");
        AddAttribute(oLock, "Status", _lockingValue.ToString() );

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
      AddAttribute(oAction, "ActionName", "DomainSetLock");
      AddAttribute(oAction, "ShopperId", ShopperID);
      AddAttribute(oAction, "UserType", "Shopper");
      AddAttribute(oAction, "UserId", ShopperID);
      AddAttribute(oAction, "PrivateLabelId", _privateLabelID.ToString());
      AddAttribute(oAction, "RequestingServer", Environment.MachineName);
      AddAttribute(oAction, "RequestedByIp", System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString());
      AddAttribute(oAction, "ModifiedBy", "1");

      XmlElement oLock = (XmlElement)AddNode(oAction, "LOCK");
      AddAttribute(oLock, "Status", _lockingValue.ToString());

      XmlElement oResources = (XmlElement)AddNode(oRoot, "RESOURCES");
      AddAttribute(oResources, "ResourceType", "1");

      XmlElement oID = (XmlElement)AddNode(oResources, "ID");
      oID.InnerText = _domainId.ToString();

      return requestDoc.InnerXml;
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("DCCSetLocking is not a cacheable request.");
    }

    #endregion
  }
}
