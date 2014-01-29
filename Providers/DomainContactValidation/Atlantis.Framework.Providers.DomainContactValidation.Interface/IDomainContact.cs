using System.Collections.Generic;
using System.Xml;

namespace Atlantis.Framework.Providers.DomainContactValidation.Interface
{
  public interface IDomainContact
  {
    List<IDomainContactError> Errors { get; }

    Dictionary<string, string> AdditionalContactAttributes { get; }

    Dictionary<string, string> TrusteeVendorIds { get; }

    Dictionary<string, ITuiFormInfo> TuiFormsInfo { get; }

    void AddTrusteeVendorIds(string key, string value);

    void AddTuiFormsInfo(string tld, ITuiFormInfo tuiFormInfo);

    bool IsValid { get; }

    string FirstName { get; set; }

    string LastName { get; set; }

    string Email { get; set; }

    string Company { get; set; }

    bool IsLegalRegistrant { get; set; }

    string Address1 { get; set; }

    string Address2 { get; set; }

    string City { get; set; }

    string Country { get; set; }

    string State { get; set; }

    string Zip { get; set; }

    string Phone { get; set; }

    string Fax { get; set; }

    string CanadianPresence { get; set; }

    string PreferredLanguage { get; set; }

    void AddAdditionalContactAttributes(XmlAttributeCollection addlContactAttributes);

    string GetContactXml(DomainContactType contactType);

    string GetContactXmlForSession(DomainContactType contactType);

    void InvalidateContact(IDomainContactError error);

    void InvalidateContact(List<IDomainContactError> errors);

    XmlNode Clone();
  }
}