using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Support.Interface
{
  public class SupportPhoneResponseData : IResponseData
  {
    private AtlantisException _exception;

    public SupportPhone SupportPhone {get; private set;}

    private SupportPhoneResponseData(string responseXml, string countryCode)
    {
      ParseResponseXml(responseXml, countryCode);
    }

    private SupportPhoneResponseData(AtlantisException ex)
    {
      _exception = ex;
      SupportPhone = null;
    }

    public static SupportPhoneResponseData FromException(AtlantisException ex)
    {
      return new SupportPhoneResponseData(ex);
    }

    public static SupportPhoneResponseData FromResponseXml(string responseXml, string countryCode)
    {
      return new SupportPhoneResponseData(responseXml, countryCode);
    }

    public string ToXML()
    {
      XElement element = new XElement("SupportPhone");
      element.Add(new XAttribute("supportPhone", SupportPhone.TechnicalSupportPhone));
      element.Add(new XAttribute("isInternational", SupportPhone.SupportPhoneIsInternational));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    private void ParseResponseXml(string responseXml, string countryCode)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(responseXml);
      XmlNode node;

      node = doc.SelectSingleNode("//item [@isActive='1' and @flagCode='" + countryCode + "']");
      if (node != null)
      {
        XmlElement nodeElement = node as XmlElement;
        if (nodeElement != null)
        {
          SupportPhone = new SupportPhone(nodeElement.GetAttribute("supportPhone"), !countryCode.Equals("us"));
        }
      }
      else
      {
        node = doc.SelectSingleNode("//item [@isActive='1' and @flagCode='" + "us" + "']");
        if (node != null)
        {
          XmlElement nodeElement = node as XmlElement;
          if (nodeElement != null)
          {
            SupportPhone = new SupportPhone(nodeElement.GetAttribute("supportPhone"), false);
          }
        }
      }
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
