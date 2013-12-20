using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Providers.DomainContactValidation.Interface
{
  public interface IDomainContactValidationProvider
  {
    IDomainContact DomainContactInstance();

    IDomainContact DomainContactInstance(XmlDocument contactDoc);

    IDomainContact DomainContactInstance(XmlElement xmlElement);

    IDomainContact DomainContactInstance(string firstName, string lastName,
      string email, string company, bool isLegalRegistrant,
      string address1, string address2, string city,
      string state, string zip, string country,
      string phone, string fax, string canadianPresence = "",
      string preferredLanguage = null);

    IDomainContactGroup DomainContactGroupInstance(IEnumerable<string> tlds, int privateLabelId);

    IDomainContactGroup DomainContactGroupInstance(string contactGroupXml);

    IDomainContactError DomainContactErrorInstance();

    IDomainContactError DomainContactErrorInstance(string attribute, int code, string description, int contactType);

    IDomainContactError DomainContactErrorInstance(XmlElement errorXml);
  }
}
