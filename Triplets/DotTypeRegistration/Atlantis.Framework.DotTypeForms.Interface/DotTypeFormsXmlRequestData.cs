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
          jContactsPropertyList.Add(new JProperty("Zip", contact.Zip));
          jContactsPropertyList.Add(new JProperty("CC", contact.CountryCode));
          jContactsPropertyList.Add(new JProperty("Phone", contact.Phone));
          jContactsPropertyList.Add(new JProperty("Fax", contact.Fax));
          jContactsPropertyList.Add(new JProperty("Email", contact.Email));

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
          var contactElement = new XElement("contact");
          contactElement.Add(new XAttribute("contacttype", contact.ContactType.ToString()));
          contactElement.Add(new XAttribute("firstname", contact.FirstName));
          contactElement.Add(new XAttribute("lastname", contact.LastName));
          contactElement.Add(new XAttribute("company", contact.Company));
          contactElement.Add(new XAttribute("address1", contact.Address1));
          contactElement.Add(new XAttribute("address2", contact.Address2));
          contactElement.Add(new XAttribute("city", contact.City));
          contactElement.Add(new XAttribute("state", contact.State));
          contactElement.Add(new XAttribute("zip", contact.Zip));
          contactElement.Add(new XAttribute("country", contact.CountryCode));
          contactElement.Add(new XAttribute("phone", contact.Phone));
          contactElement.Add(new XAttribute("fax", contact.Fax));
          contactElement.Add(new XAttribute("email", contact.Email));

          contactsElement.Add(contactElement);
        }
        element.Add(contactsElement);
      }

      return element.ToString();
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(ToXML());
    }
  }
}
