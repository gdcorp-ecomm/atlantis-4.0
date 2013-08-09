using System;
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
    }

    public TestContext TestContext { get; set; }

    private void InitializeProviders(int privateLabelId, string shopperId)
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockNoManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<ISplitTestingProvider, SplitTestingProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IUserAgentDetectionProvider, UserAgentDetectionProvider>();
      HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = privateLabelId;
      var shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetNewShopper(shopperId);
      if (!_conditionHandlersRegistered)
      {
        ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
        _conditionHandlersRegistered = true;
      }
    }

    [TestMethod]
    public void GetSplitTestSideForInvalidTestId()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      InitializeProviders(1, "858884");
      var splitProvider = HttpProviderContainer.Instance.Resolve<ISplitTestingProvider>();
      Assert.IsNotNull(splitProvider);

      var side1 = splitProvider.GetSplitTestingSide(9999);
      var expected = "A";
      Assert.AreEqual(expected, side1.Name);
    }

    [TestMethod]
    public void GetSplitTestSideForValidTestIdNoEligibilityRules()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      InitializeProviders(1, "858884");
      var splitProvider = HttpProviderContainer.Instance.Resolve<ISplitTestingProvider>();

      Assert.IsNotNull(splitProvider);

      var side1 = splitProvider.GetSplitTestingSide(1009);
      Assert.IsTrue(side1 != null && !string.IsNullOrEmpty(side1.Name));
    }

    [TestMethod]
    public void GetSplitTestSideForValidTestIdsNoEligibilityRulesFromSameRequest()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      InitializeProviders(1, "858884");
      var splitProvider = HttpProviderContainer.Instance.Resolve<ISplitTestingProvider>();

      Assert.IsNotNull(splitProvider);

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

      InitializeProviders(1, "858884");
      var splitProvider = HttpProviderContainer.Instance.Resolve<ISplitTestingProvider>();

      Assert.IsNotNull(splitProvider);

      var side1 = splitProvider.GetSplitTestingSide(1010);
      Assert.IsTrue(side1 != null);
    }

    [TestMethod]
    public void GetSplitTestSideForValidTestIdsWithEligibilityRulesFromSameRequest()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      InitializeProviders(1, "858884");
      var splitProvider = HttpProviderContainer.Instance.Resolve<ISplitTestingProvider>();

      Assert.IsNotNull(splitProvider);

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

      InitializeProviders(privateLabelId, shopperId );
      var splitProvider = HttpProviderContainer.Instance.Resolve<ISplitTestingProvider>();

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
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      string shopperId = "858884";
      int privateLabelId = 1;

      InitializeProviders(privateLabelId, shopperId);
      var splitProvider = HttpProviderContainer.Instance.Resolve<ISplitTestingProvider>();

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
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      mockHttpRequest.MockUserAgent(@"Mozilla/5.0 (Windows NT 6.0; rv:14.0) Gecko/20100101 Firefox/14.0.1");

      InitializeProviders(1, "858884");
      var splitProvider = HttpProviderContainer.Instance.Resolve<ISplitTestingProvider>();

      Assert.IsNotNull(splitProvider);

      var side = splitProvider.GetSplitTestingSide(1010);

      Assert.AreNotEqual(-2, side.SideId);
    }

    [TestMethod]
    public void BotDetectTest_Bot()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      mockHttpRequest.MockUserAgent(@"googlebot");

      InitializeProviders(1, "858884");
      var splitProvider = HttpProviderContainer.Instance.Resolve<ISplitTestingProvider>();

      Assert.IsNotNull(splitProvider);

      var side = splitProvider.GetSplitTestingSide(1010);

      Assert.AreEqual(-2, side.SideId);
    }

    [TestMethod]
    public void GetActiveTests()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Test";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      InitializeProviders(1, "858884");
      var splitProvider = HttpProviderContainer.Instance.Resolve<ISplitTestingProvider>();

      Assert.IsNotNull(splitProvider);

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
      double aLowerLimit = .48d;
      double aUpperLimit = .52d;
      double bLowerLimit = .48d;
      double bUpperLimit = .52d;

      TestAllocationTwoSides(testId, aLowerLimit, aUpperLimit, bLowerLimit, bUpperLimit);
    }

    [TestMethod]
    public void TestAllocation_0_100()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Test";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      int testId = 1012;
      double aLowerLimit = 0.0d;
      double aUpperLimit = 0.0d;
      double bLowerLimit = 1.0d;
      double bUpperLimit = 1.0d;

      TestAllocationTwoSides(testId, aLowerLimit, aUpperLimit, bLowerLimit, bUpperLimit);
    }

    [TestMethod]
    public void TestAllocation_80_20()
    {
      SplitTestingConfiguration.DefaultCategoryName = "Test";
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      int testId = 1013;
      double aLowerLimit = 0.78d;
      double aUpperLimit = 0.82d;
      double bLowerLimit = 0.18d;
      double bUpperLimit = 0.22d;

      TestAllocationTwoSides(testId, aLowerLimit, aUpperLimit, bLowerLimit, bUpperLimit);
    }

    private void TestAllocationTwoSides(int testId, double aLowerLimit, double aUpperLimit, double bLowerLimit, double bUpperLimit)
    {
      int totalCount = 5000;
      double countSideA = 0;
      double countSideB = 0;
      for (int i = 0; i < totalCount; i++)
      {
        HttpContext.Current.Items.Remove(
                "Atlantis.Framework.Interface.HttpProviderContainer.Atlantis.Framework.Providers.SplitTesting.Interface.ISplitTestingProvider");
        HttpContext.Current.Request.Cookies.Clear();
        InitializeProviders(1, "858884");
        var splitProvider = HttpProviderContainer.Instance.Resolve<ISplitTestingProvider>();
        
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
      double aPercent = countSideA / totalCount;
      double bPercent = countSideB / totalCount;
      Assert.IsTrue(aPercent >= aLowerLimit && aPercent <= aUpperLimit, "A was " + aPercent);
      Assert.IsTrue(bPercent >= bLowerLimit && bPercent <= bUpperLimit, "B was " + bPercent);
    }




  }
}
