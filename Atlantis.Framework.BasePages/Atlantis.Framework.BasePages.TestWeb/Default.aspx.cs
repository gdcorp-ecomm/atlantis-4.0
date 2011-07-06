using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Atlantis.Framework.BasePages.Providers;
using System.Diagnostics;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.BasePages.TestWeb
{
  public partial class _Default : AtlantisContextBasePage
  {
    private IDebugContext DebugHelper
    {
      get { return HttpProviderContainer.Instance.Resolve<IDebugContext>(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      Response.Cache.SetCacheability(HttpCacheability.NoCache);

      Stopwatch pageLoad = null;

      ShopperContext.SetNewShopper("832652");

      if (SiteContext.IsRequestInternal)
      {
        pageLoad = new Stopwatch();
        pageLoad.Start();
      }

      PreRender += new EventHandler(BasePageTest_PreRender);

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
      DebugHelper.LogDebugTrackingData("ShopperPriceType", ShopperContext.ShopperPriceType.ToString());

      if (!SiteContext.Manager.IsManager)
      {
        GetShopper();
        ShopperContext.SetNewShopper("123456");
      }

      if (SiteContext.IsRequestInternal)
      {
        pageLoad.Stop();
        DebugHelper.LogDebugTrackingData("Page Load", pageLoad.ElapsedTicks.ToString() + " Ticks.");
      }


    }

    void BasePageTest_PreRender(object sender, EventArgs e)
    {
      List<KeyValuePair<string, string>> list = DebugHelper.GetDebugTrackingData();
      gvPerformance.DataSource = list;
      gvPerformance.DataBind();
    }

    private void GetShopper()
    {
      Stopwatch sw = null;
      if (SiteContext.IsRequestInternal)
      {
        sw = new Stopwatch();
        sw.Start();
      }

      string shopperId = ShopperContext.ShopperId;

      if (SiteContext.IsRequestInternal)
      {
        sw.Stop();
        DebugHelper.LogDebugTrackingData("ShopperId Get", sw.ElapsedTicks.ToString() + " Ticks.");
      }
    }

    private void SetLoggedInShopper()
    {
      Stopwatch sw = null;
      if (SiteContext.IsRequestInternal)
      {
        sw = new Stopwatch();
        sw.Start();
      }

      bool Success = ShopperContext.SetLoggedInShopper("123456");

      if (SiteContext.IsRequestInternal)
      {
        sw.Stop();
        DebugHelper.LogDebugTrackingData("SetLoggedInShopper", sw.ElapsedTicks.ToString() + " Ticks.");
      }
    }
  }
}
