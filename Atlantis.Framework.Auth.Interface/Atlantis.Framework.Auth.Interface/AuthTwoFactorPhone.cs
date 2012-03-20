using System.Xml.Serialization;

namespace Atlantis.Framework.Auth.Interface
{
  [XmlRoot(ElementName = "Phone")]
  public class AuthTwoFactorPhone
  {
    [XmlAttribute(AttributeName = "number")]
    public string PhoneNumber { get; set; }

    [XmlAttribute(AttributeName = "carrier")]
    public string Carrier { get; set; }

    public AuthTwoFactorPhone()
    {
    }

    public AuthTwoFactorPhone(string xml)
    {
      if (!string.IsNullOrEmpty(xml))
      {
        XmlSerializer xmlSerializer = new XmlSerializer();

        try
        {
          AuthTwoFactorPhone authTwoFactorPhone = xmlSerializer.Deserialize<AuthTwoFactorPhone>(xml);
          PhoneNumber = authTwoFactorPhone.PhoneNumber;
          Carrier = authTwoFactorPhone.Carrier;
        }
        catch
        {
          PhoneNumber = string.Empty;
          Carrier = string.Empty;
        }
      }
    }

    public string ToXml()
    {
      string xml;
      XmlSerializer xmlSerializer = new XmlSerializer();

      try
      {
        xml = xmlSerializer.Serialize(this);
      }
      catch
      {
        xml = string.Empty;
      }

      return xml;
    }
  }
}
