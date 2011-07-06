using System;
using System.Web;
using Atlantis.Framework.BasePages.Providers;
using System.Diagnostics;
using Atlantis.Framework.Interface;
using System.Security.Principal;
using Atlantis.Framework.ManagerUser.Interface;
using Atlantis.Framework.BasePages.Cookies;
using Atlantis.Framework.BasePages.Authentication;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.BasePages.TestWeb
{
  public partial class ManagerTestLaunch : AtlantisContextBasePage
  {
    private IDebugContext DebugHelper
    {
      get { return HttpProviderContainer.Instance.Resolve<IDebugContext>(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      cmdLaunch.Click += new EventHandler(cmdLaunch_Click);
      cmdLaunchMstk.Click += new EventHandler(cmdLaunchMstk_Click);

      Response.Cache.SetCacheability(HttpCacheability.NoCache);

      Stopwatch pageLoad = null;

      if (SiteContext.IsRequestInternal)
      {
        pageLoad = new Stopwatch();
        pageLoad.Start();
      }

      PreRender += new EventHandler(ManagerTestLaunch_PreRender);

      DebugHelper.LogDebugTrackingData("IsManager", SiteContext.Manager.IsManager.ToString());
      DebugHelper.LogDebugTrackingData("ManagerId", SiteContext.Manager.ManagerUserId);
      DebugHelper.LogDebugTrackingData("ManagerName", SiteContext.Manager.ManagerUserName);
      DebugHelper.LogDebugTrackingData("ManagerShopper", SiteContext.Manager.ManagerShopperId);
      DebugHelper.LogDebugTrackingData("ManagerPrivateLabelId", SiteContext.Manager.ManagerPrivateLabelId.ToString());

      DebugHelper.LogDebugTrackingData("PrivateLabelId", SiteContext.PrivateLabelId.ToString());
      DebugHelper.LogDebugTrackingData("ShopperId", ShopperContext.ShopperId);
      DebugHelper.LogDebugTrackingData("ProgId", SiteContext.ProgId);
      DebugHelper.LogDebugTrackingData("ShopperStatus", ShopperContext.ShopperStatus.ToString());
      DebugHelper.LogDebugTrackingData("ContextId", SiteContext.ContextId.ToString());

      if (SiteContext.IsRequestInternal)
      {
        pageLoad.Stop();
        DebugHelper.LogDebugTrackingData("Page Load", pageLoad.ElapsedTicks.ToString() + " Ticks.");
      }
    }

    void cmdLaunchMstk_Click(object sender, EventArgs e)
    {
      string shopperId = tbShopperId.Text;
      string muserid;
      string mname;
      if (GetManagerData(shopperId, out muserid, out mname))
      {
        string mstk = AuthenticationHelper.GetMgrEncryptedValue(muserid, mname);
        string redirect = "~/ManagerTest.aspx?mstk=" + mstk + "&shopper_id=" + shopperId;
        Response.Redirect(redirect);
      }
    }

    void cmdLaunch_Click(object sender, EventArgs e)
    {
      string shopperId = tbShopperId.Text;
      string mgrUserId = GetManagerUserId(shopperId);
      string mgrShopper = GetMgrShopperQueryStringValue(shopperId, mgrUserId);

      string redirect = "~/ManagerTest.aspx?mgrshopper=" + mgrShopper;
      Response.Redirect(redirect);
    }

    void ManagerTestLaunch_PreRender(object sender, EventArgs e)
    {
    }

    private string GetMgrShopperQueryStringValue(string shopperId, string managerUserId)
    {
      string result = null;
      string utcExpiresString = DateTime.UtcNow.AddHours(2).ToString();
      string unencryptedValue = shopperId + "|" + managerUserId + "|" + utcExpiresString;
      result = CookieHelper.EncryptCookieValue(unencryptedValue);
      return result;
    }

    private bool GetManagerData(string shopperId, out string managerUserId, out string managerLogin)
    {
      bool result = false;
      managerUserId = string.Empty;
      managerLogin = string.Empty;

      string domain;
      string userId;
      if (GetWindowsUserAndDomain(out domain, out userId))
      {
        if (LookupManagerUser(domain, userId, shopperId, out managerUserId, out managerLogin))
        {
          result = true;
        }
      }

      return result;
    }

    private string GetManagerUserId(string shopperId)
    {
      string result = string.Empty;
      string domain;
      string userId;
      if (GetWindowsUserAndDomain(out domain, out userId))
      {
        string managerUserId;
        string managerLogin;
        if (LookupManagerUser(domain, userId, shopperId, out managerUserId, out managerLogin))
        {
          result = managerUserId;
        }
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
      }

      return result;
    }

    private bool LookupManagerUser(string domain, string userId, string shopperId, out string managerUserId, out string managerLoginName)
    {
      bool result = false;
      managerUserId = string.Empty;
      managerLoginName = string.Empty;

      try
      {
        ManagerUserLookupRequestData lookupRequest = new ManagerUserLookupRequestData(
          shopperId, HttpContext.Current.Request.Url.ToString(),
          string.Empty, string.Empty, 0, domain, userId);
        ManagerUserLookupResponseData lookupResponse =
          (ManagerUserLookupResponseData)DataCache.DataCache.GetProcessRequest(lookupRequest, BasePageEngineRequests.ManagerLookup);

        if (lookupResponse.IsSuccess)
        {
          result = true;
          managerUserId = lookupResponse.ManagerUserId;
          managerLoginName = lookupResponse.ManagerLoginName;
        }
        else
        {
          result = false;
          managerLoginName = "WS Error:" + domain + @"\" + userId;
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

  }

}
