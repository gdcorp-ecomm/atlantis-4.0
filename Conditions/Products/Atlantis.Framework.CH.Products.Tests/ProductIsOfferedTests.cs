using System.Collections.Generic;
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
  [DeploymentItem("Atlantis.Framework.ProductOffer.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
  public class ProductIsOfferedTests
  {
    private bool _conditionHandlersRegistered;

    [TestInitialize]
    public void InitializeTests()
    {
      if (!_conditionHandlersRegistered)
      {
        ConditionHandlerManager.RegisterConditionHandler(new ProductIsOfferedConditionHandler());
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
          _mockProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _mockProviderContainer.RegisterProvider<IProductProvider, ProductProvider>();
          _mockProviderContainer.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
        }

        return _mockProviderContainer;
      }
    }

    [TestMethod]
    public void EvaluateValidConditionNameGDTrue()
    {
      MockProviderContainer.SetData(MockSiteContextSettings.PrivateLabelId, 1);
      Assert.IsTrue(ConditionHandlerManager.EvaluateCondition("productIsOffered", new[] { "WebHosting" }, MockProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionNameBlueRazorTrue()
    {
      MockProviderContainer.SetData(MockSiteContextSettings.PrivateLabelId, 2);
      Assert.IsTrue(ConditionHandlerManager.EvaluateCondition("productIsOffered", new[] { "email" }, MockProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionNameWwdFalse()
    {
      MockProviderContainer.SetData(MockSiteContextSettings.PrivateLabelId, 1387);
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("productIsOffered", new[] { "shoppingcart" }, MockProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionGDTrue()
    {
      MockProviderContainer.SetData(MockSiteContextSettings.PrivateLabelId, 1);
      Assert.IsTrue(ConditionHandlerManager.EvaluateCondition("productIsOffered", new[] { "1" }, MockProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionBlueRazorTrue()
    {
      MockProviderContainer.SetData(MockSiteContextSettings.PrivateLabelId, 2);
      Assert.IsTrue(ConditionHandlerManager.EvaluateCondition("productIsOffered", new[] { "1" }, MockProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionWwdFalse()
    {
      MockProviderContainer.SetData(MockSiteContextSettings.PrivateLabelId, 1387);
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("productIsOffered", new[] { "23" }, MockProviderContainer));
    }


    [TestMethod]
    public void TestErrorConditionInvalidParameters()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("productIsOffered", new List<string>(0),
        MockProviderContainer));

    }
  }
}
