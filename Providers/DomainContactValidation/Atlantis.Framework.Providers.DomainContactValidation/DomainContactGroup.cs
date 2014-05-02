using Atlantis.Framework.DomainContactValidation.Interface;
using Atlantis.Framework.DomainsTrustee.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainContactValidation.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.DomainContactValidation
{
  public class DomainContactGroup : IDomainContactGroup
  {
    private const int DOMAIN_VALIDATION_CHECK = 782;
    private const int DOMAIN_TRUSTEE_REQUESTID = 781;
    private const string LINK_ID_ATTRIBUTE_NAME = "link_id";
    private const string CONTACT_GROUP_INFO_ELEMENT_NAME = "contactGroupInfo";
    private const string TLD_ELEMENT_NAME = "tld";
    private const string PRIVATE_LABEL_ID_ATTRIBUTE_NAME = "privateLabelId";
    private const string CONTACT_TYPE_ATTRIBUTE_NAME = "contactType";
    public const string CONTACT_INFO_ELEMENT_NAME = "contactInfo";
    private readonly Dictionary<DomainContactType, IDomainContact> _domainContactGroup = new Dictionary<DomainContactType, IDomainContact>();
    private readonly int _privateLabelId;

    private bool? _skipValidation;
    private bool SkipValidation
    {
      get
      {
        if (!_skipValidation.HasValue)
        {
          _skipValidation = false;
          bool skip;

          if (bool.TryParse(DataCache.DataCache.GetAppSetting("AVAIL_NO_VALIDATE_CONTACT"), out skip))
          {
            _skipValidation = skip;
          }
        }

        return _skipValidation.Value;
      }
    }

    private IDotTypeProvider _dotTypeProvider;
    private IDotTypeProvider GetDotTypeProvider
    {
      get { return _dotTypeProvider ?? (_dotTypeProvider = _container.Resolve<IDotTypeProvider>()); }
    }

    #region Constructors

    private static IEnumerable<string> RemoveLeadingPeriodsOnTlds(IEnumerable<string> tlds)
    {
      var result = new HashSet<string>();

      foreach (var sTld in tlds)
      {
        result.Add(sTld[0] == '.' ? sTld.ToUpperInvariant().Substring(1) : sTld.ToUpperInvariant());
      }

      return result;
    }

    private static Dictionary<string, LaunchPhases> RemoveLeadingPeriodsOnTlds(Dictionary<string, LaunchPhases> tlds)
    {
      var result = new Dictionary<string, LaunchPhases>();

      foreach (var sTld in tlds)
      {
        result.Add(sTld.Key[0] == '.' ? sTld.Key.ToUpperInvariant().Substring(1) : sTld.Key.ToUpperInvariant(), sTld.Value);
      }

      return result;
    }

    private readonly IProviderContainer _container;

    internal DomainContactGroup(IProviderContainer container, IEnumerable<string> tlds, int privateLabelId)
    {
      _container = container;
      Tlds = tlds;

      if (!Tlds.Any())
      {
        throw new Exception("TLD collection for Domain Contact Group cannot be empty.");
      }

      _privateLabelId = privateLabelId;
      ContactGroupId = Guid.NewGuid().ToString();
    }

    internal DomainContactGroup(IProviderContainer container, string contactGroupXml)
    {
      _container = container;

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
          var sTld = sTldNode.InnerText;
          Tlds = Tlds.Concat(new List<string>() { sTld });
        }
      }

      var xmlContactNodes = xmlContactGroupDoc.SelectNodes("//" + DomainContact.CONTACT_ELEMENT_NAME);

      if (xmlContactNodes != null)
      {
        foreach (XmlNode xmlContactNode in xmlContactNodes)
        {
          var xmlContactElement = (XmlElement) xmlContactNode;
          var sContactType = xmlContactElement.GetAttribute(CONTACT_TYPE_ATTRIBUTE_NAME);
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
      var newDomainContactGroup = new DomainContactGroup(_container, Tlds, _privateLabelId);

      foreach (var pair in _domainContactGroup)
      {
        newDomainContactGroup._domainContactGroup[pair.Key] = (IDomainContact)pair.Value.Clone();
      }

      return newDomainContactGroup;
    }

    public IDictionary<string, ITuiFormInfo> GetTuiFormInfo(IEnumerable<string> tlds)
    {
      tlds = RemoveLeadingPeriodsOnTlds(tlds);
      var result = new Dictionary<string, ITuiFormInfo>(8, StringComparer.OrdinalIgnoreCase);
      var contactList = GetDomainsTrusteeContacts(_domainContactGroup);
      var tldArray = tlds as string[] ?? tlds.ToArray();

      if (contactList.Count > 0 && tldArray.Length > 0)
      {
        var contactsDomains = new DomainsTrusteeContactsTlds(contactList, tldArray.ToList());
        var requestData = new DomainsTrusteeRequestData
        {
          ContactsTldsList = new List<DomainsTrusteeContactsTlds> { contactsDomains }
        };

        DomainsTrusteeResponseData response;

        try
        {
          response = (DomainsTrusteeResponseData)Engine.Engine.ProcessRequest(requestData, DOMAIN_TRUSTEE_REQUESTID);
        }
        catch (Exception ex)
        {
          var aex = new AtlantisException("DomainContactGroup.GetTuiFormInfo", string.Empty, "0", "Error running domains trustee triplet", string.Join("|", tldArray), string.Empty, string.Empty, string.Empty, string.Empty, 0);
          Engine.Engine.LogAtlantisException(aex);
          response = null;
        }

        if (response != null)
        {
          foreach (var tld in tldArray)
          {
            DomainsTrusteeResponse domainTrusteeResponse;

            if (response.TryGetDomainTrustee(tld, out domainTrusteeResponse) && !string.IsNullOrEmpty(domainTrusteeResponse.TuiFormType))
            {
              result.Add(tld, new TuiFormInfo(domainTrusteeResponse.TuiFormType, domainTrusteeResponse.VendorId));
            }
          }
        }
      }

      return result;
    }

    public IDictionary<string, ITuiFormInfo> GetTuiFormInfo(Dictionary<string, LaunchPhases> tlds)
    {
      var result = new Dictionary<string, ITuiFormInfo>(8, StringComparer.OrdinalIgnoreCase);
      tlds = RemoveLeadingPeriodsOnTlds(tlds);
      var contactList = GetDomainsTrusteeContacts(_domainContactGroup);
      var tldArray = tlds.Keys.ToList();

      if (contactList.Count > 0 && tldArray.Count > 0)
      {
        var contactsDomains = new DomainsTrusteeContactsTlds(contactList, tldArray);
        var requestData = new DomainsTrusteeRequestData
        {
          ContactsTldsList = new List<DomainsTrusteeContactsTlds> {contactsDomains}
        };

        DomainsTrusteeResponseData response;

        try
        {
          response = (DomainsTrusteeResponseData)Engine.Engine.ProcessRequest(requestData, DOMAIN_TRUSTEE_REQUESTID);
        }
        catch (Exception ex)
        {
          var aex = new AtlantisException("DomainContactGroup.GetTuiFormInfo", string.Empty, "0", "Error running domains trustee triplet", string.Join("|", tldArray), string.Empty, string.Empty, string.Empty, string.Empty, 0);
          Engine.Engine.LogAtlantisException(aex);
          response = null;
        }

        if (response != null)
        {
          foreach (var tld in tldArray)
          {
            DomainsTrusteeResponse domainTrusteeResponse;

            if (response.TryGetDomainTrustee(tld, out domainTrusteeResponse) && !string.IsNullOrEmpty(domainTrusteeResponse.TuiFormType))
            {
              result[tld] = new TuiFormInfo(domainTrusteeResponse.TuiFormType, domainTrusteeResponse.VendorId);
            }
          }
        }
      }

      return result;
    }
    private List<DomainsTrusteeContact> GetDomainsTrusteeContacts(Dictionary<DomainContactType, IDomainContact> domainContacts)
    {
      var contactList = new List<DomainsTrusteeContact>(4);

      foreach (var pair in domainContacts)
      {
        var domainsTrusteeContactType = DomainsTrusteeContactTypes.Registrant;

        switch (pair.Key)
        {
          case DomainContactType.Registrant:
          case DomainContactType.RegistrantUnicode:
            domainsTrusteeContactType = DomainsTrusteeContactTypes.Registrant;
            break;
          case DomainContactType.Billing:
          case DomainContactType.BillingUnicode:
            domainsTrusteeContactType = DomainsTrusteeContactTypes.Billing;
            break;
          case DomainContactType.Technical:
          case DomainContactType.TechnicalUnicode:
            domainsTrusteeContactType = DomainsTrusteeContactTypes.Technical;
            break;
          case DomainContactType.Administrative:
          case DomainContactType.AdministrativeUnicode:
            domainsTrusteeContactType = DomainsTrusteeContactTypes.Administrative;
            break;
        }

        contactList.Add(new DomainsTrusteeContact(domainsTrusteeContactType, pair.Value.Country));

        if (domainsTrusteeContactType == DomainsTrusteeContactTypes.Registrant)
        {
          if (!domainContacts.ContainsKey(DomainContactType.Billing))
          {
            contactList.Add(new DomainsTrusteeContact(DomainsTrusteeContactTypes.Billing, pair.Value.Country));
          }
          if (!domainContacts.ContainsKey(DomainContactType.Technical))
          {
            contactList.Add(new DomainsTrusteeContact(DomainsTrusteeContactTypes.Technical, pair.Value.Country));
          }
          if (!domainContacts.ContainsKey(DomainContactType.Administrative))
          {
            contactList.Add(new DomainsTrusteeContact(DomainsTrusteeContactTypes.Administrative, pair.Value.Country));
          }
        }
      }

      return contactList;
    }

    #region Properties

    public HashSet<string> GetTlds()
    {
      var result = new HashSet<string>(Tlds);
      return result;
    }

    private IEnumerable<string> Tlds { get; set; }

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
        var sText = "Cannot generate contact XML, no " + DomainContactType.Registrant + " defined.";
        throw new Exception(sText);
      }

      var xmlContactInfoDoc = new XmlDocument();
      var xmlContactInfo = xmlContactInfoDoc.CreateElement(CONTACT_INFO_ELEMENT_NAME);
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

    private bool SetContactInt(DomainContactType contactType, IDomainContact domainContact, bool onlySetIfValid)
    {
      if (!SkipValidation)
      {
        ValidateContact(domainContact, Tlds, DomainCheckType.Other, contactType, true);
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
              _domainContactGroup[DomainContactType.Administrative] = oOldDomainContact.Clone() as IDomainContact;
            if (!_domainContactGroup.ContainsKey(DomainContactType.Billing))
              _domainContactGroup[DomainContactType.Billing] = oOldDomainContact.Clone() as IDomainContact;
            if (!_domainContactGroup.ContainsKey(DomainContactType.Technical))
              _domainContactGroup[DomainContactType.Technical] = oOldDomainContact.Clone() as IDomainContact;
          }
        }

        _domainContactGroup[contactType] = domainContact;
      }

      if (domainContact.IsValid)
      {
        var tuiFormsInfo = GetTuiFormInfo(Tlds);
        foreach (var tuiFormInfo in tuiFormsInfo)
        {
          domainContact.AddTuiFormsInfo(tuiFormInfo.Key, tuiFormInfo.Value);
          domainContact.AddTrusteeVendorIds(tuiFormInfo.Key, tuiFormInfo.Value.VendorId);
        }
        
      }

      return domainContact.IsValid;
    }

    public bool TrySetContact(DomainContactType contactType, IDomainContact domainContact)
    {
      return SetContactInt(contactType, domainContact, true);
    }

    /// <summary>
    /// The SetContact function performs a validation of the contact against the existing
    /// set of tlds.
    /// </summary>
    /// <param name="contactType">Type of contact to be added</param>
    /// <param name="domainContact">Domain Contact</param>
    public bool SetContact(DomainContactType contactType, IDomainContact domainContact)
    {
      return SetContactInt(contactType, domainContact, false);
    }

    public bool SetContacts(IDomainContact registrantContact, IDomainContact technicalContact, IDomainContact administrativeContact, IDomainContact billingContact)
    {
      if (!SkipValidation)
      {
        ValidateContact(registrantContact, Tlds, DomainCheckType.Other, DomainContactType.Registrant, true);
        ValidateContact(technicalContact, Tlds, DomainCheckType.Other, DomainContactType.Technical, true);
        ValidateContact(administrativeContact, Tlds, DomainCheckType.Other, DomainContactType.Administrative, true);
        ValidateContact(billingContact, Tlds, DomainCheckType.Other, DomainContactType.Billing, true);
      }
      _domainContactGroup[DomainContactType.Registrant] = registrantContact;
      _domainContactGroup[DomainContactType.Administrative] = administrativeContact;
      _domainContactGroup[DomainContactType.Billing] = billingContact;
      _domainContactGroup[DomainContactType.Technical] = technicalContact;

      var tuiFormsInfo = GetTuiFormInfo(Tlds);

      foreach (var tuiFormInfo in tuiFormsInfo)
      {
        _domainContactGroup[DomainContactType.Registrant].AddTuiFormsInfo(tuiFormInfo.Key, tuiFormInfo.Value);
        _domainContactGroup[DomainContactType.Registrant].AddTrusteeVendorIds(tuiFormInfo.Key, tuiFormInfo.Value.VendorId);

        _domainContactGroup[DomainContactType.Administrative].AddTuiFormsInfo(tuiFormInfo.Key, tuiFormInfo.Value);
        _domainContactGroup[DomainContactType.Administrative].AddTrusteeVendorIds(tuiFormInfo.Key, tuiFormInfo.Value.VendorId);

        _domainContactGroup[DomainContactType.Billing].AddTuiFormsInfo(tuiFormInfo.Key, tuiFormInfo.Value);
        _domainContactGroup[DomainContactType.Billing].AddTrusteeVendorIds(tuiFormInfo.Key, tuiFormInfo.Value.VendorId);

        _domainContactGroup[DomainContactType.Technical].AddTuiFormsInfo(tuiFormInfo.Key, tuiFormInfo.Value);
        _domainContactGroup[DomainContactType.Technical].AddTrusteeVendorIds(tuiFormInfo.Key, tuiFormInfo.Value.VendorId);
      }

      return (((registrantContact.IsValid && technicalContact.IsValid) && administrativeContact.IsValid) && billingContact.IsValid);
    }

    /************************************************************************************/
    /// <summary>
    /// The SetContact (overloaded) function performs a validation of the contact against
    /// the existing set of tlds for all DomainContactTypes.
    /// </summary>
    /// <param name="domainContact">Domain Contact</param>
    public bool SetContact(IDomainContact domainContact)
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

      if (domainContact.IsValid)
      {
        var tuiFormsInfo = GetTuiFormInfo(Tlds);

        foreach (var tuiFormInfo in tuiFormsInfo)
        {
          domainContact.AddTuiFormsInfo(tuiFormInfo.Key, tuiFormInfo.Value);
          domainContact.AddTrusteeVendorIds(tuiFormInfo.Key, tuiFormInfo.Value.VendorId);
        }
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
    public void SetInvalidContact(IDomainContact domainContact, DomainContactType domainContactType, IEnumerable<string> tlds, IDomainContactError domainContactError)
    {
      Tlds = tlds;
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
    public void SetInvalidContact(IDomainContact domainContact, DomainContactType domainContactType, IEnumerable<string> tlds, List<IDomainContactError> domainContactErrors)
    {
      Tlds = tlds;
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
          throw new Exception("Cannot delete the default (Registrant) IDomainContact.");
        case DomainContactType.RegistrantUnicode:
          throw new Exception("Cannot delete the default (RegistrantUnicode) IDomainContact.");
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
    public IDomainContact GetContact(DomainContactType contactType)
    {
      IDomainContact result = null;
      var contact = GetContactInt(contactType);

      if (contact != null)
      {
        result = contact.Clone() as IDomainContact;
      }

      return result;
    }

    private IDomainContact GetContactInt(DomainContactType contactType)
    {
      IDomainContact result = null;

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

        if (result != null)
        {
          var tuiFormsInfo = GetTuiFormInfo(Tlds);
          foreach (var tuiFormInfo in tuiFormsInfo)
          {
            result.AddTuiFormsInfo(tuiFormInfo.Key, tuiFormInfo.Value);
            result.AddTrusteeVendorIds(tuiFormInfo.Key, tuiFormInfo.Value.VendorId);
          }
          
        }
      }

      return result;
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
          var sContactXml = oDomainContact.GetContactXmlForSession(domainContactType);
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

      foreach (var tld in Tlds)
      {
        var xmlTldElement = xmlContactInfoDoc.CreateElement(TLD_ELEMENT_NAME);
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
        var isValid = false;

        if ((Tlds.Any()) && (_domainContactGroup.Count > 0))
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
      var tldArray = tlds as string[] ?? tlds.ToArray();
     
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
          ValidateContact(oDomainContact, tldArray, checkType, contactType, false);
        }
      }
    }

    private void AddContactErrorsToDomainContact(IEnumerable<DomainContactValidationError> responseErrors, IDomainContact contact)
    {
      foreach (var responseError in responseErrors)
      {
        var domainContactError = new DomainContactError(
          responseError.Attribute, 
          responseError.Code,  
          responseError.ContactType,
          responseError.Description);

        contact.Errors.Add(domainContactError);
      }
    }

    private void ValidateContact(IDomainContact contact, 
                                  IEnumerable<string> tlds, 
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

      string marketId = "en-us";
      if (_container.CanResolve<ILocalizationProvider>())
      {
        var localizationProvider = _container.Resolve<ILocalizationProvider>();
        marketId = localizationProvider.MarketInfo.Id;
      }
      
      var request = new DomainContactValidationRequestData(checkType.ToString(), (int)contactType, domainContactValidation, tlds, PrivateLabelId, marketId);
     
      var response = (DomainContactValidationResponseData)Engine.Engine.ProcessRequest(request, DOMAIN_VALIDATION_CHECK);
      var xml = response.ToXML();
      
      if (clearErrors)
      {
        contact.Errors.Clear();
      }
      
      AddContactErrorsToDomainContact(response.Errors, contact);

      if (contact.IsValid)
      {
        contact.AddAdditionalContactAttributes(response.ResponseAttributes);
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
      var isValid = IsValid;
      var existingTlds = Tlds;
      var newTlds = tlds;
      var newlyAddedTlds = new HashSet<string>();

      foreach (var tld in newTlds)
      {
        if (!existingTlds.Contains(tld))
        {
          newlyAddedTlds.Add(tld);
        }
      }

      var doNewValidation = (newlyAddedTlds.Count > 0);
      var doReValidation = false;

      // If removing Tlds and group was invalid, group could become valid again.
      if (!isValid)
      {
        var diff = existingTlds.ToArray().Length - (newTlds.ToArray().Length - newlyAddedTlds.Count);
        doReValidation = (diff > 0);
      }

      // Finally replace our Tlds and revalidate contacts
      Tlds = newTlds;

      if (doNewValidation || doReValidation)
      {
        IEnumerable<string> validationTlds;
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

      foreach (var contactType in AllContactTypes)
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
      var bRet = false;

      if (HasExplicitDomainContact(contactType))
      {
        _domainContactGroup[contactType].PreferredLanguage = language;
        bRet = true;
      }

      return bRet;
    }
  }
  
}
