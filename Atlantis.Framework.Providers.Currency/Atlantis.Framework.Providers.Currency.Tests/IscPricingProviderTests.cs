using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Interface.Pricing;
using Atlantis.Framework.Providers.Interface.PromoData;
using Atlantis.Framework.Providers.PromoData;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockPreferencesProvider;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;

namespace Atlantis.Framework.Providers.Currency.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.EcommPricing.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Currency.Impl.dll")]
  public class IscPricingProviderTests
  {
    private IProviderContainer SetContexts(int privateLabelId, string shopperId)
    {
      return SetContexts(privateLabelId, shopperId, true, false);
    }

    private IProviderContainer SetContexts(int privateLabelId, string shopperId, bool includeShopperPreferences, bool includePromoData)
    {
      var container = new MockProviderContainer();
      var request = new MockHttpRequest("http://localhost/default.aspx");
      MockHttpContext.SetFromWorkerRequest(request);

      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      container.RegisterProvider<IPricingProvider, IscPricingProvider>();

      IPricingProvider provider = container.Resolve<IPricingProvider>();
      provider.Enabled = true;

      if (includeShopperPreferences)
      {
        container.RegisterProvider<IShopperPreferencesProvider, MockShopperPreference>();
      }

      if (includePromoData)
      {
        container.RegisterProvider<IPromoDataProvider, PromoDataProvider>();
      }

      HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = privateLabelId;
      IShopperContext shopperContext = container.Resolve<IShopperContext>();
      shopperContext.SetLoggedInShopper(shopperId);

      return container;
    }

    [TestMethod]
    public void DoesIscAffectPricingValidReturnsTrue()
    {
      var container = SetContexts(1, "833437");
      int originalRequestTypeId = CurrencyProviderEngineRequests.ValidateNonOrderRequest;
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (originalRequestTypeId * 1000) + 1;
      IPricingProvider provider = container.Resolve<IPricingProvider>();
      try
      {
        int yard = -1;
        Assert.IsTrue(provider.DoesIscAffectPricing("valid", out yard));
      }
      finally
      {
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = originalRequestTypeId;
      }
    }


    [TestMethod]
    public void DoesIscAffectPricingInvalidReturnsFalse()
    {
      var container = SetContexts(1, "833437");
      int originalRequestTypeId = CurrencyProviderEngineRequests.ValidateNonOrderRequest;
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (originalRequestTypeId * 1000) + 1;
      IPricingProvider provider = container.Resolve<IPricingProvider>();
      try
      {
        int yard = -1;
        Assert.IsFalse(provider.DoesIscAffectPricing("invalid", out yard));
      }
      finally
      {
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = originalRequestTypeId;
      }
    }

    [TestMethod]
    public void DoesIscAffectPricingInactiveReturnsFalse()
    {
      var container = SetContexts(1, "833437");
      container.RegisterProvider<IPricingProvider, IscPricingProvider>();
      int originalRequestTypeId = CurrencyProviderEngineRequests.ValidateNonOrderRequest;
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (originalRequestTypeId * 1000) + 1;
      IPricingProvider provider = container.Resolve<IPricingProvider>();
      try
      {
        int yard = -1;
        Assert.IsFalse(provider.DoesIscAffectPricing("inactive", out yard));
      }
      finally
      {
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = originalRequestTypeId;
      }
    }

    [TestMethod]
    public void DoesIscAffectPricingExpiredReturnsFalse()
    {
      var container = SetContexts(1, "833437");
      int originalRequestTypeId = CurrencyProviderEngineRequests.ValidateNonOrderRequest;
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (originalRequestTypeId * 1000) + 1;
      IPricingProvider provider = container.Resolve<IPricingProvider>();
      try
      {
        int yard = -1;
        Assert.IsFalse(provider.DoesIscAffectPricing("expired", out yard));
      }
      finally
      {
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = originalRequestTypeId;
      }
    }

    [TestMethod]
    public void IscPricing()
    {
      var container = SetContexts(1, "833437");

      IPricingProvider provider = container.Resolve<IPricingProvider>();

      int yard = -1;
      Assert.IsTrue(provider.DoesIscAffectPricing("gfnomeac01", out yard));
      int price;
      provider.GetCurrentPrice(101, 0, "USD", out price, "gfnomeac01");
      Assert.AreEqual(599, price);

    }

    [TestMethod]
    public void IscPricingReturnsYard()
    {
      var container = SetContexts(1, "833437");
      IPricingProvider provider = container.Resolve<IPricingProvider>();

      int yard = -1;
      Assert.IsTrue(provider.DoesIscAffectPricing("gofd1001ac", out yard));
      Assert.IsTrue(yard >= 1);

    }

    [TestMethod]
    public void IscPricingReturnsEmptyYard()
    {
      var container = SetContexts(1, "833437");
      IPricingProvider provider = container.Resolve<IPricingProvider>();

      int yard = -1;
      Assert.IsTrue(provider.DoesIscAffectPricing("gfnomeac01", out yard));
      Assert.IsTrue(yard < 1);

    }

  }
}
