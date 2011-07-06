using System;
using System.Web;
using Atlantis.Framework.BasePages.Providers;
using System.Diagnostics;
using Atlantis.Framework.Interface;
using System.Collections.Generic;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.BasePages.TestWeb
{
  public partial class ManagerTest : AtlantisContextBasePage
  {
    private IDebugContext DebugHelper
    {
      get { return HttpProviderContainer.Instance.Resolve<IDebugContext>(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      PreRender += new EventHandler(ManagerTest_PreRender);

      Response.Cache.SetCacheability(HttpCacheability.NoCache);

      Stopwatch pageLoad = null;

      if (SiteContext.IsRequestInternal)
      {
        pageLoad = new Stopwatch();
        pageLoad.Start();
      }

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

    void ManagerTest_PreRender(object sender, EventArgs e)
    {
      List<KeyValuePair<string, string>> list = DebugHelper.GetDebugTrackingData();
      gvPerformance.DataSource = list;
      gvPerformance.DataBind();
    }

  }
}
