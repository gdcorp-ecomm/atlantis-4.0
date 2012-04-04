using System.Xml.Serialization;

namespace Atlantis.Framework.Auth.Interface
{
  [XmlRoot(ElementName = "Phone")]
  public class AuthTwoFactorPhone
  {
    [XmlIgnore]
    private int _statusCode = -1;

    [XmlAttribute(AttributeName = "cc")]
    public string CountryCode { get; set; }

    [XmlAttribute(AttributeName = "number")]
    public string PhoneNumber { get; set; }

    [XmlAttribute(AttributeName = "carrier")]
    public string CarrierId { get; set; }

    [XmlAttribute(AttributeName = "status")]
    public string StatusCodeString
    {
      get
      {
        string statusCode = null;
        if(_statusCode > 0)
        {
          statusCode = _statusCode.ToString();
        }
        return statusCode;
      }
      set { _statusCode = int.Parse(value); }
    }

    [XmlIgnore]
    public int StatusCode
    {
      get { return _statusCode; }
      set { _statusCode = value; }
    }

    public AuthTwoFactorPhone()
    {
    }

    public AuthTwoFactorPhone(string countryCode, string phoneNumber, string carrierId) : this(countryCode, phoneNumber, carrierId, -1)
    {
    }

    public AuthTwoFactorPhone(string countryCode, string phoneNumber, string carrierId, int statusCode)
    {
      CountryCode = countryCode;
      PhoneNumber = phoneNumber;
      CarrierId = carrierId;
      StatusCode = statusCode;
    }

    public AuthTwoFactorPhone(string xml)
    {
      if (!string.IsNullOrEmpty(xml))
      {
        XmlSerializer xmlSerializer = new XmlSerializer();

        try
        {
          AuthTwoFactorPhone authTwoFactorPhone = xmlSerializer.Deserialize<AuthTwoFactorPhone>(xml);
          CountryCode = authTwoFactorPhone.CountryCode;
          PhoneNumber = authTwoFactorPhone.PhoneNumber;
          CarrierId = authTwoFactorPhone.CarrierId;
          StatusCode = authTwoFactorPhone.StatusCode;
        }
        catch
        {
          CountryCode = string.Empty;
          PhoneNumber = string.Empty;
          CarrierId = string.Empty;
          StatusCode = -1;
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
