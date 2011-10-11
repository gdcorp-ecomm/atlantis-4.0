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

    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(12);

    private bool _domainRegAgreement = false;
    private bool _UTOS = false;
    private bool _domainNameChange = false;
    private bool _actOnBehalf = false;

    private int _privateLabelID;
    private int _domainId;
    private string _dccDomainUser;
    Dictionary<int, Dictionary<string, string>> _contacts = new Dictionary<int, Dictionary<string, string>>();
    //(registrant = 0, technical = 1, admin = 2, billing = 3)


    public DCCSetContactsRequestData(string shopperId,
                                    string sourceUrl,
                                    string orderId,
                                    string pathway,
                                    int pageCount,
                                    int privateLabelID,
                                    int domainId,
                                    string dccDomainUser)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _privateLabelID = privateLabelID;
      _domainId = domainId;
      _dccDomainUser = dccDomainUser;
      RequestTimeout = _requestTimeout;
    }

    public int DomainID
    {
      get { return _domainId; }
    }

    public int PrivateLabelID
    {
      get { return _privateLabelID; }
    }

    public bool SetDomainRegAgreement
    {
      set { _domainRegAgreement = value; }
    }

    public bool SetUTOSAgreement
    {
      set { _UTOS = value; }
    }

    public bool SetDomainNameChangeAgreement
    {
      set { _domainNameChange = value; }
    }

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

    /*
<REQUEST>
 <ACTION ActionName="ContactUpdate" ShopperId="111111" UserType="Shopper" UserId="111111" PrivateLabelId="1" RequestingServer="SERVERNAME" RequestedByIp="1.2.3.4" ModifiedBy="1" >
   <CONTACTS>
     <CONTACT Type="0" FirstName="John" LastName="Doe" Company="John Doe, Inc" Address1="123 Some Street" Address2="" City="Anytown" State="Iowa" Zip="52402" Country="United States" Phone="319-555-1234" Fax="319-555-5678" Email="john@doe.com" />
     <CONTACT Type="1" FirstName="John" LastName="Doe" Company="John Doe, Inc" Address1="123 Some Street" Address2="" City="Anytown" State="Iowa" Zip="52402" Country="United States" Phone="319-555-1234" Fax="319-555-5678" Email="john@doe.com" />
     <CONTACT Type="2" FirstName="John" LastName="Doe" Company="John Doe, Inc" Address1="123 Some Street" Address2="" City="Anytown" State="Iowa" Zip="52402" Country="United States" Phone="319-555-1234" Fax="319-555-5678" Email="john@doe.com" />
     <CONTACT Type="3" FirstName="John" LastName="Doe" Company="John Doe, Inc" Address1="123 Some Street" Address2="" City="Anytown" State="Iowa" Zip="52402" Country="United States" Phone="319-555-1234" Fax="319-555-5678" Email="john@doe.com" />
   </CONTACTS>
   <AGREEMENTS>
     <AGREEMENT ID="26" NotePrefix="ContactsUpdate" />
    <AGREEMENT ID="1" NotePrefix="ContactsUpdate" />
    <AGREEMENT ID="46" NotePrefix="ContactsUpdate" />
    <AGREEMENT ID="47" NotePrefix="ContactsUpdate" />
  </AGREEMENTS>
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
      AddAttribute(oAction, "ActionName", "ContactUpdate");
      AddAttribute(oAction, "ShopperId", ShopperID);
      AddAttribute(oAction, "UserType", "Shopper");
      AddAttribute(oAction, "UserId", ShopperID);
      AddAttribute(oAction, "PrivateLabelId", _privateLabelID.ToString());
      AddAttribute(oAction, "RequestingServer", Environment.MachineName);
      AddAttribute(oAction, "RequestedByIp", System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString());
      AddAttribute(oAction, "ModifiedBy", "1");

      XmlElement oContacts = (XmlElement)AddNode(oAction, "CONTACTS");
      BuildContactXml(oContacts);

      XmlElement oAgreements = (XmlElement)AddNode(oAction, "AGREEMENTS");
      BuildAgreementsXml(oAgreements);

      XmlElement oResources = (XmlElement)AddNode(oRoot, "RESOURCES");
      AddAttribute(oResources, "ResourceType", "1");

      XmlElement oID = (XmlElement)AddNode(oResources, "ID");
      oID.InnerText = _domainId.ToString();

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
        XmlDocument actionDoc = new XmlDocument();
        actionDoc.LoadXml("<ACTION/>");

        XmlElement oRoot = actionDoc.DocumentElement;
        AddAttribute(oRoot, "ActionName", "ContactUpdate");
        AddAttribute(oRoot, "ShopperId", ShopperID);
        AddAttribute(oRoot, "UserType", "Shopper");
        AddAttribute(oRoot, "UserId", ShopperID);
        AddAttribute(oRoot, "PrivateLabelId", _privateLabelID.ToString());

        XmlElement oContacts = (XmlElement)AddNode(oRoot, "Contacts");
        BuildContactXml(oContacts);

        XmlDocument domainDoc = new XmlDocument();
        domainDoc.LoadXml("<DOMAINS/>");
        XmlElement oDomains = domainDoc.DocumentElement;
        XmlElement oDomain = (XmlElement)AddNode(oDomains, "DOMAIN");
        AddAttribute(oDomain, "id", _domainId.ToString());

        actionXml = actionDoc.InnerXml;
        domainXml = domainDoc.InnerXml;
      }
    }

    private void BuildAgreementsXml(XmlElement oAgreements)
    {
      if (_domainRegAgreement)
      {
        XmlElement oAgreement = (XmlElement)AddNode(oAgreements, "Agreement");
        AddAttribute(oAgreement, "ID", DomainRegistrationAgreement.ToString());
        AddAttribute(oAgreement, "NotePrefix", "ContactsUpdate");
      }

      if (_UTOS)
      {
        XmlElement oAgreement = (XmlElement)AddNode(oAgreements, "Agreement");
        AddAttribute(oAgreement, "ID", UniversalTermsOfService.ToString());
        AddAttribute(oAgreement, "NotePrefix", "ContactsUpdate");
      }

      if (_domainNameChange)
      {
        XmlElement oAgreement = (XmlElement)AddNode(oAgreements, "Agreement");
        AddAttribute(oAgreement, "ID", DomainNameChangeRegistrantAgreement.ToString());
        AddAttribute(oAgreement, "NotePrefix", "ContactsUpdate");
      }

      if (_actOnBehalf)
      {
        XmlElement oAgreement = (XmlElement)AddNode(oAgreements, "Agreement");
        AddAttribute(oAgreement, "ID", ActOnBehalfAgreement.ToString());
        AddAttribute(oAgreement, "NotePrefix", "ContactsUpdate");
      }
    }

    private void BuildContactXml(XmlElement oContacts)
    {
      //(registrant = 0, technical = 1, admin = 2, billing = 3)      
      foreach (var key in _contacts.Keys)
      {
        XmlElement oContactXml = (XmlElement)AddNode(oContacts, "CONTACT");
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
