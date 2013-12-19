using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Atlantis.Framework.DomainContactValidation.Interface;
using Atlantis.Framework.Providers.DomainContactValidation.Interface;

namespace Atlantis.Framework.Providers.DomainContactValidation
{
  public class DomainContactGroup : ICloneable, IDomainContactGroup
  {
    private const int DOMAIN_VALIDATION_CHECK = 782;

    private const string LINK_ID_ATTRIBUTE_NAME = "link_id";
    private const string CONTACT_GROUP_INFO_ELEMENT_NAME = "contactGroupInfo";
    private const string TLD_ELEMENT_NAME = "tld";
    private const string PRIVATE_LABEL_ID_ATTRIBUTE_NAME = "privateLabelId";
    private const string CONTACT_TYPE_ATTRIBUTE_NAME = "contactType";
    public const string CONTACT_INFO_ELEMENT_NAME = "contactInfo";

    private readonly Dictionary<DomainContactType, DomainContact> _domainContactGroup = new Dictionary<DomainContactType, DomainContact>();

    private readonly int _privateLabelId;

    private bool? _skipValidation;
    private bool SkipValidation
    {
      get
      {
        if (_skipValidation == null)
        {
          _skipValidation = false;
          bool skip;
          if (bool.TryParse(DataCache.DataCache.GetAppSetting("AVAIL_NO_VALIDATE_CONTACT"), out skip))
          {
            _skipValidation = skip;
          }
        }
        return (bool)_skipValidation;
      }
    }

    #region Constructors

    private static HashSet<string> CleanTlds(IEnumerable<string> tlds)
    {
      var result = new HashSet<string>();
      foreach (string sTld in tlds)
      {
        if (sTld[0] != '.')
        {
          result.Add(sTld.ToUpperInvariant().Insert(0, "."));
        }
        else
        {
          result.Add(sTld.ToUpperInvariant());
        }
      }
      return result;
    }

    public DomainContactGroup(IEnumerable<string> tlds, int privateLabelId)
    {
      Tlds = CleanTlds(tlds);

      if (Tlds.Count == 0)
      {
        throw new Exception("TLD collection for Domain Contact Group cannot be empty.");
      }

      _privateLabelId = privateLabelId;
      ContactGroupId = Guid.NewGuid().ToString();
    }

    public DomainContactGroup(string contactGroupXml)
    {
      Tlds = new HashSet<string>();
      var xmlContactGroupDoc = new XmlDocument();
      xmlContactGroupDoc.LoadXml(contactGroupXml);

      var xmlContactGroupElement = (XmlElement)xmlContactGroupDoc.SelectSingleNode("//" + CONTACT_GROUP_INFO_ELEMENT_NAME);

      if (xmlContactGroupElement != null)
      {
        var privateLabelId = xmlContactGroupElement.GetAttribute(PRIVATE_LABEL_ID_ATTRIBUTE_NAME);
        int.TryParse(privateLabelId, out _privateLabelId);
      }
      if (xmlContactGroupElement != null)
      {
        ContactGroupId = xmlContactGroupElement.GetAttribute(LINK_ID_ATTRIBUTE_NAME);
      }

      var xmlTldNodes = xmlContactGroupDoc.SelectNodes("//" + TLD_ELEMENT_NAME);

      if (xmlTldNodes != null)
      {
        foreach (XmlNode sTldNode in xmlTldNodes)
        {
          string sTld = sTldNode.InnerText;
          Tlds.Add(sTld);
        }
      }

      var xmlContactNodes = xmlContactGroupDoc.SelectNodes("//" + DomainContact.CONTACT_ELEMENT_NAME);
      if (xmlContactNodes != null)
      {
        foreach (XmlNode xmlContactNode in xmlContactNodes)
        {
          var xmlContactElement = (XmlElement) xmlContactNode;
          string sContactType = xmlContactElement.GetAttribute(CONTACT_TYPE_ATTRIBUTE_NAME);
          var oDomainContact = new DomainContact(xmlContactElement);
          var contactType = (DomainContactType) Enum.Parse(typeof (DomainContactType), sContactType);

          _domainContactGroup[contactType] = oDomainContact;

        }
      }
    }

