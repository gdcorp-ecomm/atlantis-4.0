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
    public void GetSplitTestSideForValidTestId()
    {
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      ISplitTestingProvider splitProvider = InitializeProvidersAndReturnSplitTestProvider(1, "858884");

      Assert.IsNotNull(splitProvider);

      var side1 = splitProvider.GetSplitTestingSide(1);
      Assert.IsTrue(!string.IsNullOrEmpty(side1));
    }

    [TestMethod]
    public void GetSplitTestSideForInvalidTestId()
    {
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      ISplitTestingProvider splitProvider = InitializeProvidersAndReturnSplitTestProvider(1, "858884");
      Assert.IsNotNull(splitProvider);

      var side1 = splitProvider.GetSplitTestingSide(9999);
      Assert.IsTrue(string.IsNullOrEmpty(side1));
    }

    [TestMethod]
    public void GetSplitTestSideForValidTestIdsFromSameRequest()
    {
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      ISplitTestingProvider splitProvider = InitializeProvidersAndReturnSplitTestProvider(1, "858884");

      Assert.IsNotNull(splitProvider);

      var side1 = splitProvider.GetSplitTestingSide(1);
      Assert.IsTrue(!string.IsNullOrEmpty(side1));

      var side2 = splitProvider.GetSplitTestingSide(1);
      Assert.IsTrue(side1 == side2);
    }
  }
}
