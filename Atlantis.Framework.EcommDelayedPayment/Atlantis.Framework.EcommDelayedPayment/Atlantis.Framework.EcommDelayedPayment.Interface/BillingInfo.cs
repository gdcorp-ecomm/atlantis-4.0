using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.EcommDelayedPayment.Interface
{
  public class BillingInfo
  {
    public string City { get; set; }
    public string Company { get; set; }
    public string Country { get; set; }
    public string Email { get; set; }
    public string Fax { get; set; }
    public string First_Name { get; set; }
    public string Last_Name { get; set; }
    public string Middle_Name { get; set; }
    public string Phone1 { get; set; }
    public string Phone2 { get; set; }
    public string State { get; set; }
    public string Street1 { get; set; }
    public string Street2 { get; set; }
    public string Zip { get; set; }

    private void AddAttribute(string attributeName, string value,XmlTextWriter xmlWriter)
    {
      if (!string.IsNullOrEmpty(value))
      {
        xmlWriter.WriteAttributeString(attributeName, value);
      }
    }

    public void ToXML(XmlTextWriter xtwRequest)
    {
      xtwRequest.WriteStartElement("BillingInfo");
      AddAttribute("city", City, xtwRequest);
      AddAttribute("company", Company, xtwRequest);
      AddAttribute("country", Country, xtwRequest);
      AddAttribute("email", Email, xtwRequest);
      AddAttribute("fax", Fax, xtwRequest);
      AddAttribute("first_name", First_Name, xtwRequest);
      AddAttribute("middle_name", Middle_Name, xtwRequest);
      AddAttribute("last_name", Last_Name, xtwRequest);
      AddAttribute("phone1", Phone1, xtwRequest);
      AddAttribute("phone2", Phone2, xtwRequest);
      AddAttribute("state", State, xtwRequest);
      AddAttribute("street1", Street1, xtwRequest);
      AddAttribute("street2", Street2, xtwRequest);
      AddAttribute("zip", Zip, xtwRequest);
      xtwRequest.WriteEndElement();
    }
  }
}