    #endregion

    /************************************************************************************/
    /// <summary>
    /// The clone function duplicates the contents of an existing domain group.  Since
    /// the existing domain contacts have been validated, this step is skipped when cloning.
    /// A deep-copy operation is performed, with the exception of the Guid, which will be unqiue.
    /// </summary>
    /// <returns>a near-deep copy of the existing DomainContactGroup</returns>
    public object Clone()
    {
      var newDomainContactGroup = new DomainContactGroup(Tlds, _privateLabelId);

      foreach (KeyValuePair<DomainContactType, DomainContact> pair in _domainContactGroup)
      {
        newDomainContactGroup._domainContactGroup[pair.Key] = (DomainContact)pair.Value.Clone();
      }

      return newDomainContactGroup;
    }

    #region Properties

    public HashSet<string> GetTlds()
    {
      var result = new HashSet<string>(Tlds);
      return result;
    }

    private HashSet<string> Tlds { get; set; }

    public int PrivateLabelId
    {
      get
      {
        return _privateLabelId;
      }
    }

    public string ContactGroupId { get; private set; }

    #endregion

    public string GetContactXml()
    {

      if (!HasExplicitDomainContact(DomainContactType.Registrant))
      {
        string sText = "Cannot generate contact XML, no " + DomainContactType.Registrant + " defined.";
        throw new Exception(sText);
      }

      var xmlContactInfoDoc = new XmlDocument();
      XmlElement xmlContactInfo = xmlContactInfoDoc.CreateElement(CONTACT_INFO_ELEMENT_NAME);
      xmlContactInfoDoc.AppendChild(xmlContactInfo);
      xmlContactInfo.SetAttribute(LINK_ID_ATTRIBUTE_NAME, ContactGroupId);

      var xmlContactDoc = new XmlDocument();

      foreach (DomainContactType domainContactType in Enum.GetValues(typeof(DomainContactType)))
      {
        var oDomainContact = GetContactInt(domainContactType);
        if (oDomainContact != null)
        {
          string sContactXml = oDomainContact.GetContactXml(domainContactType);
          xmlContactDoc.LoadXml(sContactXml);
          XmlNode xmlContactNode = xmlContactDoc.SelectSingleNode("//" + DomainContact.CONTACT_ELEMENT_NAME);
          if (xmlContactNode != null)
          {
            xmlContactNode = xmlContactInfoDoc.ImportNode(xmlContactNode, true);
            xmlContactInfo.AppendChild(xmlContactNode);
          }
        }
      }

      return xmlContactInfoDoc.InnerXml;
    }

    /// <summary>
    /// This function identifies whether the contact type is explicity defined or dependant 
    /// on the default value.
    /// </summary>
    /// <param name="domainContactType"></param>
    /// <returns>true of explicity defined, false otherrwise</returns>
    public bool HasExplicitDomainContact(DomainContactType domainContactType)
    {
      return _domainContactGroup.ContainsKey(domainContactType);
    }

    private bool SetContactInt(DomainContactType contactType, DomainContact domainContact, bool onlySetIfValid)
    {
      if (!SkipValidation)
      {
        ValidateContact(domainContact, GetTldString(Tlds), DomainCheckType.Other, contactType, true);
      }

      if (!onlySetIfValid || domainContact.IsValid)
      {
        // if there is a registrant already acting as default, copy it to other types that are
        // inheriting it first before overwriting it.
        if (DomainContactType.Registrant == contactType)
        {
          if (_domainContactGroup.ContainsKey(contactType))
          {
            var oOldDomainContact = _domainContactGroup[DomainContactType.Registrant];

            if (!_domainContactGroup.ContainsKey(DomainContactType.Administrative))
              _domainContactGroup[DomainContactType.Administrative] = oOldDomainContact.Clone() as DomainContact;
            if (!_domainContactGroup.ContainsKey(DomainContactType.Billing))
              _domainContactGroup[DomainContactType.Billing] = oOldDomainContact.Clone() as DomainContact;
            if (!_domainContactGroup.ContainsKey(DomainContactType.Technical))
              _domainContactGroup[DomainContactType.Technical] = oOldDomainContact.Clone() as DomainContact;
          }
        }

        _domainContactGroup[contactType] = domainContact;
      }

      return domainContact.IsValid;
    }

