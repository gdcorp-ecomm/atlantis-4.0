using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MobileApplePush.Interface;

namespace Atlantis.Framework.MobileApplePush.Impl
{
  public class MobileApplePushRequest : IRequest
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

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;

      MobileApplePushRequestData mobileApplePushRequestData = (MobileApplePushRequestData)requestData;
      WsConfigElement wsConfig = (WsConfigElement)config;

      string host = wsConfig.WSURL;
      int port = int.Parse(wsConfig.GetConfigValue("Port"));
      X509Certificate2 clientCertificate = GetClientCertificate(wsConfig.GetConfigValue("CertificateName"));
      TimeSpan connectionDuration = TimeSpan.FromSeconds(int.Parse(wsConfig.GetConfigValue("ConnectionDurationSeconds")));
      int connectionOpenRetryCount = int.Parse(wsConfig.GetConfigValue("ConnectionOpenRetryCount"));

      try
      {
        if (!mobileApplePushRequestData.Notification.IsValidDeviceToken)
        {
          throw new Exception(string.Format("Invalid device token {0}", mobileApplePushRequestData.Notification.DeviceToken));
        }
        else
        {
          byte[] notificationBytes;
          string errorMessage;
          if (!mobileApplePushRequestData.Notification.GetBytes(out notificationBytes, out errorMessage))
          {
            throw new Exception(errorMessage);
          }
          else
          {
            MobileApplePushNotificationConnection connection = MobileApplePushNotificationManager.GetConnectionInstance(host, port, clientCertificate, mobileApplePushRequestData.RequestTimeout, connectionDuration, connectionOpenRetryCount);
            if (connection.SendNotification(notificationBytes))
            {
              responseData = new MobileApplePushResponseData(true);
            }
            else
            {
              if (connection.CurrentException != null)
              {
                throw connection.CurrentException;
              }
              else
              {
                throw new Exception("Error occured during send. No exception thrown.");
              }
            }
          }
        }

      }
      catch (Exception ex)
      {
        responseData = new MobileApplePushResponseData(mobileApplePushRequestData, ex);
      }

      return responseData;
    }
  }
}
