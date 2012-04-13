using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Atlantis.Framework.Interface
{
  public class WsConfigElement : ConfigElement
  {
    private string _webServiceUrl;
    public string WSURL
    {
      get { return _webServiceUrl; }
    }

    public WsConfigElement(string progId, string assembly, bool LPC, string webServiceUrl)
      : base(progId, assembly, LPC)
    {
      _webServiceUrl = webServiceUrl;
    }

    public WsConfigElement(string progId, string assembly, bool LPC, string webServiceUrl, Dictionary<string, string> configValues)
      : base(progId, assembly, LPC, configValues)
    {
      _webServiceUrl = webServiceUrl;
    }

    private static bool IsCertificateExpired(X509Certificate2 certificate)
    {
      bool isExpired = false;

      DateTime expirationDate = DateTime.Parse(certificate.GetExpirationDateString());

      if (expirationDate < DateTime.Now)
      {
        isExpired = true;
      }

      return isExpired;
    }

    /// <summary>
    /// Retrieves the friendly name to look up from a ConfigValue element with a key of "ClientCertificateName"
    /// </summary>
    /// <returns></returns>
    public X509Certificate2 GetClientCertificate()
    {
      return GetClientCertificate(GetConfigValue("ClientCertificateName"));
    }    

    public X509Certificate2 GetClientCertificate(string friendlyName)
    {
      X509Certificate2 clientCertificate = null;
      X509Store store = null;
      string certName = GetConfigValue(friendlyName);
      if (!string.IsNullOrEmpty(certName))
      {
        try
        {
          store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
          store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

          foreach (X509Certificate2 certificate in store.Certificates)
          {
            if (certificate.FriendlyName.Equals(certName, StringComparison.CurrentCultureIgnoreCase) &&
                !IsCertificateExpired(certificate))
            {
              clientCertificate = certificate;
              break;
            }
          }
        }
        finally
        {
          if (store != null)
          {
            store.Close();
          }
        }
      }

      return clientCertificate;
    }
  }
}