    public bool TrySetContact(DomainContactType contactType, DomainContact domainContact)
    {
      return SetContactInt(contactType, domainContact, true);
    }

    /// <summary>
    /// The SetContact function performs a validation of the contact against the existing
    /// set of tlds.
    /// </summary>
    /// <param name="contactType">Type of contact to be added</param>
    /// <param name="domainContact">Domain Contact</param>
    public bool SetContact(DomainContactType contactType, DomainContact domainContact)
    {
      return SetContactInt(contactType, domainContact, false);
    }

    public bool SetContacts(DomainContact registrantContact, DomainContact technicalContact, DomainContact administrativeContact, DomainContact billingContact)
    {
      if (!SkipValidation)
      {
        string tldString = GetTldString(Tlds);
        ValidateContact(registrantContact, tldString, DomainCheckType.Other, DomainContactType.Registrant, true);
        ValidateContact(technicalContact, tldString, DomainCheckType.Other, DomainContactType.Technical, true);
        ValidateContact(administrativeContact, tldString, DomainCheckType.Other, DomainContactType.Administrative, true);
        ValidateContact(billingContact, tldString, DomainCheckType.Other, DomainContactType.Billing, true);
      }
      _domainContactGroup[DomainContactType.Registrant] = registrantContact;
      _domainContactGroup[DomainContactType.Administrative] = administrativeContact;
      _domainContactGroup[DomainContactType.Billing] = billingContact;
      _domainContactGroup[DomainContactType.Technical] = technicalContact;
      return (((registrantContact.IsValid && technicalContact.IsValid) && administrativeContact.IsValid) && billingContact.IsValid);
    }

    /************************************************************************************/
    /// <summary>
    /// The SetContact (overloaded) function performs a validation of the contact against
    /// the existing set of tlds for all DomainContactTypes.
    /// </summary>
    /// <param name="domainContact">Domain Contact</param>
    public bool SetContact(DomainContact domainContact)
    {
      _domainContactGroup[DomainContactType.Registrant] = domainContact;

      if (!SkipValidation)
      {
        DomainContactType[] contactTypes = { DomainContactType.Registrant,
                                                         DomainContactType.Technical,
                                                         DomainContactType.Administrative,
                                                         DomainContactType.Billing };
        ValidateGroupForTlds(Tlds, DomainCheckType.Other, contactTypes, true);
      }

      return domainContact.IsValid;
    }

    /************************************************************************************/
    /// <summary>
    /// The SetInvalidContact function sets Invalid contact bypassing
    /// Contact validation. For proper Contact Group validation use SetContact method.
    /// </summary>
    /// <param name="domainContact">Domain Contact</param>
    /// <param name="domainContactType">Domain Contact Type</param>
    /// <param name="tlds">Enumerable of Selected DotTypes</param>
    /// <param name="domainContactError">Domain Contact Error</param>
    public void SetInvalidContact(DomainContact domainContact, DomainContactType domainContactType, IEnumerable<string> tlds, IDomainContactError domainContactError)
    {
      Tlds = CleanTlds(tlds);
      domainContact.InvalidateContact(domainContactError);      
      _domainContactGroup[domainContactType] = domainContact;
    }

    /// <summary>
    /// The SetInvalidContact function sets Invalid contact bypassing
    /// Contact validation. For proper Contact Group validation use SetContact method.
    /// </summary>
    /// <param name="domainContact">Domain Contact</param>
    /// <param name="domainContactType">Domain Contact Type</param>
    /// <param name="tlds">Enumerable of Selected DotTypes</param>
    /// <param name="domainContactErrors">List of Domain Contact Errors</param>
    public void SetInvalidContact(DomainContact domainContact, DomainContactType domainContactType, IEnumerable<string> tlds, List<IDomainContactError> domainContactErrors)
    {
      Tlds = CleanTlds(tlds);
      domainContact.InvalidateContact(domainContactErrors);
      _domainContactGroup[domainContactType] = domainContact;
    }

