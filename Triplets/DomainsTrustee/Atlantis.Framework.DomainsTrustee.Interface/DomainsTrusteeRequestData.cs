using System;
using System.Collections.Generic;
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

    public IList<DomainsTrusteeContactsTlds> ContactsTldsList { get; set; }

    public string ToJson()
    {
      var finalArray = new JArray();

      foreach (DomainsTrusteeContactsTlds contactsDomains in ContactsTldsList)
      {
        var contactsDomainsObject = new JObject();

        var contacts = contactsDomains.Contacts;
        var tlds = contactsDomains.Tlds;

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

        foreach (var tld in tlds)
        {
          jDomainArray.Add(new JObject(
                                      new JProperty("Tld", tld)
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

      foreach (DomainsTrusteeContactsTlds cd in ContactsTldsList)
      {
        var item = new XElement("item");

        foreach (DomainsTrusteeContact contact in cd.Contacts)
        {
          var contactElement = new XElement("contact");

          contactElement.Add(new XAttribute("contactType", contact.ContactType.ToString()));
          contactElement.Add(new XAttribute("countryCode", contact.CountryCode));

          item.Add(contactElement);
        }

        foreach (var tld in cd.Tlds)
        {
          var domainElement = new XElement("tlds");

          domainElement.Add(new XAttribute("tld", tld));

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
