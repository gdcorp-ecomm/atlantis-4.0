using System;
using System.Web;
using Atlantis.Framework.Interface;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atlantis.Framework.BasePages.Providers
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
      return result && !IsInternalRequestExcluded();
    }

    private static bool IsInternalRequestExcluded()
    {
      bool returnValue = false;

      List<string> excludedIPs = new List<string>(DataCache.DataCache.GetAppSetting("ATLANTIS_INTERNALEXCLUDEDIPADDRESSES").Split('|'));
      List<string> excludedUserAgents = new List<string>(DataCache.DataCache.GetAppSetting("ATLANTIS_INTERNALEXCLUDEDUSERAGENTS").ToLower().Split('|'));
      
      string userAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];

      if (!String.IsNullOrEmpty(userAgent))
      {
        excludedUserAgents.ForEach(delegate(String agent)
        {
          if (HttpContext.Current.Request.UserAgent.ToLower().Contains(agent))
          {
            returnValue = true;
          }
        });
      }

      return returnValue || (excludedIPs.Contains(HttpContext.Current.Request.UserHostAddress));
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
            if (WebConfigurationManager.AppSettings["EnvironmentOverride"] == "Test")
            {
              result = ServerLocationType.Test;
            }
            else
            {
              result = ServerLocationType.Dev;
            }
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