    /************************************************************************************/
    /// <summary>
    /// This function clears the domain contact by type.  Attempting to clear the default (registrant)
    /// contact will raise an exception.
    /// </summary>
    /// <param name="domainContactType">Type of domain contact to clear</param>
    public void ClearContact(DomainContactType domainContactType)
    {
      switch (domainContactType)
      {
        case DomainContactType.Registrant:
          throw new Exception("Cannot delete the default (Registrant) DomainContact.");
        case DomainContactType.RegistrantUnicode:
          throw new Exception("Cannot delete the default (RegistrantUnicode) DomainContact.");
        default:
          if (_domainContactGroup.ContainsKey(domainContactType))
          {
            _domainContactGroup.Remove(domainContactType);
          }
          break;
      }
    }

    /************************************************************************************/
    /// <summary>
    /// This function returns the domain contact that should be used as  the designated contact type.
    /// It may be an explicitly assigned contact or will default to the Registrant for utf-8 or utf-16
    /// This function returns a clone of the contact in the collection.
    /// </summary>
    /// <param name="contactType">Desired type of domain contact</param>
    /// <returns>Domain contact (may be null)</returns>
    public DomainContact GetContact(DomainContactType contactType)
    {
      DomainContact result = null;
      var contact = GetContactInt(contactType);
      if (contact != null)
      {
        result = contact.Clone() as DomainContact;
      }
      return result;
    }

    private DomainContact GetContactInt(DomainContactType contactType)
    {
      DomainContact result = null;
      if (_domainContactGroup.ContainsKey(contactType))
      {
        result = _domainContactGroup[contactType];
      }
      else
      {
        if (contactType <= DomainContactType.Billing)  // utf-8 contact
        {
          if (_domainContactGroup.ContainsKey(DomainContactType.Registrant))
          {
            result = _domainContactGroup[DomainContactType.Registrant];
          }
        }
        else // The contact is a Unicode contact
        {
          if (_domainContactGroup.ContainsKey(DomainContactType.RegistrantUnicode))
          {
            result = _domainContactGroup[DomainContactType.RegistrantUnicode];
          }
        }
      }
      return result;
    }

    /************************************************************************************/
    /// <summary>
    /// This function constructs a string in the format .Com.Net.JP.etc  
    /// </summary>
    /// <returns></returns>

    private static string GetTldString(IEnumerable<string> tlds)
    {
      string result = String.Join("|", tlds.ToArray());
      if (result.Length == 0)
      {
        throw new ArgumentException("TLD collection for Domain Contact Group cannot be empty.");
      }
      return result;
    }

    private static string RemoveDotFromTldString(string tlds)
    {
      string retTlds = tlds;
      if (retTlds.Length > 0)
      {
        retTlds = retTlds.Replace("|.", "|");
        if (retTlds.StartsWith("."))
        {
          retTlds = retTlds.Remove(0, 1);
        }
      }

      return retTlds;
    }

    /******************************************************************************/
    /// <summary>
    /// This function returns an xml string that can be used to copy the object.  It contains
    /// all the explicit domain contacts, the list of tld nodes, the GUID and the private label id.
    /// </summary>
    /// <returns></returns>

