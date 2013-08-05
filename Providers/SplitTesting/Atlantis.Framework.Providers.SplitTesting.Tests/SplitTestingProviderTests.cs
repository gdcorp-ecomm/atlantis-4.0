using System.Reflection;
using System.Web;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.SplitTesting.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.SplitTesting.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.SplitTesting.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
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

      ISplitTestingProvider splitProvider = InitializeProvidersAndReturnSplitTestProvider(1, "858884");

      Assert.IsNotNull(splitProvider);

      var success = splitProvider.SetOverrideSide(1009433, "A");

      Assert.IsTrue(success);



    }
  }
}
