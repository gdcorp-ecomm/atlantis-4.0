using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Manager.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Testing.MockProviders;

namespace Atlantis.Framework.Providers.Manager.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Manager.Impl.dll")]
  public class ManagerProviderTest
  {
    private const string SHOPPER_ID = "856907";
    private const int GD_PL = 1;
    private const string URL = "http://mysite.com/default.aspx";
    private IProviderContainer ObjectContainer { get; set; }

    [TestInitialize]
    public void InitializeObjectContainer()
    {
      ObjectContainer = new ObjectProviderContainer();
      ObjectContainer.RegisterProvider<IManagerProvider, ManagerProvider>();
      ObjectContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      ObjectContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      ObjectContainer.RegisterProvider<IManagerContext, MockManagerContext>();
    }

    private IManagerProvider NewManagerProvider()
    {
      var siteContext = ObjectContainer.Resolve<ISiteContext>();
      var shopperContext = ObjectContainer.Resolve<IShopperContext>();
      var managerContext = ObjectContainer.Resolve<IManagerContext>();

      shopperContext.SetLoggedInShopper(SHOPPER_ID);
      HttpContext.Current.Items[MockSiteContextSettings.PageCount] = 1;
      HttpContext.Current.Items[MockSiteContextSettings.Pathway] = "abowk-a2-024n-29a90adf9-afde";
      HttpContext.Current.Items[MockManagerContextSettings.UserId] = "4692";
      HttpContext.Current.Items[MockManagerContextSettings.IsManager] = true;
      HttpContext.Current.Items[MockManagerContextSettings.ShopperId] = SHOPPER_ID;
      HttpContext.Current.Items[MockManagerContextSettings.PrivateLabelId] = "1";
      HttpContext.Current.Items[MockManagerContextSettings.UserName] = "Dmitri Schostakowitsch";

      var manager = ObjectContainer.Resolve<IManagerProvider>();

      return manager;
    }

    [TestMethod]
    public void ManagerCategoriesTest()
    {
      var request = new MockHttpRequest(URL);
      MockHttpContext.SetFromWorkerRequest(request);

      var manager = NewManagerProvider();
      int[] roles = {1, 3, 5};
      Assert.IsTrue(manager.IsCurrentUserInAnyRole(roles));

      string loginName;
      if (!manager.TryGetManagerAttribute("login_name", out loginName))
      {
        loginName = "wrongName";
      }
      Assert.AreEqual(loginName, "ksearle");
    }

    [TestMethod]
    public void ManagerUserLookupTest()
    {
      var request = new MockHttpRequest(URL);
      MockHttpContext.SetFromWorkerRequest(request);
      MockHttpContext.SetUser();

      var manager = NewManagerProvider();

      var data = manager.GetManagerUserData();

      Assert.AreEqual(data.LoginName, "ksearle");
    }
  }
}