    public override string ToString()
    {
      var xmlContactInfoDoc = new XmlDocument();
      XmlElement xmlContactInfo = xmlContactInfoDoc.CreateElement(CONTACT_GROUP_INFO_ELEMENT_NAME);
      xmlContactInfoDoc.AppendChild(xmlContactInfo);

      var xmlContactDoc = new XmlDocument();

      foreach (DomainContactType domainContactType in Enum.GetValues(typeof(DomainContactType)))
      {
        if (HasExplicitDomainContact(domainContactType))
        {
          var oDomainContact = GetContactInt(domainContactType);

          string sContactXml = oDomainContact.GetContactXmlForSession(domainContactType);
          xmlContactDoc.LoadXml(sContactXml);
          var xmlContactNode = xmlContactDoc.SelectSingleNode("//" + DomainContact.CONTACT_ELEMENT_NAME);
          if (xmlContactNode != null)
          {
            var xmlContactElement = (XmlElement)xmlContactInfoDoc.ImportNode(xmlContactNode, true);
            xmlContactElement.SetAttribute(CONTACT_TYPE_ATTRIBUTE_NAME, domainContactType.ToString());
            xmlContactInfo.AppendChild(xmlContactElement);
          }
        }
      }

      foreach (string tld in Tlds)
      {
        XmlElement xmlTldElement = xmlContactInfoDoc.CreateElement(TLD_ELEMENT_NAME);
        xmlTldElement.InnerText = tld;
        xmlContactInfo.AppendChild(xmlTldElement);
      }

      xmlContactInfo.SetAttribute(PRIVATE_LABEL_ID_ATTRIBUTE_NAME, _privateLabelId.ToString(CultureInfo.InvariantCulture));
      xmlContactInfo.SetAttribute(LINK_ID_ATTRIBUTE_NAME, ContactGroupId);

      return xmlContactInfoDoc.InnerXml;
    }

    public bool IsValid
    {
      get
      {
        bool isValid = false;
        if ((Tlds.Count > 0) && (_domainContactGroup.Count > 0))
        {
          isValid = true;
          foreach (DomainContactType contactType in Enum.GetValues(typeof(DomainContactType)))
          {
            if (HasExplicitDomainContact(contactType))
            {
              var contact = GetContactInt(contactType);
              if (!contact.IsValid)
              {
                isValid = false;
                break;
              }
            }
          }
        }
          
        return isValid;
      }
    }

    private void ValidateGroupForTlds(
      IEnumerable<string> tlds, 
      DomainCheckType checkType,
      IEnumerable<DomainContactType> contactTypes,
      bool clearExistingErrors)
    {
      string tldString = GetTldString(tlds);

      var domainContactTypes = contactTypes as DomainContactType[] ?? contactTypes.ToArray();

      if (clearExistingErrors)
      {
        foreach (var contactType in domainContactTypes)
        {
          if (HasExplicitDomainContact(contactType))
          {
            var oDomainContact = GetContactInt(contactType);
            oDomainContact.Errors.Clear();
          }
        }
      }

      foreach (DomainContactType contactType in domainContactTypes)
      {
        var oDomainContact = GetContactInt(contactType);
        if (null != oDomainContact)
        {
          ValidateContact(oDomainContact, tldString, checkType, contactType, false);
        }
      }
    }

    private void AddContactErrorsToDomainContact(IEnumerable<DomainContactValidationError> responseErrors, DomainContact contact)
    {
      foreach (var responseError in responseErrors)
      {
        var domainContactError = new DomainContactError(
          responseError.Attribute, 
          responseError.Code, 
          responseError.Description, 
          responseError.ContactType);

        contact.Errors.Add(domainContactError);
      }
    }

    private void ValidateContact(DomainContact contact, 
                                  string tldString, 
                                  DomainCheckType checkType, 
                                  DomainContactType contactType, 
                                  bool clearErrors)
    {

      var domainContactValidation = new Framework.DomainContactValidation.Interface.DomainContactValidation(
        contact.FirstName,
        contact.LastName,
        contact.Company,
        contact.Address1,
        contact.Address2,
        contact.City,
        contact.State,
        contact.Zip,
        contact.Country,
        contact.Phone,
        contact.Fax,
        contact.Email,
        contact.CanadianPresence);
      
      var request = new DomainContactValidationRequestData(checkType.ToString(), (int) contactType, domainContactValidation, RemoveDotFromTldString(tldString), PrivateLabelId);
     
      var response = (DomainContactValidationResponseData)Engine.Engine.ProcessRequest(request, DOMAIN_VALIDATION_CHECK);
      
      if (clearErrors)
      {
        contact.Errors.Clear();
      }
      
      AddContactErrorsToDomainContact(response.Errors, contact);

      if (contact.IsValid)
      {
        contact.AddAdditionalContactAttributes(response.ResponseAttributes);
      }

      if (response.TrusteeVendorIds.Count > 0)
      {
        foreach (KeyValuePair<string, string> trustee in response.TrusteeVendorIds)
        {
          contact.AddTrusteeVendorIds(trustee.Key,trustee.Value);
        }
      }
    }

