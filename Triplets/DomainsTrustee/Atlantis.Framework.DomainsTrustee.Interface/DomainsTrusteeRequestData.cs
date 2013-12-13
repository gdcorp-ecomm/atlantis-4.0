using System;
using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.Interface;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.DomainsTrustee.Interface
{
  public class DomainsTrusteeRequestData : RequestData
  {
    private const string CONTACT_TYPE_REGISTRANT = "registrant";
    private const string CONTACT_TYPE_ADMINISTRATIVE = "administrative";
    private const string CONTACT_TYPE_BILLING = "billing";
    private const string CONTACT_TYPE_TECHNICAL = "technical";

    public DomainsTrusteeRequestData()
    {
      RequestTimeout = TimeSpan.FromSeconds(20);
    }

    public IList<DomainsTrusteeContactsDomains> ContactsDomainsList { get; set; }

    public string ToJson()
    {
      var finalArray = new JArray();

      foreach (DomainsTrusteeContactsDomains contactsDomains in ContactsDomainsList)
      {
        var contactsDomainsObject = new JObject();

        var contacts = contactsDomains.Contacts;
        var domains = contactsDomains.Domains;

        var jContacts = new JProperty("Contacts");

        var jContactTypePropertyList = new List<JProperty>();
        foreach (var contact in contacts)
        {
          string strContactType = string.Empty;
          switch (contact.ContactType)
          {
            case DomainsTrusteeContactTypes.Registrant:
              strContactType = CONTACT_TYPE_REGISTRANT;
              break;
            case DomainsTrusteeContactTypes.Administrative:
              strContactType = CONTACT_TYPE_ADMINISTRATIVE;
              break;
            case DomainsTrusteeContactTypes.Billing:
              strContactType = CONTACT_TYPE_BILLING;
              break;
            case DomainsTrusteeContactTypes.Technical:
              strContactType = CONTACT_TYPE_TECHNICAL;
              break;
          }

          if (!string.IsNullOrEmpty(strContactType))
          {
            jContactTypePropertyList.Add(new JProperty(strContactType, contact.CountryCode));
          }
        }

        if (jContactTypePropertyList.Count > 0)
        {
          var a = new JObject();
          foreach (var jContactTypeProperty in jContactTypePropertyList)
          {
            a.Add(jContactTypeProperty);
          }
          jContacts.Value = a;
        }

        var jDomains = new JProperty("Domains");
        var jDomainArray = new JArray();

        foreach (var domain in domains)
        {
          jDomainArray.Add(new JObject(
                                      new JProperty("Name", domain.Sld),
                                      new JProperty("Tld", domain.Tld)
                                      )); 
        }
        if (jDomainArray.Count > 0)
        {
          jDomains.Value = jDomainArray;
        }

        contactsDomainsObject.Add(jContacts);
        contactsDomainsObject.Add(jDomains);
        finalArray.Add(contactsDomainsObject);
      }

      return finalArray.ToString();
    }

    public override string ToXML()
    {
      var items = new XElement("items");

      foreach (DomainsTrusteeContactsDomains cd in ContactsDomainsList)
      {
        var item = new XElement("item");

        foreach (DomainsTrusteeContact contact in cd.Contacts)
        {
          var contactElement = new XElement("contact");

          contactElement.Add(new XAttribute("contactType", contact.ContactType.ToString()));
          contactElement.Add(new XAttribute("countryCode", contact.CountryCode));

          item.Add(contactElement);
        }

        foreach (Domain domain in cd.Domains)
        {
          var domainElement = new XElement("domain");

          domainElement.Add(new XAttribute("name", domain.Sld));
          domainElement.Add(new XAttribute("tld", domain.Tld));

          item.Add(domainElement);
        }

        items.Add(item);
      }

      return items.ToString();
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(ToXML());
    }
  }
}
