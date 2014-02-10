using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsXmlRequestData : DotTypeFormsBaseRequestData
  {
    private const string CONTACT_TYPE_REGISTRANT = "registrant";
    private const string CONTACT_TYPE_ADMINISTRATIVE = "administrative";
    private const string CONTACT_TYPE_BILLING = "billing";
    private const string CONTACT_TYPE_TECHNICAL = "technical";

    public DotTypeFormsXmlRequestData(string formType, int tldId, string placement, string phase, string marketId, int contextId)
      : base(formType, tldId, placement, phase, marketId, contextId)
    {
      RequestTimeout = TimeSpan.FromSeconds(16);
    }

    public List<DotTypeFormContact> DotTypeFormContacts { get; set; }

    public string ToJson()
    {
      var jsonSearchData = new JObject(
        new JProperty("TLDId", TldId),
        new JProperty("Placement", Placement),
        new JProperty("Phase", Phase),
        new JProperty("MarketId", MarketId),
        new JProperty("FormType", FormType)
        );

      if (DotTypeFormContacts != null && DotTypeFormContacts.Any())
      {
        var jContacts = new JProperty("Contacts");
        var jArray = new JArray();

        foreach (var contact in DotTypeFormContacts)
        {
          var jObject = new JObject();
          var jContactsPropertyList = new List<JProperty>();

          string strContactType = string.Empty;
          switch (contact.ContactType)
          {
            case DotTypeFormContactTypes.Registrant:
              strContactType = CONTACT_TYPE_REGISTRANT;
              break;
            case DotTypeFormContactTypes.Administrative:
              strContactType = CONTACT_TYPE_ADMINISTRATIVE;
              break;
            case DotTypeFormContactTypes.Billing:
              strContactType = CONTACT_TYPE_BILLING;
              break;
            case DotTypeFormContactTypes.Technical:
              strContactType = CONTACT_TYPE_TECHNICAL;
              break;
          }

          if (!string.IsNullOrEmpty(strContactType))
          {
            jContactsPropertyList.Add(new JProperty("ContactType", strContactType));
          }

          jContactsPropertyList.Add(new JProperty("FirstName", contact.FirstName));
          jContactsPropertyList.Add(new JProperty("LastName", contact.LastName));
          jContactsPropertyList.Add(new JProperty("Org", contact.Company  ));
          jContactsPropertyList.Add(new JProperty("Address1", contact.Address1));
          jContactsPropertyList.Add(new JProperty("Address2", contact.Address2));
          jContactsPropertyList.Add(new JProperty("City", contact.City));
          jContactsPropertyList.Add(new JProperty("State", contact.State));
          jContactsPropertyList.Add(new JProperty("CC", contact.CountryCode));
          jContactsPropertyList.Add(new JProperty("Phone", contact.Phone));
          jContactsPropertyList.Add(new JProperty("Fax", contact.Fax));

          jObject.Add(jContactsPropertyList);
          jArray.Add(jObject);
        }

        jContacts.Value = jArray;
        jsonSearchData.Add(jContacts);
      }

      return jsonSearchData.ToString();
    }

    public override string ToXML()
    {
      var element = new XElement("parameters");
      element.Add(new XAttribute("formtype", FormType.ToString(CultureInfo.InvariantCulture)));
      element.Add(new XAttribute("tldid", TldId.ToString(CultureInfo.InvariantCulture)));
      element.Add(new XAttribute("placement", Placement));
      element.Add(new XAttribute("phase", Phase));
      element.Add(new XAttribute("marketId", MarketId));
      element.Add(new XAttribute("contextid", ContextId.ToString(CultureInfo.InvariantCulture)));

      if (DotTypeFormContacts != null && DotTypeFormContacts.Any())
      {
        var contactsElement = new XElement("contacts");
        foreach (var contact in DotTypeFormContacts)
        {
          contactsElement.Add(new XAttribute("contacttype", contact.ContactType.ToString()));
          contactsElement.Add(new XAttribute("firstname", contact.FirstName));
          contactsElement.Add(new XAttribute("lastname", contact.LastName));
          contactsElement.Add(new XAttribute("company", contact.Company));
          contactsElement.Add(new XAttribute("address1", contact.Address1));
          contactsElement.Add(new XAttribute("address2", contact.Address2));
          contactsElement.Add(new XAttribute("city", contact.City));
          contactsElement.Add(new XAttribute("state", contact.State));
          contactsElement.Add(new XAttribute("zip", contact.Zip));
          contactsElement.Add(new XAttribute("country", contact.CountryCode));
          contactsElement.Add(new XAttribute("phone", contact.Phone));
          contactsElement.Add(new XAttribute("fax", contact.Fax));
          contactsElement.Add(new XAttribute("email", contact.Email));

          element.Add(contactsElement);
        }
      }

      return element.ToString();
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(ToXML());
    }
  }
}
