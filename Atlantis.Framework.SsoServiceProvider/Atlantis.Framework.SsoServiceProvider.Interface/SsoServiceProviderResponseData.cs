using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoServiceProvider.Interface
{
  public class SsoServiceProviderResponseData : IResponseData
  {
    AtlantisException _exception = null;

    public string ServiceProviderName { get; private set; }
    public string LoginReceive { get; private set; }
    public string LoginReceiveType { get; private set; }
    public string IdentityProviderName { get; private set; }
    public string ServiceProviderGroupName { get; private set; }
    public string RedirectLoginUrl { get; private set; }
    public string LogoutUrl { get; private set; }
    public string RedirectLogoutUrl { get; private set; }
    public string CertificateName { get; private set; }
    public SsoServiceProviderStatus Status { get; private set; }

    public SsoServiceProviderResponseData(string serviceProviderName, string serviceProviderXml)
    {
      Status = SsoServiceProviderStatus.NotFound;
      ServiceProviderName = serviceProviderName;
      XDocument serviceProviderDoc = XDocument.Parse(serviceProviderXml);

      XElement itemElement = null;
      foreach (XElement item in serviceProviderDoc.Descendants("item"))
      {
        itemElement = item;
        break;
      }

      if (itemElement != null)
      {
        LoginReceive = GetAttributeValue(itemElement, "loginReceive", string.Empty);
        LoginReceiveType = GetAttributeValue(itemElement, "loginReceiveType", string.Empty);
        IdentityProviderName = GetAttributeValue(itemElement, "identityProviderName", string.Empty);
        ServiceProviderGroupName = GetAttributeValue(itemElement, "serviceProviderGroupName", string.Empty);
        RedirectLoginUrl = GetAttributeValue(itemElement, "redirectLoginURL", string.Empty);
        LogoutUrl = GetAttributeValue(itemElement, "logoutURL", string.Empty);
        RedirectLogoutUrl = GetAttributeValue(itemElement, "redirectLogoutURL", string.Empty);
        CertificateName = GetAttributeValue(itemElement, "certificateName", string.Empty);

        string isRetiredValue = GetAttributeValue(itemElement, "isRetired", "0");
        Status = "1".Equals(isRetiredValue) ? SsoServiceProviderStatus.Retired : SsoServiceProviderStatus.Active;
      }
    }

    public SsoServiceProviderResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "SsoServiceProviderResponsData", ex.Message + ex.StackTrace, requestData.ToXML());
    }

    private string GetAttributeValue(XElement element, string name, string defaultValue)
    {
      string result = defaultValue;
      XAttribute attribute = element.Attribute(name);
      if (attribute != null)
      {
        result = attribute.Value;
      }
      return result;
    }

    public string ToXML()
    {
      XElement result = new XElement("SsoServiceProvider",
        new XAttribute("ServiceProviderName", ServiceProviderName),
        new XAttribute("Status", Status.ToString()),
        new XAttribute("loginReceive", LoginReceive ?? string.Empty),
        new XAttribute("loginReceiveType", LoginReceiveType ?? string.Empty),
        new XAttribute("identityProviderName", IdentityProviderName ?? string.Empty),
        new XAttribute("serviceProviderGroupName", ServiceProviderGroupName ?? string.Empty),
        new XAttribute("redirectLoginURL", RedirectLoginUrl ?? string.Empty),
        new XAttribute("logoutURL", LogoutUrl ?? string.Empty),
        new XAttribute("redirectLogoutURL", RedirectLogoutUrl ?? string.Empty),
        new XAttribute("certificateName", CertificateName ?? string.Empty)
        );
      return result.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
