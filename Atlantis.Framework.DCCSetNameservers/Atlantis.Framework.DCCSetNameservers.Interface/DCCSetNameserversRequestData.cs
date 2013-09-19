using System;
using System.Globalization;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Linq;
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


    private IList<string> _premiumNameservers; 
    internal IList<string> PremiumNameservers {
      get
      {
        if (_premiumNameservers == null)
        {
          _premiumNameservers = new List<string>(0);
        }
        return _premiumNameservers;
      }
      set { _premiumNameservers = value; }
    }

    public NameserverType RequestType { get; private set; }

    public int DomainId { get; private set; }

    public int TldId { get; set; }

    public int PrivateLabelId { get; private set; }

    public string AppName { get; private set; }

    public string RegistrarId { get; set; }


    public bool IsPremium
    {
      get { return PremiumNameservers.Count > 0; }
    }

    public DCCSetNameserversRequestData(string shopperId,
                                        string sourceUrl,
                                        string orderId,
                                        string pathway,
                                        int pageCount,
                                        NameserverType requestType,
                                        int privateLabelId,
                                        int domainId,
                                        string applicationName) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      DomainId = domainId;
      RequestType = requestType;
      CustomNameservers = new Dictionary<string, string>(16);
      PremiumNameservers = new List<string>(16);
      AppName = applicationName;
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

    public void AddCustomNameserverWithIp(string customNameserver, string ip)
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
      XmlNode resultNode = null;

      if (parentNode.OwnerDocument != null)
      {
        resultNode = parentNode.OwnerDocument.CreateElement(sChildNodeName);
        parentNode.AppendChild(resultNode);
      }

      return resultNode;
    }

    private static void AddAttribute(XmlNode node, string sAttributeName, string sAttributeValue)
    {
      if (node.OwnerDocument != null)
      {
        var attribute = node.OwnerDocument.CreateAttribute(sAttributeName);
        if (node.Attributes != null)
        {
          node.Attributes.Append(attribute);
        }

        attribute.Value = sAttributeValue;
      }
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
    
    public void XmlToVerify(out string actionXml, out string domainXml)
    {
      XmlDocument actionDoc = new XmlDocument();
      actionDoc.LoadXml("<ACTION/>");

      XmlElement oRoot = actionDoc.DocumentElement;
      AddAttribute(oRoot, "ActionName", "NameServerUpdate");
      AddAttribute(oRoot, "ShopperId", ShopperID);
      AddAttribute(oRoot, "PrivateLabelId", PrivateLabelId.ToString(CultureInfo.InvariantCulture));
      AddAttribute(oRoot, "RequestingApplication", AppName);

      XmlElement oNameservers = (XmlElement)AddNode(oRoot, "NAMESERVERS");
      AddAttribute(oNameservers, "NameServerType", NameserverTypeToString(RequestType));
      if (CustomNameservers.Count > 1 && CustomNameservers.Count < 14)
      {
        AddNameservers(oNameservers);
      }

      XmlDocument domainDoc = new XmlDocument();
      domainDoc.LoadXml("<DOMAINS/>");
      XmlElement oDomains = domainDoc.DocumentElement;
      var oDomain = (XmlElement)AddNode(oDomains, "DOMAIN");
      AddAttribute(oDomain, "id", DomainId.ToString(CultureInfo.InvariantCulture));

      actionXml = actionDoc.InnerXml;
      domainXml = domainDoc.InnerXml;
    }
    
    IList<XElement> GetNameServerXml()
    {
      var nameservers = new List<XElement>();

      foreach (var customNameserver in CustomNameservers.Keys)
      {
        var nameserver = new XElement("NameServer",
                                      new XAttribute("HostName", customNameserver));

        if (!string.IsNullOrEmpty(CustomNameservers[customNameserver]))
        {
          nameserver.Add(new XElement("IPAddresses",
                                      new XElement("IPAddress",
                                                   new XAttribute("IP", CustomNameservers[customNameserver]))));
        }

        nameservers.Add(nameserver);
      }

      return nameservers;
    }

    internal string GetDomainNameserverValidateRequestXml()
    {
      var requestXml = new XDocument(
        new XElement("REQUEST",
                     new XAttribute("Name", "ValidateDomainNameservers"),
                     new XAttribute("RegistrarId", RegistrarId ?? string.Empty),
                     new XAttribute("ShopperId", ShopperID),
                     new XAttribute("ParentShopperId", ShopperID),
                     new XAttribute("PrivateLabelId", PrivateLabelId),
                     new XAttribute("RequestingApplication", AppName),
                     new XElement("Items",
                                  new XElement("Item",
                                               new XAttribute("ObjectId", DomainId),
                                               new XAttribute("Type", NameserverTypeToString(RequestType)),
                                               new XElement("TldIds", TldId),
                                               new XElement("NameServers",
                                                            GetNameServerXml())))));

      return requestXml.ToString(SaveOptions.DisableFormatting);
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
      AddAttribute(oAction, "PrivateLabelId", PrivateLabelId.ToString(CultureInfo.InvariantCulture));
      AddAttribute(oAction, "RequestingServer", Environment.MachineName);
      AddAttribute(oAction, "RequestingApplication", AppName);
      AddAttribute(oAction, "RequestedByIp", System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString());
      AddAttribute(oAction, "ModifiedBy", "1");

      XmlElement oNameservers = (XmlElement)AddNode(oAction, "NAMESERVERS");
      AddAttribute(oNameservers, "NameServerType", NameserverTypeToString(RequestType));
      
      if (CustomNameservers.Count > 1 && CustomNameservers.Count < 14)
      {
        AddNameservers(oNameservers);
      }

      XmlElement oResources = (XmlElement)AddNode(oRoot, "RESOURCES");
      AddAttribute(oResources, "ResourceType", "1");

      var id = (XmlElement)AddNode(oResources, "ID");
      id.InnerText = DomainId.ToString(CultureInfo.InvariantCulture);

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
  }
}
