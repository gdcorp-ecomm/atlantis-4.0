using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Auth.Interface
{
  public static class AuthClientCertHelper
  {
    public static X509Certificate2 GetClientCertificate(ConfigElement config)
    {
      X509Certificate2 clientCertificate = null;
      X509Store store = null;

      string certificateName = config.GetConfigValue("CertificateName");

      if (!string.IsNullOrEmpty(certificateName))
      {
        try
        {
          store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
          store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

          foreach (X509Certificate2 certificate in store.Certificates)
          {
            if (certificate.FriendlyName.Equals(certificateName, StringComparison.CurrentCultureIgnoreCase))
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
