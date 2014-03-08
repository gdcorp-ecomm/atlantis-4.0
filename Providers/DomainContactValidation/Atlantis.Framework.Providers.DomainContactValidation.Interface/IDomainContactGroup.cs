using System;
using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.Providers.DomainContactValidation.Interface
{
  public interface IDomainContactGroup
  {
    HashSet<string> GetTlds();
    int PrivateLabelId { get; }

    string ContactGroupId { get; }

    string GetContactXml();

    /// <summary>
    /// This function identifies whether the contact type is explicity defined or dependant 
    /// on the default value.
    /// </summary>
    /// <param name="domainContactType"></param>
    /// <returns>true of explicity defined, false otherrwise</returns>
    bool HasExplicitDomainContact(DomainContactType domainContactType);

    bool TrySetContact(DomainContactType contactType, IDomainContact domainContact);

    /// <summary>
    /// The SetContact function performs a validation of the contact against the existing
    /// set of tlds.
    /// </summary>
    /// <param name="contactType">Type of contact to be added</param>
    /// <param name="domainContact">Domain Contact</param>
    bool SetContact(DomainContactType contactType, IDomainContact domainContact);


    bool SetContacts(IDomainContact registrantContact, IDomainContact technicalContact, IDomainContact administrativeContact, IDomainContact billingContact);

    /************************************************************************************/

    /// <summary>
    /// The SetContact (overloaded) function performs a validation of the contact against
    /// the existing set of tlds for all DomainContactTypes.
    /// </summary>
    /// <param name="domainContact">Domain Contact</param>
    bool SetContact(IDomainContact domainContact);

    /************************************************************************************/

    /// <summary>
    /// The SetInvalidContact function sets Invalid contact bypassing
    /// Contact validation. For proper Contact Group validation use SetContact method.
    /// </summary>
    /// <param name="domainContact">Domain Contact</param>
    /// <param name="domainContactType">Domain Contact Type</param>
    /// <param name="tlds">Enumerable of Selected DotTypes</param>
    /// <param name="domainContactError">Domain Contact Error</param>
    void SetInvalidContact(IDomainContact domainContact, DomainContactType domainContactType, IEnumerable<string> tlds, IDomainContactError domainContactError);

    /// <summary>
    /// The SetInvalidContact function sets Invalid contact bypassing
    /// Contact validation. For proper Contact Group validation use SetContact method.
    /// </summary>
    /// <param name="domainContact">Domain Contact</param>
    /// <param name="domainContactType">Domain Contact Type</param>
    /// <param name="tlds">Enumerable of Selected DotTypes</param>
    /// <param name="domainContactErrors">List of Domain Contact Errors</param>
    void SetInvalidContact(IDomainContact domainContact, DomainContactType domainContactType, IEnumerable<string> tlds, List<IDomainContactError> domainContactErrors);

    /************************************************************************************/

    /// <summary>
    /// This function clears the domain contact by type.  Attempting to clear the default (registrant)
    /// contact will raise an exception.
    /// </summary>
    /// <param name="domainContactType">Type of domain contact to clear</param>
    void ClearContact(DomainContactType domainContactType);

    /************************************************************************************/

    /// <summary>
    /// This function returns the domain contact that should be used as  the designated contact type.
    /// It may be an explicitly assigned contact or will default to the Registrant for utf-8 or utf-16
    /// This function returns a clone of the contact in the collection.
    /// </summary>
    /// <param name="contactType">Desired type of domain contact</param>
    /// <returns>Domain contact (may be null)</returns>
    IDomainContact GetContact(DomainContactType contactType);

    /// <summary>
    /// This function returns an xml string that can be used to copy the object.  It contains
    /// all the explicit domain contacts, the list of tld nodes, the GUID and the private label id.
    /// </summary>
    /// <returns></returns>
    string ToString();

    bool IsValid { get; }

    /// <summary>
    /// Resets the Tlds into the ContactGroup
    /// </summary>
    /// <param name="tlds">A non list of tlds</param>
    /// <returns>true if revalidation occurred</returns>
    bool SetTlds(IEnumerable<string> tlds);

    List<IDomainContactError> GetAllErrors();

    Dictionary<int, List<IDomainContactError>> GetAllErrorsByContactType();

    /// <summary>
    /// The SetContactPreferredlanguage function updates an existing contact's preferred language
    /// </summary>
    /// <param name="contactType">Type of contact to be updated</param>
    /// <param name="language">Language string</param>
    bool SetContactPreferredlanguage(DomainContactType contactType, string language);

    object Clone();

    [Obsolete("Use the new GetTuiFormInfo which takes in the dictionary of tlds with launch phases.")]
    IDictionary<string, ITuiFormInfo> GetTuiFormInfo(IEnumerable<string> tlds);

    IDictionary<string, ITuiFormInfo> GetTuiFormInfo(Dictionary<string, LaunchPhases> tlds);
  }
}
