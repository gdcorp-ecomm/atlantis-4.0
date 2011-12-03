using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Atlantis.Framework.BasePages.SiteAdmin;
using Atlantis.Framework.Interface;
using System.Diagnostics;
using Atlantis.Framework.BasePages.SiteAdmin.Providers;
using Atlantis.Framework.BasePages.SiteAdmin.Security;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

public partial class _Default : SiteAdminBasePage
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

    DebugHelper.LogDebugTrackingData("WindowsUser", User.Identity.Name);

    bool isUserSystemAdmin = SiteAdminSecurity.IsCurrentUserInRole(SiteContext, 1);
    DebugHelper.LogDebugTrackingData("IsUserSystemAdmin", isUserSystemAdmin.ToString());

    DebugHelper.LogDebugTrackingData("IsManager", SiteContext.Manager.IsManager.ToString());
    DebugHelper.LogDebugTrackingData("ManagerId", SiteContext.Manager.ManagerUserId);
    DebugHelper.LogDebugTrackingData("ManagerName", SiteContext.Manager.ManagerUserName);
    DebugHelper.LogDebugTrackingData("ManagerShopper", SiteContext.Manager.ManagerShopperId);
    DebugHelper.LogDebugTrackingData("ManagerPrivateLabelId", SiteContext.Manager.ManagerPrivateLabelId.ToString());

    DebugHelper.LogDebugTrackingData("PrivateLabelId", SiteContext.PrivateLabelId.ToString());
    DebugHelper.LogDebugTrackingData("ProgId", SiteContext.ProgId);
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