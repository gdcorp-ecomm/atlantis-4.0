using Atlantis.Framework.Currency.Interface;
using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Pricing;
using Atlantis.Framework.Providers.Interface.PromoData;
using Atlantis.Framework.Providers.Localization.Interface;
using System;
using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Providers.Currency
{
  public enum ConversionRoundingType
  {
    Round = 0,
    Ceiling = 1,
    Floor = 2
  }

  internal static class ProviderErrors
  {
    internal static int CatalogPriceError = 33;
    internal static string CatalogPriceErrorMsg = "Catalog Price Missing (USD conversion used)";
    internal static int ListPriceNotFoundError = 34;
    internal static string ListPriceNotFoundErrorMsg = "List Price Not Found";
    internal static int PromoPriceNotFoundError = 35;
    internal static string PromoPriceNotFoundErrorMsg = "Promo Price Not Found";
  }

  public class CurrencyProvider : ProviderBase, ICurrencyProvider
  {
    private const string CURRENCY_TYPE_USD = "USD";
    private const string NOT_OFFERED_MSG_DEFAULT = "Product not offered.";
    private const string CURRENCY_PREFERENCE_KEY = "gdshop_currencyType";

    private Lazy<ICurrencyInfo> _USDInfo;
    private Lazy<ISiteContext> _siteContext;
    private Lazy<IShopperContext> _shopperContext;
    private Lazy<CurrencyTypesResponseData> _currencyData;
    private Lazy<int> _priceGroupId;
    private Lazy<CurrencyFormatting> _currencyFormatting;
    private Lazy<CurrencyPreference> _currencyPreference;

    private ISiteContext SiteContext
    {
      get { return _siteContext.Value; }
    }

    private IShopperContext ShopperContext
    {
      get { return _shopperContext.Value; }
    }

    private ICurrencyInfo _selectedDisplayCurrencyInfo;
    private ICurrencyInfo _selectedTransactionalCurrencyInfo;
    private string _selectedTransactionalCurrencyType;
    private string _selectedDisplayCurrencyType = null;

    public CurrencyProvider(IProviderContainer providerContainer)
      : base(providerContainer)
    {
      _siteContext = new Lazy<ISiteContext>(() => { return Container.Resolve<ISiteContext>(); });
      _shopperContext = new Lazy<IShopperContext>(() => { return Container.Resolve<IShopperContext>(); });      

      _USDInfo = new Lazy<ICurrencyInfo>(
        () => { return _currencyData.Value[CURRENCY_TYPE_USD]; });

      _currencyData = new Lazy<CurrencyTypesResponseData>(GetCurrencyTypes);

      _priceGroupId = new Lazy<int>(GetPriceGroupId);
      _currencyFormatting = new Lazy<CurrencyFormatting>(() => { return new CurrencyFormatting(Container); });
      _currencyPreference = new Lazy<CurrencyPreference>(() => { return new CurrencyPreference(Container); });

    }

    #region Selected Currency Settings

    public string SelectedDisplayCurrencyType
    {
      get
      {
        if (_selectedDisplayCurrencyType == null)
        {
          _selectedDisplayCurrencyType = _currencyPreference.Value.GetCurrencyPreference();
          if ((string.IsNullOrEmpty(_selectedDisplayCurrencyType)) || (!IsValidCurrencyType(_selectedDisplayCurrencyType)))
          {
            _selectedDisplayCurrencyType = CURRENCY_TYPE_USD;
          }
        }

        return _selectedDisplayCurrencyType;
      }
      set
      {
        if (IsValidCurrencyType(value))
        {
          _currencyPreference.Value.SetCurrencyPreference(value);

          _selectedDisplayCurrencyInfo = null;
          _selectedTransactionalCurrencyType = null;
          _selectedTransactionalCurrencyInfo = null;
          _selectedDisplayCurrencyType = value;
        }
      }
    }

    public ICurrencyInfo SelectedDisplayCurrencyInfo
    {
      get
      {
        if (_selectedDisplayCurrencyInfo == null)
        {
          _selectedDisplayCurrencyInfo = GetValidCurrencyInfo(SelectedDisplayCurrencyType);
        }
        return _selectedDisplayCurrencyInfo;
      }
    }

    public bool IsCurrencyTransactionalForContext(ICurrencyInfo currencyToCheck)
    {
      bool isTransactional = false;
      if (currencyToCheck.CurrencyType == CURRENCY_TYPE_USD)
      {
        isTransactional = true;
      }
      else
      {
        isTransactional = (currencyToCheck.IsTransactional) && (_currencyPreference.Value.IsMultiCurrencyActive);
      }
      return isTransactional;
    }

    private void SetSelectedTransactionalCurrency()
    {
      if (IsCurrencyTransactionalForContext(SelectedDisplayCurrencyInfo))
      {
        _selectedTransactionalCurrencyInfo = SelectedDisplayCurrencyInfo;
        _selectedTransactionalCurrencyType = SelectedDisplayCurrencyInfo.CurrencyType;
      }
      else
      {
        _selectedTransactionalCurrencyInfo = _USDInfo.Value;
        _selectedTransactionalCurrencyType = CURRENCY_TYPE_USD;
      }
    }

    public string SelectedTransactionalCurrencyType
    {
      get
      {
        if (_selectedTransactionalCurrencyType == null)
        {
          SetSelectedTransactionalCurrency();
        }
        return _selectedTransactionalCurrencyType;
      }
    }

    public ICurrencyInfo SelectedTransactionalCurrencyInfo
    {
      get
      {
        if (_selectedTransactionalCurrencyInfo == null)
        {
          SetSelectedTransactionalCurrency();
        }
        return _selectedTransactionalCurrencyInfo;
      }
    }

    #endregion

    #region Pricing Functions

    //private PriceGroupsByCountrySiteResponseData GetPriceGroupsByCountrySite()
    //{
    //  PriceGroupsByCountrySiteResponseData result = null;
    //  var requestData = new PriceGroupsByCountrySiteRequestData();
    //  var responseData = (PriceGroupsByCountrySiteResponseData)DataCache.DataCache.GetProcessRequest(requestData, CurrencyProviderEngineRequests.PriceGroupsyCountrySiteRequest);
    //  result = responseData;
    //  return result;
    //}

    public int PriceGroupId
    {
      get { return _priceGroupId.Value; }
    }

    private int GetPriceGroupId()
    {
      int result = 0;
      ILocalizationProvider localizationProvider = Container.CanResolve<ILocalizationProvider>() ? Container.Resolve<ILocalizationProvider>() : null;

      if (localizationProvider != null)
      {
        result = localizationProvider.CountrySiteInfo != null ? localizationProvider.CountrySiteInfo.PriceGroupId : 0;
      }
      return result;
    }

    #region Logging Missing Prices

    private void LogMissingCatalogPrice(string call, int unifiedProductId, int shopperPriceType, int quantity, int privateLabelId, string currencyType)
    {
      string data = string.Concat(call, ":uid=", unifiedProductId.ToString(), ":plid=", privateLabelId.ToString(), ":cur=", currencyType, ":pricetype=", shopperPriceType.ToString(), ":q=", quantity.ToString());
      AtlantisException ex = new AtlantisException(call, ProviderErrors.CatalogPriceError, ProviderErrors.CatalogPriceErrorMsg, data);
      Engine.Engine.LogAtlantisException(ex);
    }

    private void LogListPriceNotFound(string call, int unifiedProductId, int shopperPriceType, int quantity, int privateLabelId, string currencyType)
    {
      string data = string.Concat(call, ":uid=", unifiedProductId.ToString(), ":plid=", privateLabelId.ToString(), ":cur=", currencyType, ":pricetype=", shopperPriceType.ToString(), ":q=", quantity.ToString());
      AtlantisException ex = new AtlantisException(call, ProviderErrors.ListPriceNotFoundError, ProviderErrors.ListPriceNotFoundErrorMsg, data);
      Engine.Engine.LogAtlantisException(ex);
    }

    private void LogPromoPriceNotFound(string call, int unifiedProductId, int shopperPriceType, int quantity, int privateLabelId, string currencyType)
    {
      string data = string.Concat(call, ":uid=", unifiedProductId.ToString(), ":plid=", privateLabelId.ToString(), ":cur=", currencyType, ":pricetype=", shopperPriceType.ToString(), ":q=", quantity.ToString());
      AtlantisException ex = new AtlantisException(call, ProviderErrors.PromoPriceNotFoundError, ProviderErrors.PromoPriceNotFoundErrorMsg, data);
      Engine.Engine.LogAtlantisException(ex);
    }

    #endregion

    #region ListPrice

    private ICurrencyPrice LookupListPriceInt(int unifiedProductId, int shopperPriceType, ICurrencyInfo transactionalCurrencyInfo)
    {
      var listPriceRequestData = new ListPriceRequestData(unifiedProductId, SiteContext.PrivateLabelId, shopperPriceType, transactionalCurrencyInfo.CurrencyType, PriceGroupId);
      var listPriceResponseData = (ListPriceResponseData)DataCache.DataCache.GetProcessRequest(listPriceRequestData, CurrencyProviderEngineRequests.ListPriceRequest);

      CurrencyPriceType currencyPriceType = CurrencyPriceType.Transactional;

      if (!listPriceResponseData.IsPriceFound)
      {
        LogListPriceNotFound("CurrencyProvider.EcommPricingListPriceRequest", unifiedProductId, shopperPriceType, 1, SiteContext.PrivateLabelId, transactionalCurrencyInfo.CurrencyType);
      }
      else if (listPriceResponseData.IsEstimate)
      {
        currencyPriceType = CurrencyPriceType.Converted;
        LogMissingCatalogPrice("CurrencyProvider.EcommPricingListPriceRequest", unifiedProductId, shopperPriceType, 1, SiteContext.PrivateLabelId, transactionalCurrencyInfo.CurrencyType);
      }

      return new CurrencyPrice(listPriceResponseData.Price, transactionalCurrencyInfo, currencyPriceType);
    }

    public ICurrencyPrice GetListPrice(int unifiedProductId, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null)
    {
      if (shopperPriceType == -1)
      {
        shopperPriceType = _shopperContext.Value.ShopperPriceType;
      }

      if (transactionCurrency == null)
      {
        transactionCurrency = SelectedTransactionalCurrencyInfo;
      }
      else if (!IsCurrencyTransactionalForContext(transactionCurrency))
      {
        transactionCurrency = _USDInfo.Value;
      }

      return LookupListPriceInt(unifiedProductId, shopperPriceType, transactionCurrency);
    }

    #endregion

    #region CurrentPrice

    private ICurrencyPrice LookupCurrentPriceInt(int unifiedProductId, int shopperPriceType, ICurrencyInfo transactionalCurrencyInfo)
    {
      return LookupCurrentPriceByQuantityInt(unifiedProductId, 1, shopperPriceType, transactionalCurrencyInfo);
    }

    public ICurrencyPrice GetCurrentPrice(int unifiedProductId, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null)
    {
      if (shopperPriceType == -1)
      {
        shopperPriceType = _shopperContext.Value.ShopperPriceType;
      }

      if (transactionCurrency == null)
      {
        transactionCurrency = SelectedTransactionalCurrencyInfo;
      }
      else if (!IsCurrencyTransactionalForContext(transactionCurrency))
      {
        transactionCurrency = _USDInfo.Value;
      }

      if (isc == null)
      {
        isc = _siteContext.Value.ISC;
      }

      IPricingProvider pricingProvider;
      ICurrencyPrice currentPrice = null;

      int yard = -1;
      if (Container.TryResolve(out pricingProvider) && pricingProvider.DoesIscAffectPricing(isc, out yard))
      {
        int pricingProviderPrice;

        if (pricingProvider.GetCurrentPrice(unifiedProductId, shopperPriceType, transactionCurrency.CurrencyType,
                                            out pricingProviderPrice, isc, PriceGroupId, yard))
        {
          currentPrice = new CurrencyPrice(pricingProviderPrice, transactionCurrency, CurrencyPriceType.Transactional);
        }
      }

      if (currentPrice == null)
      {
        currentPrice = LookupCurrentPriceInt(unifiedProductId, shopperPriceType, transactionCurrency);
      }
      ICurrencyPrice promoPrice = HasPromoData ? GetPromoPrice(unifiedProductId, currentPrice, shopperPriceType) : currentPrice;

      return promoPrice;
    }

    #endregion

    #region CurrentPriceByQuantity

    private ICurrencyPrice LookupCurrentPriceByQuantityInt(int unifiedProductId, int quantity, int shopperPriceType, ICurrencyInfo transactionalCurrencyInfo)
    {
      var requestData = new PromoPriceRequestData(unifiedProductId, SiteContext.PrivateLabelId, quantity, shopperPriceType, transactionalCurrencyInfo.CurrencyType, PriceGroupId);
      var responseData = (PromoPriceResponseData)DataCache.DataCache.GetProcessRequest(requestData, CurrencyProviderEngineRequests.PromoPriceRequest);

      CurrencyPriceType currencyPriceType = CurrencyPriceType.Transactional;

      if (!responseData.IsPriceFound)
      {
        LogPromoPriceNotFound("CurrencyProvider.EcommPricingPromoPriceRequest", unifiedProductId, shopperPriceType, 1, SiteContext.PrivateLabelId, transactionalCurrencyInfo.CurrencyType);
      }
      else if (responseData.IsEstimate)
      {
        currencyPriceType = CurrencyPriceType.Converted;
        LogMissingCatalogPrice("CurrencyProvider.EcommPricingPromoPriceRequest", unifiedProductId, shopperPriceType, 1, SiteContext.PrivateLabelId, transactionalCurrencyInfo.CurrencyType);
      }

      return new CurrencyPrice(responseData.Price, transactionalCurrencyInfo, currencyPriceType);
    }

    public ICurrencyPrice GetCurrentPriceByQuantity(int unifiedProductId, int quantity, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null)
    {
      if (shopperPriceType == -1)
      {
        shopperPriceType = _shopperContext.Value.ShopperPriceType;
      }

      if (transactionCurrency == null)
      {
        transactionCurrency = SelectedTransactionalCurrencyInfo;
      }
      else if (!IsCurrencyTransactionalForContext(transactionCurrency))
      {
        transactionCurrency = _USDInfo.Value;
      }

      if (quantity < 1) { quantity = 1; }

      ICurrencyPrice currentPrice = LookupCurrentPriceByQuantityInt(unifiedProductId, quantity, shopperPriceType, transactionCurrency);
      ICurrencyPrice promoPrice = HasPromoData ? GetPromoPrice(unifiedProductId, currentPrice, shopperPriceType) : currentPrice;
      return promoPrice;
    }

    #endregion

    #region IsProductOnSale

    public bool IsProductOnSale(int unifiedProductId, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null )
    {
      if (shopperPriceType == -1)
      {
        shopperPriceType = _shopperContext.Value.ShopperPriceType;
      }

      if (transactionCurrency == null)
      {
        transactionCurrency = SelectedTransactionalCurrencyInfo;
      }
      else if (!IsCurrencyTransactionalForContext(transactionCurrency))
      {
        transactionCurrency = _USDInfo.Value;
      }

      if (isc == null)
      {
        isc = _siteContext.Value.ISC;
      }

      var requestData = new ProductIsOnSaleRequestData(unifiedProductId, SiteContext.PrivateLabelId, shopperPriceType, transactionCurrency.CurrencyType, PriceGroupId);
      var responseData = (ProductIsOnSaleResponseData)DataCache.DataCache.GetProcessRequest(requestData, CurrencyProviderEngineRequests.ProductIsOnSaleRequest);
      bool result = responseData.IsOnSale;
      
      IPricingProvider pricingProvider;
      int yard = -1;
      if (Container.TryResolve(out pricingProvider) && pricingProvider.DoesIscAffectPricing(isc, out yard))
      {
        int pricingProviderPrice;
        bool foundIscBasedPrice = pricingProvider.GetCurrentPrice(unifiedProductId, shopperPriceType, transactionCurrency.CurrencyType, out pricingProviderPrice, isc, PriceGroupId, yard);
        
        if (foundIscBasedPrice && pricingProviderPrice > 0)
        {
          result = true;
        }
      }
      
      if ((!result) && (HasPromoData))
      {
        result = IsPromoSale(unifiedProductId, ShopperContext.ShopperPriceType, SelectedTransactionalCurrencyInfo);
      }
      return result;
    }

    #endregion

    #endregion

    #region PriceText functions

    public string PriceText(ICurrencyPrice price, bool maskPrices)
    {
      return PriceText(price, maskPrices, false, false, NOT_OFFERED_MSG_DEFAULT);
    }

    public string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal)
    {
      return PriceText(price, maskPrices, dropDecimal, false, NOT_OFFERED_MSG_DEFAULT);
    }

    public string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol)
    {
      return PriceText(price, maskPrices, dropDecimal, dropSymbol, NOT_OFFERED_MSG_DEFAULT);
    }

    public string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol, string notOfferedMessage)
    {
      PriceFormatOptions formatOptions = PriceFormatOptions.None;
      if (dropDecimal)
      {
        formatOptions |= PriceFormatOptions.DropDecimal;
      }

      if (dropSymbol)
      {
        formatOptions |= PriceFormatOptions.DropSymbol;
      }

      PriceTextOptions textOptions = PriceTextOptions.None;
      if (maskPrices)
      {
        textOptions |= PriceTextOptions.MaskPrices;
      }

      return ProcessPriceInt(price, textOptions, formatOptions);
    }

    public string PriceText(ICurrencyPrice price, bool maskPrices, CurrencyNegativeFormat negativeFormat)
    {
      return PriceText(price, maskPrices, false, false, negativeFormat);
    }

    public string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol, CurrencyNegativeFormat negativeFormat)
    {
      PriceFormatOptions formatOptions = PriceFormatOptions.None;
      if (dropDecimal)
      {
        formatOptions |= PriceFormatOptions.DropDecimal;
      }

      if (dropSymbol)
      {
        formatOptions |= PriceFormatOptions.DropSymbol;
      }

      if (negativeFormat == CurrencyNegativeFormat.Parentheses)
      {
        formatOptions |= PriceFormatOptions.NegativeParentheses;
      }

      PriceTextOptions textOptions = PriceTextOptions.None;
      if (maskPrices)
      {
        textOptions |= PriceTextOptions.MaskPrices;
      }

      if (negativeFormat != CurrencyNegativeFormat.NegativeNotAllowed)
      {
        textOptions |= PriceTextOptions.AllowNegativePrice;
      }

      return ProcessPriceInt(price, textOptions, formatOptions);
    }

    public string PriceText(ICurrencyPrice price, PriceTextOptions textOptions = PriceTextOptions.None, PriceFormatOptions formatOptions = PriceFormatOptions.None)
    {
      return ProcessPriceInt(price, textOptions, formatOptions);
    }

    private string ProcessPriceInt(ICurrencyPrice price, PriceTextOptions textOptions, PriceFormatOptions formatOptions)
    {
      string result;
      if (textOptions.HasFlag(PriceTextOptions.MaskPrices))
      {
        ICurrencyPrice priceToMask = NewCurrencyPrice(111, SelectedDisplayCurrencyInfo, CurrencyPriceType.Transactional);
        result = PriceFormat(priceToMask, formatOptions);
        result = result.Replace("1", "X");
      }
      else if ((price.Price < 0) && (!textOptions.HasFlag(PriceTextOptions.AllowNegativePrice)))
      {
        result = NOT_OFFERED_MSG_DEFAULT;
      }
      else
      {
        ICurrencyPrice convertedPrice = ConvertPriceInt(price, SelectedDisplayCurrencyInfo, CurrencyConversionRoundingType.Round);
        result = PriceFormat(convertedPrice, formatOptions);
      }
      return result;
    }

    public string PriceFormat(ICurrencyPrice currencyPrice, bool dropDecimal, bool dropSymbol)
    {
      return PriceFormat(currencyPrice, dropDecimal, dropSymbol, CurrencyNegativeFormat.Minus);
    }

    public string PriceFormat(ICurrencyPrice currencyPrice, bool dropDecimal, bool dropSymbol, CurrencyNegativeFormat negativeFormat)
    {
      PriceFormatOptions options = PriceFormatOptions.None;
      if (dropDecimal)
      {
        options |= PriceFormatOptions.DropDecimal;
      }

      if (dropSymbol)
      {
        options |= PriceFormatOptions.DropSymbol;
      }

      if (negativeFormat == CurrencyNegativeFormat.Parentheses)
      {
        options |= PriceFormatOptions.NegativeParentheses;
      }

      return PriceFormat(currencyPrice, options);
    }

    public string PriceFormat(ICurrencyPrice price, PriceFormatOptions options = PriceFormatOptions.None)
    {
      return _currencyFormatting.Value.FormatPrice(price, options);
    }

    #endregion

    #region Price Conversion

    private ICurrencyPrice ConvertPriceInt(ICurrencyPrice priceToConvert, ICurrencyInfo targetCurrencyInfo, CurrencyConversionRoundingType conversionRoundingType)
    {
      ICurrencyPrice result = priceToConvert;

      if ((priceToConvert != null) && (targetCurrencyInfo != null))
      {
        if ((priceToConvert.Type == CurrencyPriceType.Transactional) && (priceToConvert.CurrencyInfo.Equals(_USDInfo.Value)))
        {
          if ((!priceToConvert.CurrencyInfo.Equals(targetCurrencyInfo)) && (targetCurrencyInfo.ExchangeRatePricing > 0))
          {
            double convertedDouble;
            if (conversionRoundingType == CurrencyConversionRoundingType.Ceiling)
            {
              convertedDouble = Math.Ceiling(priceToConvert.Price / targetCurrencyInfo.ExchangeRatePricing);
            }
            else if (conversionRoundingType == CurrencyConversionRoundingType.Floor)
            {
              convertedDouble = Math.Floor(priceToConvert.Price / targetCurrencyInfo.ExchangeRatePricing);
            }
            else
            {
              convertedDouble = Math.Round(priceToConvert.Price / targetCurrencyInfo.ExchangeRatePricing);
            }

            int convertedPrice = ConvertToInt32NoOverflow(convertedDouble);
            result = new CurrencyPrice(convertedPrice, targetCurrencyInfo, CurrencyPriceType.Converted);
          }
        }
        else if ((targetCurrencyInfo.Equals(_USDInfo.Value)) && (!priceToConvert.CurrencyInfo.Equals(_USDInfo.Value)))
        {
          double convertedDouble;
          if (conversionRoundingType == CurrencyConversionRoundingType.Ceiling)
          {
            convertedDouble = Math.Ceiling(priceToConvert.Price * targetCurrencyInfo.ExchangeRatePricing);
          }
          else if (conversionRoundingType == CurrencyConversionRoundingType.Floor)
          {
            convertedDouble = Math.Floor(priceToConvert.Price * targetCurrencyInfo.ExchangeRatePricing);
          }
          else
          {
            convertedDouble = Math.Round(priceToConvert.Price * priceToConvert.CurrencyInfo.ExchangeRatePricing);
          }

          int convertedPrice = ConvertToInt32NoOverflow(convertedDouble);
          result = new CurrencyPrice(convertedPrice, targetCurrencyInfo, CurrencyPriceType.Converted);
        }
      }

      return result;
    }

    public ICurrencyPrice ConvertPrice(ICurrencyPrice priceToConvert, ICurrencyInfo targetCurrencyInfo, CurrencyConversionRoundingType conversionRoundingType = CurrencyConversionRoundingType.Round)
    {
      return ConvertPriceInt(priceToConvert, targetCurrencyInfo, conversionRoundingType);
    }

    [Obsolete("Please use the ConvertPrice method from the ICurrencyProvider instead of this static method.")]
    public static ICurrencyPrice ConvertPrice(ICurrencyPrice priceToConvert, ICurrencyInfo targetCurrencyInfo)
    {
      return ConvertPrice(priceToConvert, targetCurrencyInfo, ConversionRoundingType.Round);
    }

    [Obsolete("Please use the ConvertPrice method from the ICurrencyProvider instead of this static method.")]
    public static ICurrencyPrice ConvertPrice(ICurrencyPrice priceToConvert, ICurrencyInfo targetCurrencyInfo, ConversionRoundingType conversionRoundingType)
    {
      ICurrencyPrice result = priceToConvert;

      if ((priceToConvert != null) && (targetCurrencyInfo != null))
      {
        if ((priceToConvert.Type == CurrencyPriceType.Transactional) && (priceToConvert.CurrencyInfo.CurrencyType.Equals(CURRENCY_TYPE_USD, StringComparison.InvariantCultureIgnoreCase)))
        {
          if ((!priceToConvert.CurrencyInfo.Equals(targetCurrencyInfo)) && (targetCurrencyInfo.ExchangeRatePricing > 0))
          {
            double convertedDouble;
            if (conversionRoundingType == ConversionRoundingType.Ceiling)
            {
              convertedDouble = Math.Ceiling(priceToConvert.Price / targetCurrencyInfo.ExchangeRatePricing);
            }
            else if (conversionRoundingType == ConversionRoundingType.Floor)
            {
              convertedDouble = Math.Floor(priceToConvert.Price / targetCurrencyInfo.ExchangeRatePricing);
            }
            else
            {
              convertedDouble = Math.Round(priceToConvert.Price / targetCurrencyInfo.ExchangeRatePricing);
            }

            int convertedPrice = ConvertToInt32NoOverflow(convertedDouble);
            result = new CurrencyPrice(convertedPrice, targetCurrencyInfo, CurrencyPriceType.Converted);
          }
        }
      }

      return result;
    }

    private static int ConvertToInt32NoOverflow(double value)
    {
      if (value >= int.MaxValue)
      {
        return int.MaxValue;
      }
      else if (value <= int.MinValue)
      {
        return int.MinValue;
      }

      return (int)value;
    }

    #endregion

    #region Promo Pricing

    private bool _skipPromoDataProviderCheck = false;

    private IPromoDataProvider _promoData;
    private IPromoDataProvider PromoData
    {
      get
      {
        if (!_skipPromoDataProviderCheck && _promoData == null && Container.CanResolve<IPromoDataProvider>())
        {
          _promoData = Container.Resolve<IPromoDataProvider>();
        }
        else
        {
          _skipPromoDataProviderCheck = true;
        }

        return _promoData;
      }
    }

    private bool? _hasPromoData;
    protected bool HasPromoData
    {
      get
      {
        if (!_hasPromoData.HasValue)
        {
          _hasPromoData = (PromoData != null && PromoData.HasPromoCodes);
        }

        return _hasPromoData.Value;
      }
    }

    private Dictionary<int, string> _ProductPromoCodes = new Dictionary<int, string>();

    private ICurrencyPrice GetPromoPrice(int unifiedProductId, ICurrencyPrice currentPrice, int shopperPriceType)
    {
      int? promoPrice = GetProductPromoPrice(unifiedProductId, currentPrice, shopperPriceType);

      if (!promoPrice.HasValue)
      {
        return currentPrice;
      }
      else
      {
        return new CurrencyPrice(promoPrice.Value, currentPrice.CurrencyInfo, currentPrice.Type);
      }
    }

    private int? GetProductPromoPrice(int unifiedProductId, ICurrencyPrice currentPrice, int shopperPriceType)
    {
      string key = string.Concat(unifiedProductId.ToString(),
        "|", shopperPriceType.ToString(),
        "|", currentPrice.CurrencyInfo.CurrencyType);

      return GetPromoPriceFromDictionary(unifiedProductId, currentPrice, key);
    }

    private int? GetPromoPriceFromDictionary(int unifiedProductId, ICurrencyPrice currentPrice, string key)
    {
      int? promoPrice;

      if (!this._productPromoPriceByShopperAndCurrencyTypes.TryGetValue(key, out promoPrice))
      {
        promoPrice = CalculatePromoPrice(unifiedProductId, currentPrice);

        if (promoPrice.HasValue && (promoPrice < currentPrice.Price))
        {
          this._productPromoPriceByShopperAndCurrencyTypes[key] = promoPrice;
        }
        else
        {
          this._productPromoPriceByShopperAndCurrencyTypes[key] = null;
        }
      }

      return promoPrice;
    }

    private Dictionary<string, int?> _productPromoPriceByShopperAndCurrencyTypes = new Dictionary<string, int?>(StringComparer.InvariantCultureIgnoreCase);

    private bool IsPromoSale(int unifiedProductId, int shopperPriceType, ICurrencyInfo transactionalCurrencyInfo)
    {
      int? promoPrice;
      string key = string.Concat(unifiedProductId.ToString(),
        "|", shopperPriceType.ToString(),
        "|", transactionalCurrencyInfo.CurrencyType);

      if (!this._productPromoPriceByShopperAndCurrencyTypes.TryGetValue(key, out promoPrice))
      {
        ICurrencyPrice currentPrice = LookupCurrentPriceInt(unifiedProductId, shopperPriceType, transactionalCurrencyInfo);
        promoPrice = GetPromoPriceFromDictionary(unifiedProductId, currentPrice, key);
      }

      return promoPrice.HasValue;
    }

    private int? CalculatePromoPrice(int unifiedProductId, ICurrencyPrice currentPrice)
    {
      string awardType;
      int? price = null;

      if (PromoData != null)
      {
        IPromoData pd = PromoData.GetProductPromoData();

        if (pd != null)
        {
          int awardAmound = pd.GetAwardAmount(unifiedProductId, currentPrice.CurrencyInfo.CurrencyType, out awardType);

          if (awardType.Equals("AmountOff", StringComparison.InvariantCultureIgnoreCase))
          {
            price = currentPrice.Price - awardAmound;
          }
          else if (awardType.Equals("PercentOff", StringComparison.InvariantCultureIgnoreCase))
          {
            price = Convert.ToInt32(currentPrice.Price * (100 - awardAmound) / 100);
          }
          else if (awardType.Equals("SetAmount", StringComparison.InvariantCultureIgnoreCase)
            && (currentPrice.Price > awardAmound))
          {
            price = awardAmound;
          }
        }
      }

      return price;
    }

    #endregion Promo Pricing

    #region CurrencyData

    private CurrencyTypesResponseData GetCurrencyTypes()
    {
      CurrencyTypesRequestData requestData = new CurrencyTypesRequestData();
      CurrencyTypesResponseData responseData = (CurrencyTypesResponseData)DataCache.DataCache.GetProcessRequest(requestData, CurrencyProviderEngineRequests.CurrencyTypesRequest);
      return responseData;
    }

    private bool IsValidCurrencyType(string currencyType)
    {
      ICurrencyInfo currencyInfoItem = _currencyData.Value[currencyType];
      return (currencyInfoItem != null);
    }

    public ICurrencyInfo GetCurrencyInfo(string currencyType)
    {
      return _currencyData.Value[currencyType];
    }

    public IEnumerable<ICurrencyInfo> CurrencyInfoList
    {
      get { return _currencyData.Value; }
    }

    public ICurrencyInfo GetValidCurrencyInfo(string currencyType)
    {
      ICurrencyInfo result = _currencyData.Value[currencyType];
      if (result == null)
      {
        result = _USDInfo.Value;
        if (result == null)
        {
          string message = "Critical currency error. Could not get valid currency info for USD or " + HttpUtility.HtmlEncode(currencyType) + ".";
          throw new Exception(message);
        }
      }
      return result;
    }

    #endregion

    #region Helper Functions

    public ICurrencyPrice NewCurrencyPrice(int price, ICurrencyInfo currencyInfo, CurrencyPriceType currencyPriceType)
    {
      return new CurrencyPrice(price, currencyInfo, currencyPriceType);
    }

    public ICurrencyPrice NewCurrencyPriceFromUSD(int usdPrice, ICurrencyInfo currencyInfo = null, CurrencyConversionRoundingType roundingType = CurrencyConversionRoundingType.Round)
    {
      if (currencyInfo == null)
      {
        currencyInfo = SelectedTransactionalCurrencyInfo;
      }
      else if (!IsCurrencyTransactionalForContext(currencyInfo))
      {
        currencyInfo = _USDInfo.Value;
      }

      ICurrencyPrice usdCurrencyPrice = new CurrencyPrice(usdPrice, _USDInfo.Value, CurrencyPriceType.Transactional);
      return ConvertPriceInt(usdCurrencyPrice, currencyInfo, roundingType);
    }

    #endregion
  }
}
