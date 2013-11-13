using System.Globalization;
using Atlantis.Framework.BasePages.Cookies;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Manager.Interface;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;

namespace Atlantis.Framework.BasePages.Providers
{
  public delegate void ManagerLoggingDelegate(string message, string data);

  public class GdgManagerContextProvider : ManagerContextBase
  {
    private const string _MGRSHOPPERQSKEY = "mgrshopper";

    public GdgManagerContextProvider(IProviderContainer providerContainer)
      : base(providerContainer)
    {
    }

    private bool IsRequestInternalManagerHost()
    {
      bool result = false;

      if (HttpContext.Current != null)
      {
        string managerHost = WebConfigurationManager.AppSettings["ManagerHost"];
        if (!string.IsNullOrEmpty(managerHost))
        {
          string host = RequestHelper.GetContextHost();
          result = managerHost.Equals(host, StringComparison.OrdinalIgnoreCase);
        }
      }

      return result;
    }

    private string GetManagerLoginName(int managerUserId)
    {
      var request = new ManagerCategoriesRequestData(managerUserId);
      var response = (ManagerCategoriesResponseData) DataCache.DataCache.GetProcessRequest(request, BasePageEngineRequests.ManagerProperties);
      string result;

      if (!response.TryGetManagerAttribute("login_name", out result))
      {
        result = string.Empty;
      }

      return result;
    }

    private bool LookupManagerUser(string userId, string validManagerUserid, out string managerUserId, out string managerLoginName)
    {
      bool result = false;
      managerUserId = string.Empty;
      managerLoginName = string.Empty;

      try
      {
        if (string.IsNullOrEmpty(validManagerUserid))
        {
          LogManagerException("GdgManagerContextProvider.LookupManagerUser", "validManagerUserId is null or empty.", "Check contents of mgrshopper");
        }
        else
        {
          var lookupRequest = new ManagerUserLookupRequestData(userId);
          var lookupResponse =
            (ManagerUserLookupResponseData) DataCache.DataCache.GetProcessRequest(lookupRequest, BasePageEngineRequests.ManagerLookup);

          if (lookupResponse.IsValid)
          {
            if (validManagerUserid.Equals(lookupResponse.ManagerUserId.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
            {
              result = true;
              managerUserId = lookupResponse.ManagerUserId.ToString(CultureInfo.InvariantCulture);
              managerLoginName = GetManagerLoginName(lookupResponse.ManagerUserId);
            }
            else
            {
              managerLoginName = "QS Error: " + userId;
              LogManagerException("GdgManagerContextProvider.LookupManagerUser", managerLoginName, validManagerUserid + "!=" + lookupResponse.ManagerUserId);
            }
          }
          else
          {
            managerLoginName = "Error:" + userId;
            LogManagerException("GdgManagerContextProvider.LookupManagerUser", managerLoginName, "Unknown error.");
          }
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + ex.StackTrace;
        LogManagerException("GdgManagerContextProvider.LookupManagerUser", message, string.Empty);
      }

      return result;
    }

    private string[] DecryptShopperIdFromQueryString(out string encryptedQueryStringValue)
    {
      string[] result = null;
      encryptedQueryStringValue = HttpContext.Current.Request.QueryString[_MGRSHOPPERQSKEY];
      if (!string.IsNullOrEmpty(encryptedQueryStringValue))
      {
        string delimitedValue = CookieHelper.DecryptCookieValue(encryptedQueryStringValue);
        result = delimitedValue.Split('|');
        if (result.Length < 3)
        {
          LogManagerException("GdgManagerContext.DecryptShopperIdFromQueryString", "mgrshopper query string invalid # of item.", delimitedValue);
        }
      }
      else
      {
        LogManagerException("GdgManagerContext.DecryptShopperIdFromQueryString", "mgrshopper query string missing.", HttpContext.Current.Request.RawUrl);
      }
      return result;
    }

    private bool GetWindowsUserAndDomain(out string domain, out string userId)
    {
      bool result = false;
      domain = null;
      userId = null;
      try
      {
        if (HttpContext.Current != null && HttpContext.Current.User != null)
        {
          var windowsIdentity = HttpContext.Current.User.Identity as WindowsIdentity;
          if ((windowsIdentity != null) && (windowsIdentity.IsAuthenticated))
          {
            string[] nameParts = windowsIdentity.Name.Split('\\');
            if (nameParts.Length == 2)
            {
              domain = nameParts[0];
              userId = nameParts[1];
              result = true;
            }
            else
            {
              LogManagerException("GdgManagerContext.GetWindowsUserAndDomain", "Windows identity cannot be determined.", windowsIdentity.Name);
            }
          }

          if (!result)
          {
            LogManagerException("GdgManagerContext.GetWindowsUserAndDomain", "Windows identity cannot be determined result False.", "Windows authentication issue.");
          }
        }
        else
        {
          if (HttpContext.Current == null)
          {
            LogManagerException("GdgManagerContext.GetWindowsUserAndDomain", "No Current Context.", "Windows authentication issue.");
          }
          else if (HttpContext.Current.User == null)
          {
            LogManagerException("GdgManagerContext.GetWindowsUserAndDomain", "No Current User.", "Windows authentication issue.");
          }
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + ex.StackTrace;
        LogManagerException("GdgManagerContext.GetWindowsUserAndDomain", "Windows identity cannot be determined." + message, "Windows authentication issue.");
      }

      return result;
    }


    protected override void DetermineManager()
    {
      if (RequestHelper.IsRequestInternal() && IsRequestInternalManagerHost())
      {
        string encryptedQueryStringValue;
        string[] mgrShopperArray = DecryptShopperIdFromQueryString(out encryptedQueryStringValue);
        if ((mgrShopperArray != null) && (mgrShopperArray.Length >= 3))
        {
          string shopperIdFromQueryString = mgrShopperArray[0];
          string managerUserIdFromQueryString = mgrShopperArray[1];
          string expiresString = mgrShopperArray[2];

          if (!string.IsNullOrEmpty(shopperIdFromQueryString)
            && !string.IsNullOrEmpty(managerUserIdFromQueryString)
            && !string.IsNullOrEmpty(expiresString))
          {
            DateTime expiresUtc;
            if (DateTime.TryParse(expiresString, out expiresUtc))
            {
              if (DateTime.UtcNow <= expiresUtc)
              {
                string domain;
                string userId;
                if (GetWindowsUserAndDomain(out domain, out userId))
                {
                  string managerUserId;
                  string managerUserName;
                  if (LookupManagerUser(userId, managerUserIdFromQueryString, out managerUserId, out managerUserName))
                  {
                    ManagerUserId = managerUserId;
                    ManagerUserName = managerUserName;
                    ManagerShopperId = shopperIdFromQueryString;
                    ManagerQuery[_MGRSHOPPERQSKEY] = encryptedQueryStringValue;
                    SetManagerContext();
                  }
                }
              }
            }
          }
        }
      }
    }

  }
}
