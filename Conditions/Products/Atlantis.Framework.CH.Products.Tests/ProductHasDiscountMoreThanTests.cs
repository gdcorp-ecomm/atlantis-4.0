using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Products;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Currency;

namespace Atlantis.Framework.CH.Products.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Currency.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.EcommPricing.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.CH.Products.dll")]
  public class ProductHasDiscountMoreThanTests
  {
    private bool _conditionHandlersRegistered;

    [TestInitialize]
    public void InitializeTests()
    {
      if (!_conditionHandlersRegistered)
      {
        ConditionHandlerManager.RegisterConditionHandler(new ProductHasDiscountMoreThanConditionHandler());
        _conditionHandlersRegistered = true;
      }
    }

    private MockProviderContainer _mockProviderContainer;
    private MockProviderContainer MockProviderContainer
    {
      get
      {
        if (_mockProviderContainer == null)
        {
          _mockProviderContainer = new MockProviderContainer();
          _mockProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _mockProviderContainer.SetData(MockSiteContextSettings.PrivateLabelId, "1");
          _mockProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _mockProviderContainer.RegisterProvider<IProductProvider, ProductProvider>();
          _mockProviderContainer.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
        }

        return _mockProviderContainer;
      }
    }

    [TestMethod]
    public void EvaluateValidConditionTrue()
    {
      Assert.IsTrue(ConditionHandlerManager.EvaluateCondition("ProductHasDiscountMoreThan", new[] { "7521", "0" }, MockProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionInvalidFalse()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("ProductHasDiscountMoreThan", new[] { "7521", "99" }, MockProviderContainer));
    }
  }
}
