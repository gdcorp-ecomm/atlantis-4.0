using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using System.Linq;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.SplitTesting.Interface;
using Atlantis.Framework.Providers.SplitTesting.Tests.Mocks;
using Atlantis.Framework.Providers.UserAgentDetection.Interface;
using Atlantis.Framework.SplitTesting.Interface;
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
    }

    public TestContext TestContext { get; set; }

    private ISplitTestingProvider InitializeProviders(int privateLabelId, string shopperId, bool isBotUserAgent = false, bool isInternal = false)
    {
      var container = new MockProviderContainer();

      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      container.RegisterProvider<ISplitTestingProvider, SplitTestingProvider>();
      if (isBotUserAgent)
      {
        container.RegisterProvider<IUserAgentDetectionProvider, BotUserAgentProvider>();
      }
      else
      {
        container.RegisterProvider<IUserAgentDetectionProvider, NoBotUserAgentProvider>();
      }
      container.SetMockSetting(MockSiteContextSettings.IsRequestInternal, isInternal);
      HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = privateLabelId;
      var shopperContext = container.Resolve<IShopperContext>();
      shopperContext.SetNewShopper(shopperId);
      if (!_conditionHandlersRegistered)
      {
        ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
        _conditionHandlersRegistered = true;
      }

      SplitTestingEngineRequests.ActiveSplitTests = 684;
      SplitTestingEngineRequests.ActiveSplitTestDetails = 685;

      return container.Resolve<ISplitTestingProvider>();
    }

    [TestMethod]
    public void GetSplitTestSideForInvalidTestId()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      
      SplitTestingEngineRequests.ActiveSplitTests = MockEngineRequests.ActiveSplitTests_NoTests;
      var splitProvider = InitializeProviders(1, "858884");

      var side1 = splitProvider.GetSplitTestingSide(9999);
      var expectedName = "A";
      var expectedSideId = -1;
      Assert.AreEqual(expectedName, side1.Name);
      Assert.AreEqual(expectedSideId, side1.SideId);
    }

    [TestMethod]
    public void GetSplitTestSideForValidTestIdNoEligibilityRules()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var splitProvider = InitializeProviders(1, "858884");

      var side1 = splitProvider.GetSplitTestingSide(1009);
      Assert.IsTrue(side1 != null && !string.IsNullOrEmpty(side1.Name));
    }

    [TestMethod]
    public void GetSplitTestSideForValidTestIdsNoEligibilityRulesFromSameRequest()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var splitProvider = InitializeProviders(1, "858884");

      var side1 = splitProvider.GetSplitTestingSide(1009);
      Assert.IsTrue(side1 != null);

      var side2 = splitProvider.GetSplitTestingSide(1009);
      Assert.IsTrue(side2 != null && side1.Name == side2.Name);
    }

    [TestMethod]
    public void GetSplitTestSideForValidTestIdWithEligibilityRules()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      
      var splitProvider = InitializeProviders(1, "858884");

      var side1 = splitProvider.GetSplitTestingSide(1010);
      Assert.IsTrue(side1 != null);
    }

    [TestMethod]
    public void GetSplitTestSideForValidTestIdsWithEligibilityRulesFromSameRequest()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      
      var splitProvider = InitializeProviders(1, "858884");

      var side1 = splitProvider.GetSplitTestingSide(1010);
      Assert.IsTrue(side1 != null);

      var side2 = splitProvider.GetSplitTestingSide(1010);
      Assert.IsTrue(side2 != null && side1.Name == side2.Name);
    }

    [TestMethod]
    public void SetOverrideSide()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      string shopperId = "858884";
      int privateLabelId = 1;


      var splitProvider = InitializeProviders(privateLabelId, shopperId);

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
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      string shopperId = "858884";
      int privateLabelId = 1;


      var splitProvider = InitializeProviders(privateLabelId, shopperId, isInternal:true);
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
    public void BotDetectTest_Bot()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var splitProvider = InitializeProviders(1, "858884", isBotUserAgent: true);
      var side = splitProvider.GetSplitTestingSide(1010);
      Assert.AreEqual(-2, side.SideId);
    }

    [TestMethod]
    public void GetActiveTests()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Test";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var splitProvider = InitializeProviders(1, "858884");
      var tests = splitProvider.GetAllActiveTests;
      Assert.IsNotNull(tests);
      var iter = tests.GetEnumerator();
      Assert.IsTrue(iter.MoveNext(), "Check admin to see if there is at least one active test in TEST category");
    }

    [TestMethod]
    public void TestAllocation_50_50()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Test";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      int testId = 1011;
      TestAllocationTwoSides(testId, 50d, 50d, 2d);
    }

    [TestMethod]
    public void TestAllocation_0_100()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Test";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      int testId = 1012;
      TestAllocationTwoSides(testId, 0d, 100d, 2d);
    }

    [TestMethod]
    public void TestAllocation_80_20()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Test";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      int testId = 1013;
      TestAllocationTwoSides(testId, 80d, 20d, 2d);
    }

    private void TestAllocationTwoSides(int testId, double aTarget, double bTarget, double allowableDelta)
    {
      int totalCount = 5000;
      double countSideA = 0;
      double countSideB = 0;
      for (int i = 0; i < totalCount; i++)
      {
        HttpContext.Current.Items.Remove(
                "Atlantis.Framework.Interface.HttpProviderContainer.Atlantis.Framework.Providers.SplitTesting.Interface.ISplitTestingProvider");
        HttpContext.Current.Request.Cookies.Clear();

        var splitProvider = InitializeProviders(1, "858884");

        var side = splitProvider.GetSplitTestingSide(testId);
        switch (side.Name.ToUpper())
        {
          case "A":
            countSideA++;
            break;
          case "B":
            countSideB++;
            break;
        }
      }
      Assert.IsTrue(countSideA + countSideB == totalCount, "count of sides do not equal number of requests");
      double aPercent = countSideA / totalCount * 100;
      double bPercent = countSideB / totalCount * 100;
      Assert.AreEqual(aTarget, aPercent, allowableDelta);
      Assert.AreEqual(bTarget, bPercent, allowableDelta);
    }


    #region GetTrackingData tests

    [TestMethod]
    public void GetTrackingData_NoTestRequested()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Test";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var expected = string.Empty;

      var splitProvider = InitializeProviders(1, "858884");
      SplitTestingEngineRequests.ActiveSplitTests = MockEngineRequests.ActiveSplitTests_3Tests;
      SplitTestingEngineRequests.ActiveSplitTestDetails = MockEngineRequests.ActiveSplitTestDetails_AB;

      var actual = splitProvider.GetTrackingData;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetTrackingData_1TestRequested()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Test";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var expected = string.Format("{0}.{1}.{2}.{3}", 1, 1, 1, 1);

      var splitProvider = InitializeProviders(1, "858884");
      SplitTestingEngineRequests.ActiveSplitTests = MockEngineRequests.ActiveSplitTests_3Tests;
      SplitTestingEngineRequests.ActiveSplitTestDetails = MockEngineRequests.ActiveSplitTestDetails_A;

      splitProvider.GetSplitTestingSide(1);
      var actual = splitProvider.GetTrackingData;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetTrackingData_2TestsRequested()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Test";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var expected = string.Format("{0}.{1}.{2}.{3}", 1, 1, 1, 1);
      expected += "^";
      expected += string.Format("{0}.{1}.{2}.{3}", 3, 3, 3, 1);

      var splitProvider = InitializeProviders(1, "858884");
      SplitTestingEngineRequests.ActiveSplitTests = MockEngineRequests.ActiveSplitTests_3Tests;
      SplitTestingEngineRequests.ActiveSplitTestDetails = MockEngineRequests.ActiveSplitTestDetails_A;

      splitProvider.GetSplitTestingSide(1);
      splitProvider.GetSplitTestingSide(3);
      var actual = splitProvider.GetTrackingData;

      Assert.AreEqual(expected, actual);

    }

    #endregion

    #region GetTrackingDictionary Tests

    [TestMethod]
    public void GetTrackingDictionary_NoTestsRequested()
    {

      SplitTestingConfiguration.DefaultCategoryName = "Test";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var splitProvider = InitializeProviders(1, "858884");
      SplitTestingEngineRequests.ActiveSplitTests = MockEngineRequests.ActiveSplitTests_3Tests;
      SplitTestingEngineRequests.ActiveSplitTestDetails = MockEngineRequests.ActiveSplitTestDetails_A;

      var actual = splitProvider.GetTrackingDictionary;

      Assert.AreEqual(0, actual.Count);
    }
    
    [TestMethod]
    public void GetTrackingDictionary_2TestsRequested()
    {

      SplitTestingConfiguration.DefaultCategoryName = "Test";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      var splitProvider = InitializeProviders(1, "858884");
      SplitTestingEngineRequests.ActiveSplitTests = MockEngineRequests.ActiveSplitTests_3Tests;
      SplitTestingEngineRequests.ActiveSplitTestDetails = MockEngineRequests.ActiveSplitTestDetails_A;

      splitProvider.GetSplitTestingSide(1);
      splitProvider.GetSplitTestingSide(3);

      var actual = splitProvider.GetTrackingDictionary;

      Assert.AreEqual(2, actual.Count);

      IActiveSplitTest test1 = actual.Keys.Single(a => a.TestId == 1);
      Assert.AreEqual(1, test1.TestId);
      Assert.AreEqual(1, test1.VersionNumber);
      Assert.AreEqual(1, test1.RunId);
      IActiveSplitTestSide side1 = actual[test1];
      Assert.AreEqual(1, side1.SideId);
      Assert.AreEqual("A", side1.Name);
      
      IActiveSplitTest test2 = actual.Keys.Single(a => a.TestId == 3);
      Assert.AreEqual(3, test2.TestId);
      Assert.AreEqual(3, test2.VersionNumber);
      Assert.AreEqual(3, test2.RunId);
      IActiveSplitTestSide side2 = actual[test1];
      Assert.AreEqual(1, side2.SideId);
      Assert.AreEqual("A", side2.Name);

    }

    #endregion

  }
}
