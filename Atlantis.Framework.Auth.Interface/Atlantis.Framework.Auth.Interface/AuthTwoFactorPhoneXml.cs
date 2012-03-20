using System.Xml.Serialization;
using XmlSerializer = Atlantis.Framework.Auth.Interface.Serializers.XmlSerializer;

namespace Atlantis.Framework.Auth.Interface
{
  [XmlRoot(ElementName = "Phone")]
  public class AuthTwoFactorPhoneXml
  {
    [XmlAttribute(AttributeName = "number")]
    public string PhoneNumber { get; set; }

    [XmlAttribute(AttributeName = "carrier")]
    public string Carrier { get; set; }

    public AuthTwoFactorPhoneXml()
    {
    }

    public AuthTwoFactorPhoneXml(string xml)
    {
      if (!string.IsNullOrEmpty(xml))
      {
        XmlSerializer xmlSerializer = new XmlSerializer();

        try
        {
          AuthTwoFactorPhoneXml authTwoFactorPhoneXml = xmlSerializer.Deserialize<AuthTwoFactorPhoneXml>(xml);
          PhoneNumber = authTwoFactorPhoneXml.PhoneNumber;
          Carrier = authTwoFactorPhoneXml.Carrier;
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
