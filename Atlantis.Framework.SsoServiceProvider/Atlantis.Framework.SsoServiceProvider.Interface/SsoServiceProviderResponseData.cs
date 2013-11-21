using System;
using System.Linq;
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
    public bool IsUsingPrimaryServiceProviderName { get; private set; }
    public string PrimaryServiceProviderName { get; private set; }

    public SsoServiceProviderResponseData(string serviceProviderName, string serviceProviderXml)
    {
      SetDefaultValuesForAllProperties();
      Status = SsoServiceProviderStatus.NotFound;
      ServiceProviderName = serviceProviderName;

      if (!string.IsNullOrEmpty(serviceProviderXml))
      {
        XDocument serviceProviderDoc = XDocument.Parse(serviceProviderXml);

        XElement itemElement = serviceProviderDoc.Descendants("item").FirstOrDefault();
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

          string serviceProviderNameReturn = GetAttributeValue(itemElement, "serviceProviderName", string.Empty);
          PrimaryServiceProviderName = string.Empty;

          if (!string.IsNullOrEmpty(serviceProviderNameReturn))
          {
            IsUsingPrimaryServiceProviderName = !serviceProviderNameReturn.Equals(serviceProviderName, StringComparison.OrdinalIgnoreCase);
            if (IsUsingPrimaryServiceProviderName)
            {
              PrimaryServiceProviderName = serviceProviderNameReturn;
            }
          }

        }
      }

    }

    public SsoServiceProviderResponseData(RequestData requestData, Exception ex)
    {
      SetDefaultValuesForAllProperties();
      _exception = new AtlantisException(requestData, "SsoServiceProviderResponsData", ex.Message + ex.StackTrace, requestData.ToXML());
    }

    public string ToXML()
    {
      XElement result = new XElement("SsoServiceProvider",
        new XAttribute("ServiceProviderName", ServiceProviderName ?? string.Empty),
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

    private static string GetAttributeValue(XElement element, string name, string defaultValue)
    {
      string result = defaultValue;
      XAttribute attribute = element.Attribute(name);
      if (attribute != null)
      {
        result = attribute.Value;
      }
      return result;
    }
    
    private void SetDefaultValuesForAllProperties()
    {
      ServiceProviderName = string.Empty;
      LoginReceive = string.Empty;
      LoginReceiveType = string.Empty;
      IdentityProviderName = string.Empty;
      ServiceProviderGroupName = string.Empty;
      RedirectLoginUrl = string.Empty;
      RedirectLogoutUrl = string.Empty;
      LogoutUrl = string.Empty;
      CertificateName = string.Empty;
      Status = SsoServiceProviderStatus.NotFound;
      IsUsingPrimaryServiceProviderName = false;
      PrimaryServiceProviderName = string.Empty;
    }
  }
}
