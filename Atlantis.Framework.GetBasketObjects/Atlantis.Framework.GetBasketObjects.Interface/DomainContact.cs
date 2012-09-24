using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.GetBasketObjects.Interface
{
  public class DomainContact : CartBaseDictionary
  {

    public string ContactType
    {
      get { return GetStringProperty("contactType", string.Empty); }
    }

    public string FirstName
    {
      get { return GetStringProperty("fname", string.Empty); }
    }

    public string LastName
    {
      get { return GetStringProperty("lname", string.Empty); }
    }

    public string Email
    {
      get { return GetStringProperty("email", string.Empty); }
    }

    public string Org
    {
      get { return GetStringProperty("org", string.Empty); }
    }

    public string StreetAddress1
    {
      get { return GetStringProperty("sa1", string.Empty); }
    }

    public string StreetAddress2
    {
      get { return GetStringProperty("sa2", string.Empty); }
    }

    public string City
    {
      get { return GetStringProperty("city", string.Empty); }
    }

    public string Phone
    {
      get { return GetStringProperty("phone", string.Empty); }
    }

    public DomainContact()
    {
    }

    public DomainContact(XmlNode currentContact)
    {
      foreach (XmlAttribute regAtt in currentContact.Attributes)
      {
        this[regAtt.Name] = regAtt.Value;
      }
    }

  }

  
}
