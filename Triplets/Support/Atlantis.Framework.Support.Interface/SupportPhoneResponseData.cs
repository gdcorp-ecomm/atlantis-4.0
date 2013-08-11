using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Support.Interface
{
  public class SupportPhoneResponseData : IResponseData
  {
    private static readonly ISupportPhoneData _emptySupportPhoneData = new SupportPhoneData(string.Empty, false);

    private readonly AtlantisException _exception;

    private ISupportPhoneData _supportPhoneData;
    public ISupportPhoneData SupportPhoneData
    {
      get
      {
        if (_supportPhoneData == null)
        {
          _supportPhoneData = _emptySupportPhoneData;
        }

        return _supportPhoneData;
      }
    }

    private SupportPhoneResponseData(string responseXml, string countryCode)
    {
      ParseResponseXml(responseXml, countryCode);
    }

    private SupportPhoneResponseData(AtlantisException ex)
    {
      _exception = ex;
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
      XElement element = new XElement("SupportPhoneData");
      element.Add(new XAttribute("number", SupportPhoneData.Number));
      element.Add(new XAttribute("isInternational", SupportPhoneData.IsInternational));
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
          _supportPhoneData = new SupportPhoneData(nodeElement.GetAttribute("supportPhone"), !countryCode.Equals("us"));
        }
      }
      else
      {
        node = doc.SelectSingleNode("//item [@isActive='1' and @flagCode='us']");
        if (node != null)
        {
          XmlElement nodeElement = node as XmlElement;
          if (nodeElement != null)
          {
            _supportPhoneData = new SupportPhoneData(nodeElement.GetAttribute("supportPhone"), false);
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
