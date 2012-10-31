using System;
using System.Net;
using System.Net.Sockets;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal static class ErrorLogHelper
  {
    internal static string ClientIp
    {
      get
      {
        string clientIp = "127.0.0.1";

        if(HttpContext.Current == null)
        {
          IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
          foreach (IPAddress ip in host.AddressList)
          {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
              clientIp = ip.ToString();
              break;
            }
          }
        }
        else
        {
          clientIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
          if (!string.IsNullOrEmpty(clientIp))
          {
            string[] ipRange = clientIp.Split(',');
            if (ipRange.Length > 0)
            {
              clientIp = ipRange[0].Trim();
            }
          }

          if (string.IsNullOrEmpty(clientIp))
          {
            clientIp = HttpContext.Current.Request.UserHostAddress;
          }
        }

        return clientIp;
      }
    }

    internal static void LogError(Exception ex, string sourceFunction)
    {
      try
      {
        Engine.Engine.LogAtlantisException(new AtlantisException(sourceFunction, HttpContext.Current != null ? HttpContext.Current.Request.Url.ToString() : "http://templateplaceholder.godaddy.com/", "0", ex.Message, ex.StackTrace, string.Empty, string.Empty, ClientIp, Guid.Empty.ToString(), 1));  
      }
      catch
      {
      }
    }
  }
}
