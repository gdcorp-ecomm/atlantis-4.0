using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Interface.Pricing;
using Atlantis.Framework.Providers.Interface.PromoData;
using Atlantis.Framework.Providers.PromoData;
using Atlantis.Framework.Testing.MockHttpContext;
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
    private void SetContexts(int privateLabelId, string shopperId)
    {
      SetContexts(privateLabelId, shopperId, true, false);
    }

    private void SetContexts(int privateLabelId, string shopperId, bool includeShopperPreferences, bool includePromoData)
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockNoManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<IPricingProvider, IscPricingProvider>();

      if (includeShopperPreferences)
      {
        HttpProviderContainer.Instance.RegisterProvider<IShopperPreferencesProvider, MockShopperPreference>();
      }

      if (includePromoData)
      {
        HttpProviderContainer.Instance.RegisterProvider<IPromoDataProvider, PromoDataProvider>();
      }

      HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = privateLabelId;
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetLoggedInShopper(shopperId);
    }

    [TestMethod]
    public void DoesIscAffectPricingValidReturnsTrue()
    {
      SetContexts(1, "833437");
      int originalRequestTypeId = CurrencyProviderEngineRequests.ValidateNonOrderRequest;
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (originalRequestTypeId * 1000) + 1;
      IPricingProvider provider = HttpProviderContainer.Instance.Resolve<IPricingProvider>();
      try
      {
        Assert.IsTrue(provider.DoesIscAffectPricing("valid"));
      }
      finally
      {
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = originalRequestTypeId;
      }
    }

    [TestMethod]
    public void DoesIscAffectPricingInvalidReturnsFalse()
    {
      SetContexts(1, "833437");
      int originalRequestTypeId = CurrencyProviderEngineRequests.ValidateNonOrderRequest;
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (originalRequestTypeId * 1000) + 1;
      IPricingProvider provider = HttpProviderContainer.Instance.Resolve<IPricingProvider>();
      try
      {
        Assert.IsFalse(provider.DoesIscAffectPricing("invalid"));
      }
      finally
      {
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = originalRequestTypeId;
      }
    }

    [TestMethod]
    public void DoesIscAffectPricingInactiveReturnsFalse()
    {
      SetContexts(1, "833437");
      HttpProviderContainer.Instance.RegisterProvider<IPricingProvider, IscPricingProvider>();
      int originalRequestTypeId = CurrencyProviderEngineRequests.ValidateNonOrderRequest;
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (originalRequestTypeId * 1000) + 1;
      IPricingProvider provider = HttpProviderContainer.Instance.Resolve<IPricingProvider>();
      try
      {
        Assert.IsFalse(provider.DoesIscAffectPricing("inactive"));
      }
      finally
      {
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = originalRequestTypeId;
      }
    }

    [TestMethod]
    public void DoesIscAffectPricingExpiredReturnsFalse()
    {
      SetContexts(1, "833437");
      int originalRequestTypeId = CurrencyProviderEngineRequests.ValidateNonOrderRequest;
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (originalRequestTypeId * 1000) + 1;
      IPricingProvider provider = HttpProviderContainer.Instance.Resolve<IPricingProvider>();
      try
      {
        Assert.IsFalse(provider.DoesIscAffectPricing("expired"));
      }
      finally
      {
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = originalRequestTypeId;
      }
    }
  }
}
