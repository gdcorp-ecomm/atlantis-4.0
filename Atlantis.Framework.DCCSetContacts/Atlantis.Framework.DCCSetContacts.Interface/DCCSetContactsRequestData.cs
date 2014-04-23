using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetContacts.Interface
{
  public class DCCSetContactsRequestData : RequestData
  {
    private const int DomainRegistrationAgreement = 46;
    private const int UniversalTermsOfService = 1;
    private const int DomainNameChangeRegistrantAgreement = 26;
    private const int ActOnBehalfAgreement = 47;

    private readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(12);
    readonly Dictionary<int, Dictionary<string, string>> _contacts = new Dictionary<int, Dictionary<string, string>>();
    //(registrant = 0, technical = 1, admin = 2, billing = 3)
    
    public DCCSetContactsRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int privateLabelID, int domainId, string applicationName)
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
    private bool _domainRegAgreement;
    public bool SetDomainRegAgreement
    {
      set { _domainRegAgreement = value; }
    }

    private bool _utos;
    public bool SetUTOSAgreement
    {
      set { _utos = value; }
    }

    private bool _domainNameChange;
    public bool SetDomainNameChangeAgreement
    {
      set { _domainNameChange = value; }
    }

    private bool _actOnBehalf;
    public bool SetActOnBehalfAgreemnt
    {
      set { _actOnBehalf = value; }
    }

    public bool addContact(int iType, Dictionary<string, string> oContact)
    {
      bool isValid = false;
      if (iType >= 0 && iType <= 3 && !_contacts.ContainsKey(iType))
      {
        _contacts.Add(iType, oContact);
        isValid = true;
      }
      return isValid;
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

    public string XmlToSubmit()
    {
      var requestDoc = new XmlDocument();
      requestDoc.LoadXml("<REQUEST/>");

      XmlElement oRoot = requestDoc.DocumentElement;

      var oAction = (XmlElement)AddNode(oRoot, "ACTION");
      AddAttribute(oAction, "ActionName", "ContactUpdate");
      AddAttribute(oAction, "ShopperId", ShopperID);
      AddAttribute(oAction, "UserType", "Shopper");
      AddAttribute(oAction, "UserId", ShopperID);
      AddAttribute(oAction, "PrivateLabelId", PrivateLabelID.ToString());
      AddAttribute(oAction, "RequestingServer", Environment.MachineName);
      AddAttribute(oAction, "RequestingApplication", AppName);
      AddAttribute(oAction, "RequestedByIp", System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString());
      AddAttribute(oAction, "ModifiedBy", "1");

      var oContacts = (XmlElement)AddNode(oAction, "CONTACTS");
      BuildContactXml(oContacts);

      var oAgreements = (XmlElement)AddNode(oAction, "AGREEMENTS");
      BuildAgreementsXml(oAgreements);

      var oResources = (XmlElement)AddNode(oRoot, "RESOURCES");
      AddAttribute(oResources, "ResourceType", "1");

      var oID = (XmlElement)AddNode(oResources, "ID");
      oID.InnerText = DomainID.ToString();

      return requestDoc.InnerXml;
    }


    public void XmlToVerify(out string actionXml, out string domainXml)
    {
      if (_contacts.Count == 0)
      {
        actionXml = "";
        domainXml = "";
      }
      else
      {
        var actionDoc = new XmlDocument();
        actionDoc.LoadXml("<ACTION/>");

        XmlElement oRoot = actionDoc.DocumentElement;
        AddAttribute(oRoot, "ActionName", "ContactUpdate");
        AddAttribute(oRoot, "ShopperId", ShopperID);
        AddAttribute(oRoot, "UserType", "Shopper");
        AddAttribute(oRoot, "UserId", ShopperID);
        AddAttribute(oRoot, "PrivateLabelId", PrivateLabelID.ToString());
        AddAttribute(oRoot, "RequestingApplication", AppName);
        AddAttribute(oRoot, "MarketId", MarketId);

        var oContacts = (XmlElement)AddNode(oRoot, "Contacts");
        BuildContactXml(oContacts);

        var domainDoc = new XmlDocument();
        domainDoc.LoadXml("<DOMAINS/>");
        XmlElement oDomains = domainDoc.DocumentElement;
        var oDomain = (XmlElement)AddNode(oDomains, "DOMAIN");
        AddAttribute(oDomain, "id", DomainID.ToString());

        actionXml = actionDoc.InnerXml;
        domainXml = domainDoc.InnerXml;
      }
    }

    private void BuildAgreementsXml(XmlElement oAgreements)
    {
      if (_domainRegAgreement)
      {
        var oAgreement = (XmlElement)AddNode(oAgreements, "Agreement");
        AddAttribute(oAgreement, "ID", DomainRegistrationAgreement.ToString());
        AddAttribute(oAgreement, "NotePrefix", "ContactsUpdate");
      }

      if (_utos)
      {
        var oAgreement = (XmlElement)AddNode(oAgreements, "Agreement");
        AddAttribute(oAgreement, "ID", UniversalTermsOfService.ToString());
        AddAttribute(oAgreement, "NotePrefix", "ContactsUpdate");
      }

      if (_domainNameChange)
      {
        var oAgreement = (XmlElement)AddNode(oAgreements, "Agreement");
        AddAttribute(oAgreement, "ID", DomainNameChangeRegistrantAgreement.ToString());
        AddAttribute(oAgreement, "NotePrefix", "ContactsUpdate");
      }

      if (_actOnBehalf)
      {
        var oAgreement = (XmlElement)AddNode(oAgreements, "Agreement");
        AddAttribute(oAgreement, "ID", ActOnBehalfAgreement.ToString());
        AddAttribute(oAgreement, "NotePrefix", "ContactsUpdate");
      }
    }

    private void BuildContactXml(XmlElement oContacts)
    {
      //(registrant = 0, technical = 1, admin = 2, billing = 3)      
      foreach (var key in _contacts.Keys)
      {
        var oContactXml = (XmlElement)AddNode(oContacts, "CONTACT");
        Dictionary<string, string> oContact = _contacts[key];
        AddAttribute(oContactXml, "Type", oContact["Type"]);
        AddAttribute(oContactXml, "FirstName", oContact["FirstName"]);
        AddAttribute(oContactXml, "LastName", oContact["LastName"]);
        AddAttribute(oContactXml, "Company", oContact["Company"]);
        AddAttribute(oContactXml, "Address1", oContact["Address1"]);
        AddAttribute(oContactXml, "Address2", oContact["Address2"]);
        AddAttribute(oContactXml, "City", oContact["City"]);
        AddAttribute(oContactXml, "State", oContact["State"]);
        AddAttribute(oContactXml, "Zip", oContact["Zip"]);
        AddAttribute(oContactXml, "Country", oContact["Country"]);
        AddAttribute(oContactXml, "Phone", oContact["Phone"]);
        AddAttribute(oContactXml, "Fax", oContact["Fax"]);
        AddAttribute(oContactXml, "Email", oContact["Email"]);
      }
    }

    public override string GetCacheMD5()
    {
      throw new Exception("DCCSetContacts is not a cacheable request.");
    }
  }
}
