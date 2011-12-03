using System;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BasePages.SiteAdmin.Providers
{
  internal static class RequestHelper
  {
    internal static bool IsRequestInternal()
    {
      bool result = false;
      string[] ipsplits = HttpContext.Current.Request.UserHostAddress.Split('.');
      if (ipsplits.Length == 4)
      {
        if ((HttpContext.Current.Request.UserHostAddress == "127.0.0.1") ||
          (ipsplits[0] == "10") ||
          ((ipsplits[0] == "192") && (ipsplits[1] == "168")))
        {
          result = true;
        }
        else if (ipsplits[0] == "172")
        {
          int second = Convert.ToInt32(ipsplits[1]);
          if ((second >= 16) && (second <= 31))
            result = true;
        }
      }
      return result;
    }

    internal static ServerLocationType GetServerLocation(bool isRequestInternal)
    {
      ServerLocationType result = ServerLocationType.Prod;

      string hostName = HttpContext.Current.Request.Url.Host.ToLowerInvariant();

      if (hostName.Contains(".ote."))
      {
        result = ServerLocationType.Ote;
      }
      else if (isRequestInternal)
      {
        if (hostName.EndsWith(".ide"))
        {
          if (hostName.Contains(".test."))
          {
            result = ServerLocationType.Test;
          }
          else if (hostName.Contains(".dev.") || hostName.Contains(".debug."))
          {
            result = ServerLocationType.Dev;
          }
        }
        else if (hostName.StartsWith("localhost"))
        {
          result = ServerLocationType.Dev;
        }
      }

      return result;
    }
  }

}
