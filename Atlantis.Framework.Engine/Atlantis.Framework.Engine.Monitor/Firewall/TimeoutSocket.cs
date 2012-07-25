using System;
using System.Threading;
using System.Net.Sockets;

namespace Atlantis.Framework.Engine.Monitor.Firewall
{
  internal class TimeOutSocket
  {
    private static bool _isConnectionSuccessful = false;
    private static Exception _socketexception;
    private static ManualResetEvent _timeoutObject = new ManualResetEvent(false);

    public static TcpClient Connect(string host, int port, TimeSpan timeout)
    {
      _timeoutObject.Reset();
      _socketexception = null;

      TcpClient tcpclient = new TcpClient();

      tcpclient.BeginConnect(host, port, new AsyncCallback(CallBackMethod), tcpclient);

      if (_timeoutObject.WaitOne((int)timeout.TotalMilliseconds, false))
      {
        if (_isConnectionSuccessful)
        {
          return tcpclient;
        }
        else
        {
          throw _socketexception;
        }
      }
      else
      {
        tcpclient.Close();
        throw new TimeoutException("TimeOut Exception");
      }
    }

    private static void CallBackMethod(IAsyncResult asyncresult)
    {
      try
      {
        _isConnectionSuccessful = false;
        TcpClient tcpclient = asyncresult.AsyncState as TcpClient;

        if (tcpclient.Client != null)
        {
          tcpclient.EndConnect(asyncresult);
          _isConnectionSuccessful = true;
        }
      }
      catch (Exception ex)
      {
        _isConnectionSuccessful = false;
        _socketexception = ex;
      }
      finally
      {
        _timeoutObject.Set();
      }
    }
  }
}
