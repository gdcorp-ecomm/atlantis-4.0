using System;
using Atlantis.Framework.Interface;
using System.Collections.Specialized;
using System.Web;
using System.Security.Principal;
using Atlantis.Framework.VerifyShopper.Interface;
using Atlantis.Framework.ManagerUser.Interface;
using System.Web.Configuration;
using Atlantis.Framework.BasePages.Cookies;

namespace Atlantis.Framework.BasePages.Providers
{
  public class GdgManagerContextProvider : ProviderBase, IManagerContext
  {
    private const int _WWD_PLID = 1387;
    private const string _MGRSHOPPERQSKEY = "mgrshopper";

    private NameValueCollection _managerQuery = new NameValueCollection();
    private string _managerUserId = string.Empty;
    private string _managerUserName = string.Empty;
    private string _managerShopperId = string.Empty;
    private bool _isManager;
    private int _managerPrivateLabelId;
    private int _managerContextId = ContextIds.Unknown;

    public GdgManagerContextProvider(IProviderContainer providerContainer) : base(providerContainer)
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
          string host = HttpContext.Current.Request.Url.Host;
          result = (string.Compare(host, managerHost, true) == 0);
        }
      }

      return result;
    }

    private bool LookupManagerUser(string domain, string userId, string shopperId, string validManagerUserid, out string managerUserId, out string managerLoginName)
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
          ManagerUserLookupRequestData lookupRequest = new ManagerUserLookupRequestData(
            shopperId, HttpContext.Current.Request.Url.ToString(),
            string.Empty, string.Empty, 0, domain, userId);
          ManagerUserLookupResponseData lookupResponse =
            (ManagerUserLookupResponseData)DataCache.DataCache.GetProcessRequest(lookupRequest, BasePageEngineRequests.ManagerLookup);

          if (lookupResponse.IsSuccess)
          {
            if (string.Compare(validManagerUserid, lookupResponse.ManagerUserId, true) == 0)
            {
              result = true;
              managerUserId = lookupResponse.ManagerUserId;
              managerLoginName = lookupResponse.ManagerLoginName;
            }
            else
            {
              result = false;
              managerLoginName = "QS Error: " + domain + @"\" + userId;
              LogManagerException(managerLoginName, validManagerUserid + "!=" + lookupResponse.ManagerUserId );
            }

          }
          else
          {
            result = false;
            managerLoginName = "WS Error:" + domain + @"\" + userId;
            string data = "Unknown error.";
            if (!string.IsNullOrEmpty(lookupResponse.Error))
            {
              data = lookupResponse.Error;
            }
            LogManagerException(managerLoginName, data);
          }

        }
      }
      catch (Exception ex)
      {
        AtlantisException aEx = new AtlantisException("GdgManagerContextProvider.LookupManagerUser",
          HttpContext.Current.Request.Url.ToString(),
          "0", ex.Message, ex.StackTrace, shopperId,
          string.Empty, HttpContext.Current.Request.UserHostAddress,
          string.Empty, 0);
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
        VerifyShopperRequestData request = new VerifyShopperRequestData(shopperId,
          HttpContext.Current.Request.Url.ToString(),
          string.Empty, string.Empty, 0);
        VerifyShopperResponseData response = (VerifyShopperResponseData)Engine.Engine.ProcessRequest(
          request, BasePageEngineRequests.VerifyShopper);

        if (response.IsSuccess)
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
        AtlantisException aEx = new AtlantisException("GdgManagerContextProvider.VerifyShopper",
          HttpContext.Current.Request.Url.ToString(),
          "0", ex.Message, ex.StackTrace, shopperId,
          string.Empty, HttpContext.Current.Request.UserHostAddress,
          string.Empty, 0);
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

      WindowsIdentity windowsIdentity = HttpContext.Current.User.Identity as WindowsIdentity;
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
        LogManagerException("Windows identity cannot be determined.", "Windows authentication issue.");
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
                    if (LookupManagerUser(domain, userId, shopperIdFromQueryString, managerUserIdFromQueryString, out _managerUserId, out _managerUserName))
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
      catch(Exception ex)
      {
        _isManager = false;
        _managerUserId = string.Empty;
        _managerUserName = string.Empty;
        _managerQuery.Clear();
        _managerContextId = 0;
        LogManagerException(ex.Message, ex.StackTrace);
      }
    }

    private string RequestUrl
    {
      get
      {
        string result = string.Empty;
        if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
        {
          result = HttpContext.Current.Request.RawUrl;
        }
        return result;
      }
    }

    private string ClientIP
    {
      get
      {
        string result = string.Empty;
        if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
        {
          result = HttpContext.Current.Request.UserHostAddress;
        }
        return result;
      }
    }

    private void LogManagerException(string message, string data)
    {
      AtlantisException managerException = new AtlantisException(
        "GdgManagerContextProvider.DetermineManager", RequestUrl, "403", message, data,
        _managerShopperId, string.Empty, ClientIP, string.Empty, 0);
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
