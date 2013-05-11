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

    public WsConfigElement(int requestType, string progId, string assembly, string webServiceUrl)
      : base(requestType, progId, assembly)
    {
      _webServiceUrl = webServiceUrl;
    }

    public WsConfigElement(int requestType, string progId, string assembly, string webServiceUrl, Dictionary<string, string> configValues)
      : base(requestType, progId, assembly, configValues)
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
      return GetClientCertificate("ClientCertificateName");
    }    

    public X509Certificate2 GetClientCertificate(string configKey)
    {
      X509Certificate2 clientCertificate = null;
      X509Store store = null;
      string friendlyName = GetConfigValue(configKey);
      if (!string.IsNullOrEmpty(friendlyName))
      {
        try
        {
          store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
          store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

          foreach (X509Certificate2 certificate in store.Certificates)
          {
            if (certificate.FriendlyName.Equals(friendlyName, StringComparison.CurrentCultureIgnoreCase) &&
                !IsCertificateExpired(certificate))
            {
              clientCertificate = certificate;
              break;
            }
          }
          if (clientCertificate == null)
          {
            throw new AtlantisException("WsConfigElement::GetClientCertificate", "0", string.Format("Unable to find Client Certificate '{0}' in cert store.", friendlyName), string.Empty, null, null);
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
      else
      {
        throw new AtlantisException("WsConfigElement::GetClientCertificate", "0", "Unable to find Client Certificate config key.", string.Format("configKey: '{0}'", configKey), null, null);
      }
      return clientCertificate;
    }
  }
}
