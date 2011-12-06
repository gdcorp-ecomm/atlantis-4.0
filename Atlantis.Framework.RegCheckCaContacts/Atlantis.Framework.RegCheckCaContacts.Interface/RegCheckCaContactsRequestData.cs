using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegCheckCaContacts.Interface
{
  public class RegCheckCaContactsRequestData : RequestData
  {
    private CaContactProperties _caContactProperties;

    public RegCheckCaContactsRequestData(
      string shopperID,
      string sourceURL,
      string orderID,
      string pathway,
      int pageCount,
      CaContactProperties caContactProperties)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(4);
      this._caContactProperties = caContactProperties;
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      return string.Empty;
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("contact");

      xtwRequest.WriteAttributeString("cadomainname", _caContactProperties.CaDomainName);
      xtwRequest.WriteAttributeString("adminfname", _caContactProperties.AdminFirstName);
      xtwRequest.WriteAttributeString("adminlname", _caContactProperties.AdminLastName);
      xtwRequest.WriteAttributeString("org", _caContactProperties.Organization);
      xtwRequest.WriteAttributeString("adminsa1", _caContactProperties.AdminAddress1);
      xtwRequest.WriteAttributeString("adminsa2", _caContactProperties.AdminAddress2);
      xtwRequest.WriteAttributeString("admincity", _caContactProperties.AdminCity);
      xtwRequest.WriteAttributeString("adminsp", _caContactProperties.AdminState);
      xtwRequest.WriteAttributeString("adminpc", _caContactProperties.AdminZip);
      xtwRequest.WriteAttributeString("admincc", _caContactProperties.AdminCountry);
      xtwRequest.WriteAttributeString("adminphone", _caContactProperties.AdminPhone);
      xtwRequest.WriteAttributeString("adminfax", _caContactProperties.AdminFax);
      xtwRequest.WriteAttributeString("adminemail", _caContactProperties.AdminEmail);
      xtwRequest.WriteAttributeString("techfname", _caContactProperties.TechFirstName);
      xtwRequest.WriteAttributeString("techlname", _caContactProperties.TechLastName);
      xtwRequest.WriteAttributeString("techsa1", _caContactProperties.TechAddress1);
      xtwRequest.WriteAttributeString("techsa2", _caContactProperties.TechAddress2);
      xtwRequest.WriteAttributeString("techcity", _caContactProperties.TechCity);
      xtwRequest.WriteAttributeString("techsp", _caContactProperties.TechState);
      xtwRequest.WriteAttributeString("techpc", _caContactProperties.TechZip);
      xtwRequest.WriteAttributeString("techcc", _caContactProperties.TechCountry);
      xtwRequest.WriteAttributeString("techphone", _caContactProperties.TechPhone);
      xtwRequest.WriteAttributeString("techfax", _caContactProperties.TechFax);
      xtwRequest.WriteAttributeString("techemail", _caContactProperties.TechEmail);
      xtwRequest.WriteAttributeString("caregistrantname", _caContactProperties.CaRegistrantName);
      xtwRequest.WriteAttributeString("calegaltype", _caContactProperties.CaLegalType);

      xtwRequest.WriteEndElement();

      return sbRequest.ToString();
    }

    #endregion
  }
}
