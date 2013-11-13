using System.CodeDom;
using System.Globalization;
using Atlantis.Framework.BasePages.Cookies;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Manager.Interface;
using Atlantis.Framework.Shopper.Interface;
using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;

namespace Atlantis.Framework.BasePages.Providers
{
  public delegate void ManagerLoggingDelegate(string message, string data);

  public class GdgManagerContextProvider : ProviderBase, IManagerContext
  {
    private const int _WWD_PLID = 1387;
    private const string _MGRSHOPPERQSKEY = "mgrshopper";
    private readonly NameValueCollection _managerQuery = new NameValueCollection();
    private string _managerUserId = string.Empty;
    private string _managerUserName = string.Empty;
    private string _managerShopperId = string.Empty;
    private bool _isManager;
    private int _managerPrivateLabelId;
    private int _managerContextId = ContextIds.Unknown;

    public GdgManagerContextProvider(IProviderContainer providerContainer)
      : base(providerContainer)
    {
      DetermineManager();
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
          result = (string.Compare(host, managerHost, true) == 0);
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
          LogManagerException("validManagerUserId is null or empty.", "Check contents of mgrshopper");
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
              LogManagerException(managerLoginName, validManagerUserid + "!=" + lookupResponse.ManagerUserId);
            }
          }
          else
          {
            managerLoginName = "Error:" + userId;
            LogManagerException(managerLoginName, "Unknown error.");
          }
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + ex.StackTrace;
        var aEx = new AtlantisException("GdgManagerContextProvider.LookupManagerUser", 0, message, string.Empty);
        Engine.Engine.LogAtlantisException(aEx);
      }

      return result;
    }

    private bool VerifyShopper(string shopperId, out int privateLabelId)
    {
      bool result = false;
      privateLabelId = 0;

      try
      {
        var request = new VerifyShopperRequestData(shopperId);
        var response = (VerifyShopperResponseData)Engine.Engine.ProcessRequest(request, BasePageEngineRequests.VerifyShopper);

        if (response.IsVerified)
        {
          privateLabelId = response.PrivateLabelId;
          result = true;
        }
        else
        {
          LogManagerException("Verify Shopper Failed.", "shopperid=" + shopperId);
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + ex.StackTrace;
        var aEx = new AtlantisException("GdgManagerContextProvider.VerifyShopper", 0, message, shopperId);
        Engine.Engine.LogAtlantisException(aEx);
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
          LogManagerException("mgrshopper query string invalid # of item.", delimitedValue);
        }
      }
      else
      {
        LogManagerException("mgrshopper query string missing.", HttpContext.Current.Request.RawUrl);
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
              LogManagerException("Windows identity cannot be determined.", windowsIdentity.Name);
            }
          }

          if (!result)
          {
            LogManagerException("Windows identity cannot be determined result False.", "Windows authentication issue.");
          }
        }
        else
        {
          if (HttpContext.Current == null)
          {
            LogManagerException("No Current Context.", "Windows authentication issue.");
          }
          else if (HttpContext.Current.User == null)
          {
            LogManagerException("No Current User.", "Windows authentication issue.");
          }
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + ex.StackTrace;
        LogManagerException("Windows identity cannot be determined." + message, "Windows authentication issue.");
      }

      return result;
    }

    private void SetManagerContext()
    {
      _managerContextId = ContextIds.Unknown;
      if (!string.IsNullOrEmpty(_managerShopperId))
      {
        if (VerifyShopper(_managerShopperId, out _managerPrivateLabelId))
        {
          _isManager = true;
          if (_managerPrivateLabelId == 1)
          {
            _managerContextId = ContextIds.GoDaddy;
          }
          else if (_managerPrivateLabelId == 2)
          {
            _managerContextId = ContextIds.BlueRazor;
          }
          else if (_managerPrivateLabelId == _WWD_PLID)
          {
            _managerContextId = ContextIds.WildWestDomains;
          }
          else
          {
            _managerContextId = ContextIds.Reseller;
          }
        }
      }

    }

    private void DetermineManager()
    {
      _isManager = false;

      try
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
                    if (LookupManagerUser(userId, managerUserIdFromQueryString, out _managerUserId, out _managerUserName))
                    {
                      _managerShopperId = shopperIdFromQueryString;
                      _managerQuery[_MGRSHOPPERQSKEY] = encryptedQueryStringValue;
                      SetManagerContext();
                    }
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        _isManager = false;
        _managerUserId = string.Empty;
        _managerUserName = string.Empty;
        _managerQuery.Clear();
        _managerContextId = 0;
        LogManagerException(ex.Message, ex.StackTrace);
      }
    }

    private void LogManagerException(string message, string data)
    {
      var managerException = new AtlantisException("GdgManagerContextProvider.DetermineManager", 403, message, data);
      Engine.Engine.LogAtlantisException(managerException);
    }

    #region IManagerContext Members

    public bool IsManager
    {
      get { return _isManager; }
    }

    public string ManagerUserId
    {
      get { return _managerUserId; }
    }

    public string ManagerUserName
    {
      get { return _managerUserName; }
    }

    public NameValueCollection ManagerQuery
    {
      get { return _managerQuery; }
    }

    public string ManagerShopperId
    {
      get { return _managerShopperId; }
    }

    public int ManagerContextId
    {
      get { return _managerContextId; }
    }

    public int ManagerPrivateLabelId
    {
      get { return _managerPrivateLabelId; }
    }

    #endregion
  }
}