    #region TLDs

    /// <summary>
    /// Resets the Tlds into the ContactGroup
    /// </summary>
    /// <param name="tlds">A non list of tlds</param>
    /// <returns>true if revalidation occurred</returns>
    public bool SetTlds(IEnumerable<string> tlds)
    {
      bool isValid = IsValid;

      HashSet<string> existingTlds = Tlds;
      HashSet<string> newTlds = CleanTlds(tlds);
      var newlyAddedTlds = new HashSet<string>();

      foreach (string tld in newTlds)
      {
        if (!existingTlds.Contains(tld))
        {
          newlyAddedTlds.Add(tld);
        }
      }

      bool doNewValidation = (newlyAddedTlds.Count > 0);
      bool doReValidation = false;

      // If removing Tlds and group was invalid, group could become valid again.
      if (!isValid)
      {
        int diff = existingTlds.Count - (newTlds.Count - newlyAddedTlds.Count);
        doReValidation = (diff > 0);
      }

      // Finally replace our Tlds and revalidate contacts
      Tlds = newTlds;

      if (doNewValidation || doReValidation)
      {
        HashSet<string> validationTlds;
        bool clearExistingErrors;
        if (doReValidation)
        {
          validationTlds = Tlds;
          clearExistingErrors = true;
        }
        else
        {
          validationTlds = newlyAddedTlds;
          clearExistingErrors = false;
        }

        ValidateGroupForTlds(
          validationTlds, 
          DomainCheckType.Other, 
          AllContactTypes,
          clearExistingErrors);
      }

      return (doNewValidation || doReValidation);
    }

    private List<DomainContactType> _allContactTypes;
    private IEnumerable<DomainContactType> AllContactTypes
    {
      get
      {
        if (_allContactTypes == null)
        {
          _allContactTypes = new List<DomainContactType>(8);
          foreach (DomainContactType contactType in Enum.GetValues(typeof(DomainContactType)))
          {
            _allContactTypes.Add(contactType);
          }
        }
        return _allContactTypes;
      }
    }

    #endregion

    public List<IDomainContactError> GetAllErrors()
    {
      var result = new List<IDomainContactError>();
      foreach (DomainContactType contactType in AllContactTypes)
      {
        if (HasExplicitDomainContact(contactType))
        {
          var contact = GetContactInt(contactType);
          result.AddRange(contact.Errors);
        }
      }
      return result;
    }

    public Dictionary<int, List<IDomainContactError>> GetAllErrorsByContactType()
    {
      var result = new Dictionary<int, List<IDomainContactError>>();
      foreach (var contactType in AllContactTypes)
      {
        var typeErrors = new List<IDomainContactError>();

        if (HasExplicitDomainContact(contactType))
        {
          var contact = GetContactInt(contactType);
          typeErrors.AddRange(contact.Errors);
          result.Add((int)contactType, typeErrors);
        }
      }
      return result;
    }

    /// <summary>
    /// The SetContactPreferredlanguage function updates an existing contact's preferred language
    /// </summary>
    /// <param name="contactType">Type of contact to be updated</param>
    /// <param name="language">Language string</param>
    public bool SetContactPreferredlanguage(DomainContactType contactType, string language)
    {
      bool bRet = false;

      if (HasExplicitDomainContact(contactType))
      {
        _domainContactGroup[contactType].PreferredLanguage = language;
        bRet = true;
      }
      return bRet;
    }
  }
  
}
