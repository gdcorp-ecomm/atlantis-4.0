using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using Atlantis.Framework.Providers.DomainContactValidation.Interface;

namespace Atlantis.Framework.Providers.DomainContactValidation
{
  public sealed class DomainContact : XmlDocument, IDomainContact
  {
    private readonly XmlElement _contactElement;

    public const string CONTACT_ELEMENT_NAME = "contact"; 

    #region Properties

    public List<IDomainContactError> Errors { get; private set; }
    public Dictionary<string, string> AdditionalContactAttributes { get; private set; }
    public Dictionary<string, string> TrusteeVendorIds { get; private set; }
    
    public void AddTrusteeVendorIds(string key, string value)
    {
      TrusteeVendorIds[key] = value;
    }

    public bool IsValid
    {
      get { return (Errors.Count == 0); }
    }

    public string FirstName
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.FirstName); }
      set { _contactElement.SetAttribute(DomainContactAttributes.FirstName, value); }
    }

    public string LastName
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.LastName); }
      set { _contactElement.SetAttribute(DomainContactAttributes.LastName, value); }
    }

    public string Email
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.Email); }
      set { _contactElement.SetAttribute(DomainContactAttributes.Email, value); }
    }

    public string Company
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.Organization); }
      set { _contactElement.SetAttribute(DomainContactAttributes.Organization, value); }
    }

    public bool IsLegalRegistrant
    {
      get 
      {
        bool result = false;
        if (Company.Length > 0)
        {
          result = _contactElement.GetAttribute(DomainContactAttributes.CheckedOrganization).Equals("1");
        }
        return result;
      }
      set { _contactElement.SetAttribute(DomainContactAttributes.CheckedOrganization, value ? "1" : string.Empty); }
    }

    public string Address1
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.Address1); }
      set { _contactElement.SetAttribute(DomainContactAttributes.Address1, value); }
    }

    public string Address2
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.Address2); }
      set { _contactElement.SetAttribute(DomainContactAttributes.Address2, value); }
    }

    public string City
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.City); }
      set { _contactElement.SetAttribute(DomainContactAttributes.City, value); }
    }

    public string Country
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.Country); }
      set { _contactElement.SetAttribute(DomainContactAttributes.Country, value); }
    }

    public string State
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.State); }
      set { _contactElement.SetAttribute(DomainContactAttributes.State, value); }
    }

    public string Zip
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.Zip); }
      set { _contactElement.SetAttribute(DomainContactAttributes.Zip, value); }
    }

    public string Phone
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.Phone); }
      set { _contactElement.SetAttribute(DomainContactAttributes.Phone, value); }
    }

    public string Fax
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.Fax); }
      set { _contactElement.SetAttribute(DomainContactAttributes.Fax, value); }
    }

    public string CanadianPresence
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.CanadianPresence); }
      set { _contactElement.SetAttribute(DomainContactAttributes.CanadianPresence, value); }
    }

    public string PreferredLanguage
    {
      get { return _contactElement.GetAttribute(DomainContactAttributes.PreferredLanguage); }
      set { _contactElement.SetAttribute(DomainContactAttributes.PreferredLanguage, value); }
    }

    #endregion

    public void AddAdditionalContactAttributes(XmlAttributeCollection addlContactAttributes)
    {
      if (addlContactAttributes == null)
        return;

      foreach (XmlAttribute attr in addlContactAttributes)
      {
        switch (attr.Name)
        {
          case DomainContactAttributes.FirstName:
          case DomainContactAttributes.LastName:
          case DomainContactAttributes.Email:
          case DomainContactAttributes.Organization:
          case DomainContactAttributes.CheckedOrganization:
          case DomainContactAttributes.Address1:
          case DomainContactAttributes.Address2:
          case DomainContactAttributes.City:
          case DomainContactAttributes.State:
          case DomainContactAttributes.Zip:
          case DomainContactAttributes.Country:
          case DomainContactAttributes.Phone:
          case DomainContactAttributes.Fax:
          case DomainContactAttributes.Tlds:
          case DomainContactAttributes.PrivateLabelId:
          case DomainContactAttributes.DomainContactType:
          case DomainContactAttributes.Type:
          case DomainContactAttributes.PreferredLanguage:
          case DomainContactAttributes.CanadianPresence:
            break;
          default:
            AdditionalContactAttributes[attr.Name] = attr.Value;
            break;
        }
      }
    }

    public string GetContactXml(DomainContactType contactType)
    {
      _contactElement.SetAttribute(DomainContactAttributes.DomainContactType, ((int)contactType).ToString(CultureInfo.InvariantCulture));
      var errorXml = new StringBuilder();

      foreach (var error in Errors)
      {
        errorXml.Append(error.InnerXml);
      }

      _contactElement.InnerXml = errorXml.ToString();

      // remove attr. that are not valid for cart post
      var doc = new XmlDocument();
      doc.LoadXml(InnerXml);
      XmlElement contactElement = doc.DocumentElement;

      var xml = string.Empty;

      if (contactElement != null)
      {
        contactElement.RemoveAttribute(DomainContactAttributes.CanadianPresence);

        if (contactElement.OwnerDocument != null)
        {
          xml = contactElement.OwnerDocument.InnerXml;
        }
      }

      return xml;
    }

    public string GetContactXmlForSession(DomainContactType contactType)
    {
      _contactElement.SetAttribute(DomainContactAttributes.DomainContactType, ((int)contactType).ToString(CultureInfo.InvariantCulture));

      foreach (KeyValuePair<string, string> attr in AdditionalContactAttributes)
      {
        _contactElement.SetAttribute(attr.Key, attr.Value);
      }

      var errorXml = new StringBuilder();
      foreach (var error in Errors)
      {
        errorXml.Append(error.InnerXml);
      }

      var trusteeXml = new StringBuilder();
      foreach (KeyValuePair<string, string> trustee in TrusteeVendorIds)
      {
        trusteeXml.Append("<trustee tld=\"" + trustee.Key + "\" vendorid=\"" + trustee.Value + "\"/>");
      }

      _contactElement.InnerXml = errorXml + trusteeXml.ToString();

      return InnerXml;
    }
    
    public override XmlNode Clone()
    {
      var result = new DomainContact(FirstName, LastName,
        Email, Company, IsLegalRegistrant, Address1, Address2, City, 
        State, Zip, Country, Phone, Fax, CanadianPresence, PreferredLanguage);
      
      foreach (KeyValuePair<string, string> attr in AdditionalContactAttributes)
      {
        result.AdditionalContactAttributes[attr.Key] = attr.Value;
      }

      foreach (var error in Errors)
      {
        result.Errors.Add(error.Clone() as IDomainContactError);
      }

      foreach (KeyValuePair<string, string> pair in TrusteeVendorIds)
      {
        result.AddTrusteeVendorIds(pair.Key, pair.Value);
      }

      return result;
    }

    public DomainContact()
    {
      TrusteeVendorIds = new Dictionary<string, string>();
      AdditionalContactAttributes = new Dictionary<string, string>();
      Errors = new List<IDomainContactError>();
      _contactElement = CreateElement(CONTACT_ELEMENT_NAME);
      AppendChild(_contactElement);
    }

    public DomainContact(XmlDocument contactDoc) : this()
    {
      var root = contactDoc.SelectSingleNode("//" + CONTACT_ELEMENT_NAME) as XmlElement;
      if (root != null)
      {
        LoadContactFromElement(root);
      }
    }

    public DomainContact(XmlElement xmlElement) : this()
    {
      LoadContactFromElement(xmlElement);
    }

    public DomainContact( string firstName, string lastName, 
                          string email,     string company,   bool isLegalRegistrant, 
                          string address1,  string address2,  string city,
                          string state,     string zip,       string country,              
                          string phone,     string fax,       string canadianPresence,
                          string preferredLanguage)
      : this()
    {
      FirstName = firstName;
      LastName = lastName;
      Email = email;
      IsLegalRegistrant = isLegalRegistrant;
      Company = company;
      Address1 = address1;
      Address2 = address2;
      City = city;
      State = state;
      Zip = zip;
      Country = country;
      Phone = phone;
      Fax = fax;
      CanadianPresence = canadianPresence;
      PreferredLanguage = preferredLanguage;
    }

    public DomainContact(string firstName, string lastName,
                          string email, string company, bool isLegalRegistrant,
                          string address1, string address2, string city,
                          string state, string zip, string country,
                          string phone, string fax)
      : this()
    {
      FirstName = firstName;
      LastName = lastName;
      Email = email;
      IsLegalRegistrant = isLegalRegistrant;
      Company = company;
      Address1 = address1;
      Address2 = address2;
      City = city;
      State = state;
      Zip = zip;
      Country = country;
      Phone = phone;
      Fax = fax;
      CanadianPresence = string.Empty;
    }

    public void InvalidateContact(IDomainContactError error)
    {
      Errors.Clear();
      Errors.Add(error);
    }

    public void InvalidateContact(List<IDomainContactError> errors)
    {
      Errors = errors;
    }

    private void LoadContactFromElement(XmlElement element)
    {
      foreach (XmlAttribute attr in element.Attributes)
      {
        switch (attr.Name)
        {
          case DomainContactAttributes.FirstName:
            FirstName = attr.Value;
            break;
          case DomainContactAttributes.LastName:
            LastName = attr.Value;
            break;
          case DomainContactAttributes.Email:
            Email = attr.Value;
            break;
          case DomainContactAttributes.CheckedOrganization:
            IsLegalRegistrant = attr.Value.Equals("1");
            break;
          case DomainContactAttributes.Organization:
            Company = attr.Value;
            break;
          case DomainContactAttributes.Address1:
            Address1 = attr.Value;
            break;
          case DomainContactAttributes.Address2:
            Address2 = attr.Value;
            break;
          case DomainContactAttributes.City:
            City = attr.Value;
            break;
          case DomainContactAttributes.State:
            State = attr.Value;
            break;
          case DomainContactAttributes.Zip:
            Zip = attr.Value;
            break;
          case DomainContactAttributes.Country:
            Country = attr.Value;
            break;
          case DomainContactAttributes.Phone:
            Phone = attr.Value;
            break;
          case DomainContactAttributes.Fax:
            Fax = attr.Value;
            break;
          case DomainContactAttributes.PreferredLanguage:
              string preferredLang = attr.Value;

              if (!string.IsNullOrEmpty(preferredLang))
              {
                PreferredLanguage = preferredLang;
              }

              break;
          case DomainContactAttributes.CanadianPresence:
              string canadianPresence = attr.Value;

              if (!string.IsNullOrEmpty(canadianPresence))
              {
                CanadianPresence = canadianPresence;
              }

              break;
          default:
            AdditionalContactAttributes[attr.Name] = attr.Value;
            break;
        }
      }

      XmlNodeList errorNodes = element.SelectNodes("./" + DomainContactError.ERROR_ELEMENT_NAME);

      foreach (XmlNode errorNode in errorNodes)
      {
        var errorNodeElement = errorNode as XmlElement;
        if (errorNodeElement != null)
        {
          var error = new DomainContactError(errorNodeElement);
          Errors.Add(error);
        }
      }

      XmlNodeList trusteeNodes = element.SelectNodes("./trustee");

      foreach (XmlElement trustee in trusteeNodes)
      {
        string dotType = trustee.GetAttribute("tld");
        string vendorId = trustee.GetAttribute("vendorid");
        TrusteeVendorIds[dotType] = vendorId;
      }
    }
  }
}
