using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Atlantis.Framework.MobileApplePush.Impl
{
  internal class MobileApplePushNotificationConnection
  {
    private const int MAX_CONNECTION_OPEN_RETRY = 10;

    private TcpClient _apnsClient;
    private SslStream _apnsStream;

    private readonly string _host;
    private readonly int _port;
    private readonly X509Certificate _certificate;
    private readonly TimeSpan _requestTimeout;
    private readonly long _connectionDurationTicks;
    private readonly int _connectionOpenRetryCount;

    private bool IsConnected
    {
      get
      {
        return !IsConnectionExpired &&
               IsClientConnected &&
               IsStreamOpened;
      }
    }

    private bool IsConnectionExpired
    {
      get { return DateTime.UtcNow.Ticks > _connectionDurationTicks; }
    }

    private bool IsClientConnected
    {
      get { return _apnsClient != null && _apnsClient.Connected; }
    }

    private bool IsStreamOpened
    {
      get { return _apnsStream != null && _apnsStream.IsMutuallyAuthenticated && _apnsStream.CanWrite; }
    }

    internal Exception CurrentException { get; private set; }

    internal MobileApplePushNotificationConnection(string host, int port, X509Certificate certificate, TimeSpan requestTimeout, TimeSpan connectionDuration, int connectionOpenRetryCount)
    {
      _host = host;
      _port = port;
      _certificate = certificate;
      _requestTimeout = requestTimeout;
      _connectionDurationTicks = DateTime.UtcNow.Add(connectionDuration).Ticks;
      _connectionOpenRetryCount = connectionOpenRetryCount > 0 ? connectionOpenRetryCount : 0;
      if (_connectionOpenRetryCount > MAX_CONNECTION_OPEN_RETRY)
      {
        _connectionOpenRetryCount = MAX_CONNECTION_OPEN_RETRY;
      }
    }

    /// <summary>
    /// This is NOT thread safe, make sure to get a lock before calling this method
    /// </summary>
    /// <returns></returns>
    private bool OpenConnection()
    {
      bool success = false;

      try
      {
        // Dispose of the current connection if there is one
        Dispose();

        _apnsClient = new TcpClient();
        _apnsClient.SendTimeout = (int)_requestTimeout.TotalMilliseconds;
        _apnsClient.Connect(_host, _port);
        _apnsStream = new SslStream(_apnsClient.GetStream(), false);
        _apnsStream.AuthenticateAsClient(_host, new X509CertificateCollection { _certificate }, SslProtocols.Ssl3, false);

        success = _apnsStream.IsMutuallyAuthenticated && _apnsStream.CanWrite;
      }
      catch (Exception ex)
      {
        CurrentException = ex;
        Dispose();
      }
      return success;
    }


    /// <summary>
    /// This is NOT thread safe, make sure to get a lock before calling this method
    /// </summary>
    /// <param name="notificationBytes"></param>
    /// <returns></returns>
    private bool WriteBytesToStream(byte[] notificationBytes)
    {
      bool success = false;

      try
      {
        _apnsStream.Write(notificationBytes);
        success = true;
      }
      catch (Exception ex)
      {
        CurrentException = ex;
      }

      return success;
    }

    /// <summary>
    /// This is NOT thread safe, make sure to get a lock before calling this method
    /// </summary>
    /// <returns></returns>
    private void Dispose()
    {
      if (_apnsStream != null)
      {
        _apnsStream.Close();
        _apnsStream.Dispose();
      }

      if (_apnsClient != null)
      {
        _apnsClient.Client.Shutdown(SocketShutdown.Both);
        _apnsClient.Client.Close();
        _apnsClient.Close();
      }
    }

    internal bool SendNotification(byte[] notificationBytes)
    {
      bool success = false;

      using(new MonitorLock(this))
      {
        if (!IsConnected)
        {
          bool opened = OpenConnection();
          if (!opened && _connectionOpenRetryCount > 0)
          {
            for (int i = 0; i < _connectionOpenRetryCount; i++)
            {
              Thread.Sleep(100);
              opened = OpenConnection();
              if (opened)
              {
                break;
              }
            }
          }

          if (!IsConnected)
          {
            if (CurrentException == null)
            {
              CurrentException = new Exception(string.Format("Unable to open a connection after {0} re-tries. No exception thrown.", _connectionOpenRetryCount));
            }
          }
        }

        if(IsConnected)
        {
          success = WriteBytesToStream(notificationBytes);
        }
      }

      return success;
    }
  }
}
