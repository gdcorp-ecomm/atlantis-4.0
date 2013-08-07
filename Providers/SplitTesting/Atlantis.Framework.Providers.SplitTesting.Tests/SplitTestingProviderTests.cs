using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.SplitTesting.Interface;
using Atlantis.Framework.Providers.UserAgentDetection;
using Atlantis.Framework.Providers.UserAgentDetection.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.SplitTesting.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.SplitTesting.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.UserAgentEx.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  public class SplitTestingProviderTests
  {
    private bool _conditionHandlersRegistered;

    [TestInitialize]
    public void InitializeTests()
    {
      if (!_conditionHandlersRegistered)
      {
        ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
        _conditionHandlersRegistered = true;
      }

      SplitTestingConfiguration.DefaultCategoryName = "test";
    }

    public TestContext TestContext { get; set; }

    private ISplitTestingProvider InitializeProvidersAndReturnSplitTestProvider(int privateLabelId, string shopperId)
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockNoManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<ISplitTestingProvider, SplitTestingProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IUserAgentDetectionProvider, UserAgentDetectionProvider>();

      HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = privateLabelId;
      var shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetNewShopper(shopperId);

      var splitProvider = HttpProviderContainer.Instance.Resolve<ISplitTestingProvider>();
      return splitProvider;
    }

    [TestMethod]
    public void GetSplitTestSideForInvalidTestId()
    {
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      ISplitTestingProvider splitProvider = InitializeProvidersAndReturnSplitTestProvider(1, "858884");
      Assert.IsNotNull(splitProvider);

      var side1 = splitProvider.GetSplitTestingSide(9999);
      var expected = "A";
      Assert.AreEqual(expected, side1.Name);
    }

    [TestMethod]
    public void GetSplitTestSideForValidTestIdNoEligibilityRules()
    {
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      ISplitTestingProvider splitProvider = InitializeProvidersAndReturnSplitTestProvider(1, "858884");

      Assert.IsNotNull(splitProvider);

      var side1 = splitProvider.GetSplitTestingSide(1009);
      Assert.IsTrue(side1 != null && !string.IsNullOrEmpty(side1.Name));
    }

    [TestMethod]
    public void GetSplitTestSideForValidTestIdsNoEligibilityRulesFromSameRequest()
    {
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      ISplitTestingProvider splitProvider = InitializeProvidersAndReturnSplitTestProvider(1, "858884");

      Assert.IsNotNull(splitProvider);

      var side1 = splitProvider.GetSplitTestingSide(1009);
      Assert.IsTrue(side1 != null);

      var side2 = splitProvider.GetSplitTestingSide(1009);
      Assert.IsTrue(side2 != null && side1.Name == side2.Name);
    }

    [TestMethod]
    public void GetSplitTestSideForValidTestIdWithEligibilityRules()
    {
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      ISplitTestingProvider splitProvider = InitializeProvidersAndReturnSplitTestProvider(1, "858884");

      Assert.IsNotNull(splitProvider);

      var side1 = splitProvider.GetSplitTestingSide(1010);
      Assert.IsTrue(side1 != null);
    }

    [TestMethod]
    public void GetSplitTestSideForValidTestIdsWithEligibilityRulesFromSameRequest()
    {
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      ISplitTestingProvider splitProvider = InitializeProvidersAndReturnSplitTestProvider(1, "858884");

      Assert.IsNotNull(splitProvider);

      var side1 = splitProvider.GetSplitTestingSide(1010);
      Assert.IsTrue(side1 != null);

      var side2 = splitProvider.GetSplitTestingSide(1010);
      Assert.IsTrue(side2 != null && side1.Name == side2.Name);
    }

    [TestMethod]
    public void SetOverrideSide()
    {
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      string shopperId = "858884";
      int privateLabelId = 1;

      ISplitTestingProvider splitProvider = InitializeProvidersAndReturnSplitTestProvider(privateLabelId, shopperId );

      Assert.IsNotNull(splitProvider);
      int testIdNotActive = -99;
      string sideName = "A";
      var success = splitProvider.SetOverrideSide(testIdNotActive, sideName);

      Assert.IsTrue(success);
      var cookie = HttpContext.Current.Response.Cookies.Get(string.Format("SplitTestingOverride{0}_{1}", privateLabelId, shopperId));
      Assert.IsNotNull(cookie);
      var cookieValue = cookie.Value.Split('=');
      Assert.AreEqual(testIdNotActive.ToString(), cookieValue[0]);
      Assert.AreEqual(sideName, cookieValue[1]);
    }

    [TestMethod]
    public void SideSelectedViaOverrideCookie()
    {
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      string shopperId = "858884";
      int privateLabelId = 1;

      ISplitTestingProvider splitProvider = InitializeProvidersAndReturnSplitTestProvider(privateLabelId, shopperId);

      Assert.IsNotNull(splitProvider);
      int testIdNotActive = 989858;
      string sideName = "W";

      var cookies = new NameValueCollection();
      cookies.Add(string.Format("SplitTestingOverride{0}_{1}", privateLabelId, shopperId), testIdNotActive + "=" + sideName);
      mockHttpRequest.MockCookies(cookies);

      var side1 = splitProvider.GetSplitTestingSide(testIdNotActive);
      Assert.IsNotNull(side1);
      Assert.AreEqual(sideName, side1.Name);
      Assert.AreEqual(-1, side1.SideId);
    }


    [TestMethod]
    public void BotDetectTest_Browser()
    {
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      mockHttpRequest.MockUserAgent(@"Mozilla/5.0 (Windows NT 6.0; rv:14.0) Gecko/20100101 Firefox/14.0.1");

      ISplitTestingProvider splitProvider = InitializeProvidersAndReturnSplitTestProvider(1, "858884");

      Assert.IsNotNull(splitProvider);

      var side = splitProvider.GetSplitTestingSide(1010);

      Assert.AreNotEqual(-2, side.SideId);
    }

    [TestMethod]
    public void BotDetectTest_Bot()
    {
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      mockHttpRequest.MockUserAgent(@"googlebot");

      ISplitTestingProvider splitProvider = InitializeProvidersAndReturnSplitTestProvider(1, "858884");

      Assert.IsNotNull(splitProvider);

      var side = splitProvider.GetSplitTestingSide(1010);

      Assert.AreEqual(-2, side.SideId);
    }


  }
}
