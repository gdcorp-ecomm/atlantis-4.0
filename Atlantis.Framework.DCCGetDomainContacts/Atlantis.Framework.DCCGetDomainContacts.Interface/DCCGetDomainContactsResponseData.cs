using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetDomainContacts.Interface
{
  public class DCCGetDomainContactsResponseData : IResponseData
  {
    private AtlantisException _exception;

    public string ResponseXml { get; private set; }

    public string DomainName { get; private set; }

    bool _isSuccess;
    public bool IsSuccess
    {
      get { return (_exception == null && _isSuccess); }
    }

    public Dictionary<int, Dictionary<string, string>> _contacts = new Dictionary<int, Dictionary<string, string>>();

    public DCCGetDomainContactsResponseData(string responseXml)
    {
      ResponseXml = responseXml;
      PopulateFromXml(responseXml);
    }

    public DCCGetDomainContactsResponseData(string responseXml, AtlantisException exAtlantis)
    {
      ResponseXml = responseXml;
      _exception = exAtlantis;
    }

    public DCCGetDomainContactsResponseData(string responseXML, RequestData oRequestData, Exception ex)
    {
      ResponseXml = responseXML;
      _exception = new AtlantisException(oRequestData,
                                   "DCCGetDomainContactsResponseData", 
                                   ex.Message, 
                                   oRequestData.ToXML());
    }

    /*
<results>
<method>GetDomainInfoByNameWithContacts</method> 
<success>1</success>
<domains>
      <domain domainname="vector421.info" shopperid="847001" id="1619762" isproxied="0" isexpirationprotected="0" istransferprotected="0" registrationperiod="2" processing="success">
           <contact contacttype="registrant" name="Alex Passos" company="" address1="Some Address" address2="Some Address" city="Some City" state="Essex" pc="52402" country="United Kingdom" phone="0000000000" fax="" email="apassos@godaddy.com" />
           <contact contacttype="technical" name="Alex Passos" company="" address1="Some Address" address2="Some Address" city="Some City" state="Essex" pc="52402" country="United Kingdom" phone="0000000000" fax="" email="apassos@godaddy.com" />
           <contact contacttype="administrative" name="Alex Passos" company="" address1="Some Address" address2="Some Address" city="Some City" state="Essex" pc="52402" country="United Kingdom" phone="0000000000" fax="" email="apassos@godaddy.com" />
           <contact contacttype="billing" name="Alex Passos" company="" address1="Some Address" address2="Some Address" city="Some City" state="Essex" pc="52402" country="United Kingdom" phone="0000000000" fax="" email="apassos@godaddy.com" /> 
       </domain>
    <domain domainname="vector567.com" shopperid="847001" processing="failure" />
</domains>

</results>

    */

    private void PopulateFromXml(string basketXml)
    {
      
      XmlDocument xdDoc = new XmlDocument();
      xdDoc.LoadXml(basketXml);

      XmlElement xnSuccess = (XmlElement)xdDoc.SelectSingleNode("/results/success");

      if (xnSuccess != null)
      {
        if(xnSuccess.InnerText == "1")
        {
          _isSuccess = true;
        }
      }

      XmlElement xndomain = (XmlElement)xdDoc.SelectSingleNode("/results/domains/domain");
      if (xndomain != null)
      {
        DomainName = xndomain.Attributes["domainname"].Value;
        if(xndomain.Attributes["processing"].Value.ToLower() == "success")
        {
          _isSuccess = true;
        }
        else
        {
          _isSuccess = false;
        }
      }

      XmlNodeList xnlContacts = xdDoc.SelectNodes("/results/domains/domain/contact");
      foreach (XmlElement xnContact in xnlContacts)
      {
        Dictionary<string, string> oContact = new Dictionary<string,string>();
        foreach (XmlAttribute oContactAtter in xnContact.Attributes)
        {
          HandleAttribute(ref oContact, oContactAtter.Name, oContactAtter.Value);
        }
        _contacts.Add( getContactType(xnContact.Attributes["contacttype"].Value), oContact);
      }      
    }

    private void HandleAttribute( ref Dictionary<string, string> oContact, string attribName, string attribValue)
    {
      if (attribName.CompareTo("fname") == 0)
      {
        oContact.Add("FirstName", attribValue);
      }
      else if (attribName.CompareTo("lname") == 0)
      {
        oContact.Add("LastName", attribValue);
      }
      else if (attribName.CompareTo("contacttype") == 0)
      {
        oContact.Add("Type", getContactType(attribValue).ToString());
      }
      else if (attribName.CompareTo("company") == 0)
      {
        oContact.Add("Company", attribValue);
      }
      else if (attribName.CompareTo("address1") == 0)
      {
        oContact.Add("Address1", attribValue);
      }
      else if (attribName.CompareTo("address2") == 0)
      {
        oContact.Add("Address2", attribValue);
      }
      else if (attribName.CompareTo("city") == 0)
      {
        oContact.Add("City", attribValue);
      }
      else if (attribName.CompareTo("state") == 0)
      {
        oContact.Add("State", attribValue);
      }
      else if (attribName.CompareTo("pc") == 0)
      {
        oContact.Add("Zip", attribValue);
      }
      else if (attribName.CompareTo("country") == 0)
      {
        oContact.Add("Country", attribValue);
      }
      else if (attribName.CompareTo("phone") == 0)
      {
        oContact.Add("Phone", attribValue);
      }
      else if (attribName.CompareTo("fax") == 0)
      {
        oContact.Add("Fax", attribValue);
      }
      else if (attribName.CompareTo("email") == 0)
      {
        oContact.Add("Email", attribValue);
      }
    }

    private int getContactType(string contactType)
    {
      int iContactType = -1;
      if (contactType.CompareTo("registrant") == 0)
      {
        iContactType = 0;
      }
      else if (contactType.CompareTo("technical") == 0)
      {
        iContactType = 1;
      }
      else if (contactType.CompareTo("administrative") == 0)
      {
        iContactType = 2;
      }
      else if (contactType.CompareTo("billing") == 0)
      {
        iContactType = 3;
      }
      return iContactType;
    }

    

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return ResponseXml;
    }

    #endregion
  }
}
