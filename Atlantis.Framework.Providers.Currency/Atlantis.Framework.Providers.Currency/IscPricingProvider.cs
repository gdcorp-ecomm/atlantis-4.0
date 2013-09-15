using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Pricing;
using System;

namespace Atlantis.Framework.Providers.Currency
{
  public class IscPricingProvider: ProviderBase, IPricingProvider
  {
    const string _ISCPRICINGACTIVESETTING ="ATLANTIS_PRICING_ISC_ACTIVE";
    public static bool DefaultEnabled = true;

    private Lazy<ISiteContext> _siteContext;
    private Lazy<bool> _isIscPricingProviderActive;
    
    public IscPricingProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => { return Container.Resolve<ISiteContext>(); });
      _isIscPricingProviderActive = new Lazy<bool>(() => { return LoadIsIscPricingProviderActive(); });      
      Enabled = IscPricingProvider.DefaultEnabled;
    }

    private bool LoadIsIscPricingProviderActive()
    {
      string iscPricingProviderActiveSetting = DataCache.DataCache.GetAppSetting(_ISCPRICINGACTIVESETTING);
      return iscPricingProviderActiveSetting != "OFF";
    }
    
    public bool DoesIscAffectPricing(string iscCode, out int yard)
    {
      yard = -1;
      if (!_isIscPricingProviderActive.Value)
      {
        return false;
      }

      if (!Enabled)
      {
        return false;
      }

      var requestData = new ValidateNonOrderRequestData(_siteContext.Value.PrivateLabelId, iscCode);
      var responseData = (ValidateNonOrderResponseData)DataCache.DataCache.GetProcessRequest(requestData, CurrencyProviderEngineRequests.ValidateNonOrderRequest);
      DateTime currentDate = DateTime.Now;
      
      bool result = (responseData.IsActive && responseData.StartDate <= currentDate && currentDate <= responseData.EndDate);

      if (result)
      {
        yard = responseData.YARD;
      }
      return result;
    }

    public bool GetCurrentPrice(int unifiedProductId, int shopperPriceType, string currencyType, out int price, string isc = "", int catalogId = 0, int yard = -1)
    {
      bool result = false;
      price = 0;

      if (!_isIscPricingProviderActive.Value)
      {
        return result;
      }

      if (!Enabled)
      {
        return result;
      }

      var requestData = new PriceEstimateRequestData(_siteContext.Value.PrivateLabelId, shopperPriceType, currencyType, isc, catalogId, unifiedProductId);
      requestData.YARD = yard;
      var responseData = (PriceEstimateResponseData)DataCache.DataCache.GetProcessRequest(requestData, CurrencyProviderEngineRequests.PriceEstimateRequest);

      if (responseData.IsPriceFound)
      {
        price = responseData.AdjustedPrice;
        result = responseData.IsPriceFound;
      }

      return result;
    }

    public bool Enabled { get; set; }

  }
}