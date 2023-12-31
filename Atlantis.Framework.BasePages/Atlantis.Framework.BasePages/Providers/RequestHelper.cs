﻿using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;

namespace Atlantis.Framework.BasePages.Providers
{
  internal static class RequestHelper
  {
    internal static IProxyContext GetProxyContext()
    {
      IProxyContext result = null;
      if (HttpProviderContainer.Instance.CanResolve<IProxyContext>())
      {
        result = HttpProviderContainer.Instance.Resolve<IProxyContext>();
      }
      return result;
    }

    internal static string GetOriginUserHostAddress()
    {
      string result = HttpContext.Current.Request.UserHostAddress;
      IProxyContext proxyContext = GetProxyContext();
      if (proxyContext != null)
      {
        result = proxyContext.OriginIP;
      }
      return result;
    }

    internal static string SafeUrlHost()
    {
      try
      {
        return HttpContext.Current.Request.Url.Host;
      }
      catch
      {
        return string.Empty;
      }
    }

    internal static string GetContextHost()
    {
      string result = SafeUrlHost();
      IProxyContext proxyContext = GetProxyContext();
      if (proxyContext != null)
      {
        result = proxyContext.ContextHost;
      }
      return result;
    }

    internal static bool IsRequestInternal()
    {
      IProxyContext proxyContext = GetProxyContext();
      if ((proxyContext != null) && (proxyContext.Status == ProxyStatusType.Invalid))
      {
        return false; // If the proxy status is invalid for any reason, treat as external
      }

      bool result = false;
      string originalHostAddress = GetOriginUserHostAddress();
      if (originalHostAddress != null)
      {
        string[] ipsplits = originalHostAddress.Split('.');
        if (ipsplits.Length == 4)
        {
          if ((originalHostAddress == "127.0.0.1") ||
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
      }
      return result && !IsInternalRequestExcluded(originalHostAddress);
    }

    private static bool IsInternalRequestExcluded(string userHostAddress)
    {
      bool returnValue = false;

      List<string> excludedIPs = new List<string>(DataCache.DataCache.GetAppSetting("ATLANTIS_INTERNALEXCLUDEDIPADDRESSES").Split('|'));
      List<string> excludedUserAgents = new List<string>(DataCache.DataCache.GetAppSetting("ATLANTIS_INTERNALEXCLUDEDUSERAGENTS").ToLower().Split('|'));

      string userAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];

      if (!String.IsNullOrEmpty(userAgent))
      {
        excludedUserAgents.ForEach(delegate(String agent)
        {
          if (userAgent.Contains(agent))
          {
            returnValue = true;
          }
        });
      }

      return returnValue || (excludedIPs.Contains(userHostAddress));
    }

    internal static ServerLocationType GetServerLocation(bool isRequestInternal)
    {
      ServerLocationType result = ServerLocationType.Prod;

      string hostName = GetContextHost().ToLowerInvariant();

      Match match = Regex.Match(hostName, @"\.?ote[\.-]");
      if (match.Success)
      {
        result = ServerLocationType.Ote;
      }
      else if (isRequestInternal)
      {
        match = Regex.Match(hostName, @"\.?test[\.-]");
        if (match.Success)
        {
          result = ServerLocationType.Test;
        }
        else
        {
          match = Regex.Match(hostName, @"\.?dev[\.-]|\.?debug[\.-]");
          if (match.Success)
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
      }

      return result;
    }
  }

}
