using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobilePushShopper.Impl
{
  public static class ShopperMobilePushServiceClient
  {
    private static X509Certificate2 GetClientCertificate(string certificateName)
    {
      X509Certificate2 clientCertificate = null;
      X509Store store = null;

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

    public static ShopperMobilePushService.Service1 GetWebServiceInstance(WsConfigElement config, TimeSpan requestTimeout)
    {
      ShopperMobilePushService.Service1 shopperMobilePushService = new ShopperMobilePushService.Service1();
      shopperMobilePushService.Timeout = (int)requestTimeout.TotalMilliseconds;
      shopperMobilePushService.Url = config.WSURL;

      X509Certificate2 clientCertificate = GetClientCertificate(config.GetConfigValue("CertificateName"));
      if (clientCertificate == null)
      {
        throw new Exception("Certificate not found.");
      }

      shopperMobilePushService.ClientCertificates.Add(clientCertificate);

      return shopperMobilePushService;
    }
  }
}
