using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml.Linq;
using Atlantis.Framework.BasePages.Providers;
using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MiniEncrypt;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.BasePages.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Manager.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Shopper.Impl.dll")]
  public class MstkManagerProviderTests
  {
    private IProviderContainer SetValidManagerContext(string shopperId)
    {
      string url = "http://gdgmanagertests.host/mypage.aspx?shopper_id=" + shopperId + "&mstk=" + BuildMstk();
      var request = new MockHttpRequest(url);
      MockHttpContext.SetFromWorkerRequest(request);
      HttpContext.Current.User = Thread.CurrentPrincipal;

      var result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IShopperContext, MockShopperContext>();
      result.RegisterProvider<IManagerContext, MstkManagerContextProvider>();

      return result;
    }

    private string BuildMstk()
    {
      string mstk;
      using (var mstkAuth = MstkAuthentication.CreateDisposable())
      {
        mstk = mstkAuth.CreateMstk("2055", "mmicco");
      }
      return mstk;
    }

    [TestMethod]
    public void GdgManagerContextValid()
    {
      var container = SetValidManagerContext("832652");
      var manager = container.Resolve<IManagerContext>();

      Assert.IsTrue(manager.IsManager);
    }
  }
}
