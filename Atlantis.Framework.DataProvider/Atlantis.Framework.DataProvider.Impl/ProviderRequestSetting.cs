using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DataProvider.Impl
{
  internal enum ProviderRequestSettingType
  {
    None = 0,
    StoredProcedure = 1,
    WebService = 2,
    RestService = 3
  }

  internal class ProviderRequestSetting
  {
    public string RequestName;
    public string HostName;
    public string DSN;
    public string AppName;
    public string CertName;
    public string TargetName;
    public List<ProviderParameter> ParamList;
    public ProviderRequestSettingType RequestSettingType;

    public X509Certificate2 GetClientCertificate(string friendlyName)
    {
      X509Certificate2 clientCertificate = null;
      X509Store store = null;
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
            throw new AtlantisException("ProviderRequestSetting::GetClientCertificate", "0", string.Format("Unable to find Client Certificate '{0}' in cert store.", friendlyName), string.Empty, null, null);
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
        throw new AtlantisException("ProviderRequestSetting::GetClientCertificate", "0", "Unable to find Client Certificate config key.", string.Format("configKey: '{0}'", friendlyName), null, null);
      }
      return clientCertificate;
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
  }
}
