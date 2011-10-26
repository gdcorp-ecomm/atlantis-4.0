using System;
using System.Xml;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetNameservers.Interface
{
  public class DCCSetNameserversRequestData : RequestData
  {
    public enum NameserverType
    {
      Park,
      Forward,
      Host,
      Custom
    };
    
    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);
    
    internal IDictionary<string, string> CustomNameservers { get; set; }

    internal IList<string> PremiumNameservers { get; set; }

    public NameserverType RequestType { get; private set; }

    public int DomainID { get; private set; }

    public int PrivateLabelID { get; private set; }

    public string DccDomainUser { get; private set; }

    public bool IsPremium
    {
      get { return PremiumNameservers != null && PremiumNameservers.Count > 0; }
    }
    
    public DCCSetNameserversRequestData(string shopperId,
                                        string sourceUrl,
                                        string orderId,
                                        string pathway,
                                        int pageCount,
                                        NameserverType requestType,
                                        int privateLabelID,
                                        int domainID,
                                        string dccDomainUser) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PrivateLabelID = privateLabelID;
      DomainID = domainID;
      RequestType = requestType;
      CustomNameservers = new Dictionary<string, string>(16);
      PremiumNameservers = new List<string>(16);
      DccDomainUser = dccDomainUser;
      RequestTimeout = _requestTimeout;
    }

    public void AddPremiumNameserver(string premiumNameserver)
    {
      PremiumNameservers.Add(premiumNameserver);
    }

    public void AddCustomNameserver(string customNameserver)
    {
      // Remove from dictionary and take new value
      if (CustomNameservers.ContainsKey(customNameserver))
      {
        CustomNameservers.Remove(customNameserver);
      }

      CustomNameservers.Add(customNameserver, string.Empty);
    }

    public void AddCustomNameserverWithIP(string customNameserver, string ip)
    {   
      // Remove from dictionary and take new value
      if(CustomNameservers.ContainsKey(customNameserver))
      {
        CustomNameservers.Remove(customNameserver);
      }

      if (string.IsNullOrEmpty(ip))
      {
        CustomNameservers.Add(customNameserver, string.Empty);
      }
      else
      {
        CustomNameservers.Add(customNameserver, ip);
      }
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

    private void AddNameservers(XmlElement oNameservers)
    {
      foreach (string nameserver in CustomNameservers.Keys)
      {
        XmlElement oNameserver = (XmlElement)AddNode(oNameservers, "NAMESERVER");
        AddAttribute(oNameserver, "Name", nameserver);
        AddAttribute(oNameserver, "NameServerIP", CustomNameservers[nameserver]);
      }
    }

    public void XmlToVerify(out string actionXml, out string domainXML)
    {
      XmlDocument actionDoc = new XmlDocument();
      actionDoc.LoadXml("<ACTION/>");

      XmlElement oRoot = actionDoc.DocumentElement;
      AddAttribute(oRoot, "ActionName", "NameServerUpdate");
      AddAttribute(oRoot, "ShopperId", ShopperID);
      AddAttribute(oRoot, "PrivateLabelId", PrivateLabelID.ToString());

      XmlElement oNameservers = (XmlElement)AddNode(oRoot, "NAMESERVERS");
      AddAttribute(oNameservers, "NameServerType", NameserverTypeToString(RequestType));
      if (CustomNameservers.Count >= 2 && CustomNameservers.Count <= 13)
      {
        AddNameservers(oNameservers);
      }

      XmlDocument domainDoc = new XmlDocument();
      domainDoc.LoadXml("<DOMAINS/>");
      XmlElement oDomains = domainDoc.DocumentElement;
      XmlElement oDomain = (XmlElement)AddNode(oDomains, "DOMAIN");
      AddAttribute(oDomain, "id", DomainID.ToString());

      actionXml = actionDoc.InnerXml;
      domainXML = domainDoc.InnerXml;
    }

    public override string ToXML()
    {
      XmlDocument requestDoc = new XmlDocument();
      requestDoc.LoadXml("<REQUEST/>");

      XmlElement oRoot = requestDoc.DocumentElement;

      XmlElement oAction = (XmlElement)AddNode(oRoot, "ACTION");
      AddAttribute(oAction, "ActionName", "NameServerUpdate");
      AddAttribute(oAction, "ShopperId", ShopperID);
      AddAttribute(oAction, "UserType", "Shopper");
      AddAttribute(oAction, "UserId", ShopperID);
      AddAttribute(oAction, "PrivateLabelId", PrivateLabelID.ToString());
      AddAttribute(oAction, "RequestingServer", Environment.MachineName);
      AddAttribute(oAction, "RequestedByIp", System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString());
      AddAttribute(oAction, "ModifiedBy", "1");

      XmlElement oNameservers = (XmlElement)AddNode(oAction, "NAMESERVERS");
      AddAttribute(oNameservers, "NameServerType", NameserverTypeToString(RequestType));

      if (CustomNameservers.Count >= 2 && CustomNameservers.Count <= 13)
      {
        AddNameservers(oNameservers);
      }

      XmlElement oResources = (XmlElement)AddNode(oRoot, "RESOURCES");
      AddAttribute(oResources, "ResourceType", "1");

      XmlElement oID = (XmlElement)AddNode(oResources, "ID");
      oID.InnerText = DomainID.ToString();

      return requestDoc.InnerXml;
    }

    private static string NameserverTypeToString(NameserverType inNameserverType)
    {
      var returnString = "";
      switch(inNameserverType)
      {
        case NameserverType.Custom:
          returnString = "Custom";
          break;
        case NameserverType.Forward:
          returnString = "Forwarded";
          break;
        case NameserverType.Host:
          returnString = "Hosted";
          break;
        case NameserverType.Park:
          returnString = "Parked";
          break;
      }
      return returnString;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("DCCSetNameserversRequestData is not a cacheable request.");
    }
  }
}
