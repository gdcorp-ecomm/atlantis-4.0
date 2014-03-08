using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainContactValidation.Interface;

namespace Atlantis.Framework.Providers.DomainContactValidation
{
  public class DomainContactValidationProvider : ProviderBase, IDomainContactValidationProvider // Framework providers should implement a corresponding interface
  {
    public DomainContactValidationProvider(IProviderContainer container) : base(container)
    {
      
    }

    public IDomainContact DomainContactInstance()
    {
      return new DomainContact();
    }

    public IDomainContact DomainContactInstance(XmlDocument contactDoc)
    {
      IDomainContact contact;

      var root = contactDoc.SelectSingleNode("//" + DomainContact.CONTACT_ELEMENT_NAME) as XmlElement;
      if (root != null)
      {
        contact = new DomainContact(root);
      }
      else
      {
        contact = new DomainContact();
      }

      return contact;
    }

    public IDomainContact DomainContactInstance(XmlElement xmlElement)
    {
      return new DomainContact(xmlElement);
    }

    public IDomainContact DomainContactInstance(string firstName, string lastName, string email, string company, bool isLegalRegistrant, string address1, string address2, string city, string state, string zip, 
      string country, string phone, string fax, string canadianPresence = "", string preferredLanguage = null)
    {
      return new DomainContact(firstName, lastName, email, company, isLegalRegistrant, address1, address2, city, state, zip, country, phone, fax, canadianPresence,
        preferredLanguage);
    }

    public IDomainContactGroup DomainContactGroupInstance(IEnumerable<string> tlds, int privateLabelId)
    {
      return new DomainContactGroup(Container, tlds, privateLabelId);
    }

    public IDomainContactGroup DomainContactGroupInstance(string contactGroupXml)
    {
      return new DomainContactGroup(Container, contactGroupXml);
    }

    public IDomainContactError DomainContactErrorInstance()
    {
      return new DomainContactError();
    }

    public IDomainContactError DomainContactErrorInstance(string attribute, int code, string description, int contactType)
    {
      return new DomainContactError(attribute, code, contactType, description);
    }

    public IDomainContactError DomainContactErrorInstance(XmlElement errorXml)
    {
      return new DomainContactError(errorXml);
    }
  }
}
