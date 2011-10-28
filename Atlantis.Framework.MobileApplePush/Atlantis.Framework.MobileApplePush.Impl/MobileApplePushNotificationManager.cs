using System;
using System.Security.Cryptography.X509Certificates;

namespace Atlantis.Framework.MobileApplePush.Impl
{
  internal class MobileApplePushNotificationManager
  {
    private static readonly object _lockSync = new object();
    private static volatile MobileApplePushNotificationConnection _connection;

    /// <summary>
    /// Gets a singleton instance of MobileApplePushNotificationConnection. Eventually, we could implement a connection pool if we run into performance issues.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="certificate"></param>
    /// <param name="requestTimeout"></param>
    /// <param name="connectionDuration"></param>
    /// <param name="connectionOpenRetryCount"></param>
    /// <returns></returns>
    public static MobileApplePushNotificationConnection GetConnectionInstance(string host, int port, X509Certificate certificate, TimeSpan requestTimeout, TimeSpan connectionDuration, int connectionOpenRetryCount)
    {
      if (_connection == null)
      {
        lock (_lockSync)
        {
          if (_connection == null)
          {
            _connection = new MobileApplePushNotificationConnection(host, port, certificate, requestTimeout, connectionDuration, connectionOpenRetryCount);
          }
        }
      }

      return _connection;
    }
  }
}
