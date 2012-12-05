using Atlantis.Framework.Interface;
using Atlantis.Framework.PLSignupInfo.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Interface.Pricing;
using Atlantis.Framework.Providers.Interface.PromoData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Atlantis.Framework.Providers.Currency
{
  public enum ConversionRoundingType
  {
    Round = 0,
    Ceiling = 1,
    Floor = 2
  }

  public class CurrencyProvider : ProviderBase, ICurrencyProvider
  {
    #region Static Properties

    private static bool _useCookies = false;
    /// <summary>
    /// Only used if the ShopperPreferences Provider cannot be resolved
    /// </summary>
    public static bool UseLegacyCookies
    {
      get { return _useCookies; }
      set { _useCookies = value; }
    }

    #endregion

    #region Static Methods

    private static bool IsValidCurrencyType(string currencyType)
    {
      ICurrencyInfo currencyInfoItem = CurrencyData.GetCurrencyInfo(currencyType);
      return (currencyInfoItem != null);
    }

    #endregion

    const int RESELLER_CONTEXT = 6;
    private const string CURRENCY_TYPE_USD = "USD";
    private const string NOT_OFFERED_MSG_DEFAULT = "Product not offered.";
    private const string CURRENCY_PREFERENCE_KEY = "gdshop_currencyType";
    private const string LEGACY_CURRENCY_COOKIE_PREFIX = "currency";

    protected const string LEGACY_CURRENCY_COOKIE_PORTABLE_SOURCE_STR_KEY = "potableSourceStr";

    private Lazy<ICurrencyInfo> _USDInfo;
    private Lazy<ISiteContext> _siteContext;
    private Lazy<IShopperContext> _shopperContext;
    private Lazy<IShopperPreferencesProvider> _shopperPreferences;

    private ISiteContext SiteContext
    {
      get { return _siteContext.Value; }
    }

    private IShopperContext ShopperContext
    {
      get { return _shopperContext.Value; }
    }

    private IShopperPreferencesProvider ShopperPreferences
    {
      get { return _shopperPreferences.Value; }
    }


    private string _legacyCurrencyCookieName;
    private string LegacyCurrencyCookieName
    {
      get
      {
        if (string.IsNullOrEmpty(_legacyCurrencyCookieName))
        {
          _legacyCurrencyCookieName = LEGACY_CURRENCY_COOKIE_PREFIX + SiteContext.PrivateLabelId;
        }
        return _legacyCurrencyCookieName;
      }
    }

    /// <summary>
    /// Only used if the ShopperPreferences Provider cannot be resolved
    /// </summary>
    private string LegacyCurrencyCookieValue
    {
      get
      {
        string currencyValue = string.Empty;
        HttpCookie currencyCookie = HttpContext.Current.Request.Cookies[LegacyCurrencyCookieName];

        if (currencyCookie != null && !string.IsNullOrEmpty(currencyCookie[LEGACY_CURRENCY_COOKIE_PORTABLE_SOURCE_STR_KEY]))
        {
          currencyValue = currencyCookie[LEGACY_CURRENCY_COOKIE_PORTABLE_SOURCE_STR_KEY];
        }

        return currencyValue;
      }
      set
      {
        if (IsValidCurrencyType(value))
        {
          HttpCookie cookie = SiteContext.NewCrossDomainCookie(LegacyCurrencyCookieName, DateTime.Now.AddYears(1));
          if (cookie != null)
          {
            cookie[LEGACY_CURRENCY_COOKIE_PORTABLE_SOURCE_STR_KEY] = value;
            HttpContext.Current.Response.Cookies.Set(cookie);
          }
        }
      }
    }

    private readonly Dictionary<string, string> _priceFormatCache;

    private ICurrencyInfo _selectedDisplayCurrencyInfo;
    private ICurrencyInfo _selectedTransactionalCurrencyInfo;
    private string _selectedTransactionalCurrencyType;
    private string _selectedDisplayCurrencyType = null;

    public CurrencyProvider(IProviderContainer providerContainer)
      : base(providerContainer)
    {
      _priceFormatCache = new Dictionary<string, string>();

      _siteContext = new Lazy<ISiteContext>(() => { return Container.Resolve<ISiteContext>(); });
      _shopperContext = new Lazy<IShopperContext>(() => { return Container.Resolve<IShopperContext>(); });
      _shopperPreferences = new Lazy<IShopperPreferencesProvider>(
        () => { return Container.CanResolve<IShopperPreferencesProvider>() ? Container.Resolve<IShopperPreferencesProvider>() : null; });
      _USDInfo = new Lazy<ICurrencyInfo>(() => { return CurrencyData.GetCurrencyInfo(CURRENCY_TYPE_USD); });
    }

    #region Selected Currency Settings

    /// <summary>
    /// Returns or sets the shoppers selected Currency type
    /// </summary>
    public string SelectedDisplayCurrencyType
    {
      get
      {
        if (_selectedDisplayCurrencyType == null)
        {
          if (ShopperPreferences != null)
          {
            if (ShopperPreferences.HasPreference(CURRENCY_PREFERENCE_KEY))
            {
              _selectedDisplayCurrencyType = ShopperPreferences.GetPreference(CURRENCY_PREFERENCE_KEY, string.Empty);
            }
          }
          else if (UseLegacyCookies)
          {
            // If the ShopperPreferences provider could not be resolved, we have to manually grab the legacy cookie value
            _selectedDisplayCurrencyType = LegacyCurrencyCookieValue;
          }

          if ((string.IsNullOrEmpty(_selectedDisplayCurrencyType)) || (!IsValidCurrencyType(_selectedDisplayCurrencyType)))
          {
            _selectedDisplayCurrencyType = CURRENCY_TYPE_USD;

            if ((IsMultiCurrencyActiveForContext) && (SiteContext.ContextId == RESELLER_CONTEXT))
            {
              if ((PLSignupInfoData != null) && (IsValidCurrencyType(PLSignupInfoData.DefaultTransactionCurrencyType)))
              {
                _selectedDisplayCurrencyType = PLSignupInfoData.DefaultTransactionCurrencyType;
              }
            }
          }
        }

        return _selectedDisplayCurrencyType;
      }
      set
      {
        if (IsValidCurrencyType(value))
        {
          if (ShopperPreferences != null)
          {
            ShopperPreferences.UpdatePreference(CURRENCY_PREFERENCE_KEY, value);
          }
          else if (UseLegacyCookies)
          {
            // If the ShopperPreferences provider could not be resolved, we have to manually set the legacy cookie value
            LegacyCurrencyCookieValue = value;
          }

          _selectedDisplayCurrencyInfo = null;
          _selectedTransactionalCurrencyType = null;
          _selectedTransactionalCurrencyInfo = null;
          _selectedDisplayCurrencyType = value;
        }
      }
    }

    /// <summary>
    /// Returns the ICurrencyInfo for the shoppers selected currency type
    /// </summary>
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

    private bool? _isMultiCurrencyActiveForContext = null;
    private bool IsMultiCurrencyActiveForContext
    {
      get
      {
        if (!_isMultiCurrencyActiveForContext.HasValue)
        {
          if (SiteContext.ContextId == RESELLER_CONTEXT)
          {
            _isMultiCurrencyActiveForContext =
              MultiCurrencyContexts.GetIsContextIdActive(SiteContext.ContextId) &&
              ((PLSignupInfoData != null) && (PLSignupInfoData.IsMultiCurrencyReseller));
          }
          else
          {
            _isMultiCurrencyActiveForContext = MultiCurrencyContexts.GetIsContextIdActive(SiteContext.ContextId);
          }
        }
        return _isMultiCurrencyActiveForContext.Value;
      }
    }

    private PLSignupInfoResponseData _plSignupInfoData = null;
    private bool plSignupInfoCalled = false;
    private PLSignupInfoResponseData PLSignupInfoData
    {
      get
      {
        if ((_plSignupInfoData == null) && (!plSignupInfoCalled))
        {
          plSignupInfoCalled = true;
          try
          {
            PLSignupInfoRequestData request = new PLSignupInfoRequestData(ShopperContext.ShopperId, string.Empty, string.Empty, SiteContext.Pathway, SiteContext.PageCount, SiteContext.PrivateLabelId);
            _plSignupInfoData = (PLSignupInfoResponseData)DataCache.DataCache.GetProcessRequest(request, CurrencyProviderEngineRequests.PLSignupInfo);
          }
          catch
          {
            _plSignupInfoData = null; // Engine will log the error once. 
          }
        }

        return _plSignupInfoData;
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
        isTransactional = (currencyToCheck.IsTransactional) && (IsMultiCurrencyActiveForContext);
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

    /// <summary>
    /// Returns the shoppers transactional currency based on their selected currency
    /// </summary>
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

    /// <summary>
    /// Returns the ICurrencyInfo for the shoppers selected transactional currency type
    /// </summary>
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

    #region Logging Missing Prices

    private const string _catalogPriceError = "33";
    private void LogMissingCatalogPrice(string call, int unifiedProductId, int shopperPriceType, int quantity, int privateLabelId, string currencyType)
    {
      string message = "Catalog Price Missing (USD conversion used)";
      string data = string.Concat(call, ":uid=", unifiedProductId.ToString(), ":plid=", privateLabelId.ToString(), ":cur=", currencyType, ":pricetype=", shopperPriceType.ToString(), ":q=", quantity.ToString());
      AtlantisException ex = new AtlantisException(call, _catalogPriceError, message, data, SiteContext, ShopperContext);
      Engine.Engine.LogAtlantisException(ex);
    }

    #endregion

    #region ListPrice

    private ICurrencyPrice LookupListPriceInt(int unifiedProductId, int shopperPriceType, ICurrencyInfo transactionalCurrencyInfo)
    {
      int listPrice;
      bool wasConverted;

      bool success = DataCache.DataCache.GetListPriceEx(SiteContext.PrivateLabelId, unifiedProductId, shopperPriceType, transactionalCurrencyInfo.CurrencyType, out listPrice, out wasConverted);
      CurrencyPriceType currencyPriceType = CurrencyPriceType.Transactional;

      if (wasConverted)
      {
        currencyPriceType = CurrencyPriceType.Converted;
        LogMissingCatalogPrice("CurrencyProvider.GetListPriceEx", unifiedProductId, shopperPriceType, 1, SiteContext.PrivateLabelId, transactionalCurrencyInfo.CurrencyType);
      }

      return new CurrencyPrice(listPrice, transactionalCurrencyInfo, currencyPriceType);
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
      int currentPrice;
      bool wasConverted;

      bool success = DataCache.DataCache.GetPromoPriceEx(SiteContext.PrivateLabelId, unifiedProductId, shopperPriceType, transactionalCurrencyInfo.CurrencyType, out currentPrice, out wasConverted);
      CurrencyPriceType currencyPriceType = CurrencyPriceType.Transactional;

      if (wasConverted)
      {
        currencyPriceType = CurrencyPriceType.Converted;
        LogMissingCatalogPrice("CurrencyProvider.GetPromoPriceEx", unifiedProductId, shopperPriceType, 1, SiteContext.PrivateLabelId, transactionalCurrencyInfo.CurrencyType);
      }

      return new CurrencyPrice(currentPrice, transactionalCurrencyInfo, currencyPriceType);
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

      if (Container.TryResolve(out pricingProvider) && pricingProvider.DoesIscAffectPricing(isc))
      {
        int pricingProviderPrice;

        if (pricingProvider.GetCurrentPrice(unifiedProductId, shopperPriceType, transactionCurrency.CurrencyType,
                                            out pricingProviderPrice, isc))
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
      int currentPrice;
      bool wasConverted;

      bool success = DataCache.DataCache.GetPromoPriceByQtyEx(SiteContext.PrivateLabelId, unifiedProductId, shopperPriceType, quantity, transactionalCurrencyInfo.CurrencyType, out currentPrice, out wasConverted);
      CurrencyPriceType currencyPriceType = CurrencyPriceType.Transactional;

      if (wasConverted)
      {
        currencyPriceType = CurrencyPriceType.Converted;
        LogMissingCatalogPrice("CurrencyProvider.GetPromoPriceByQtyEx", unifiedProductId, shopperPriceType, quantity, SiteContext.PrivateLabelId, transactionalCurrencyInfo.CurrencyType);
      }

      return new CurrencyPrice(currentPrice, transactionalCurrencyInfo, currencyPriceType);
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


    public bool IsProductOnSale(int unifiedProductId, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null)
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

      bool result = DataCache.DataCache.IsProductOnSaleForCurrency(SiteContext.PrivateLabelId, unifiedProductId, transactionCurrency.CurrencyType);
      
      IPricingProvider pricingProvider;
      if (Container.TryResolve(out pricingProvider) && pricingProvider.DoesIscAffectPricing(isc))
      {
        int pricingProviderPrice;
        bool foundIscBasedPrice = pricingProvider.GetCurrentPrice(unifiedProductId, shopperPriceType, transactionCurrency.CurrencyType, out pricingProviderPrice, isc);
        
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
        result = FormatPriceInt(SelectedDisplayCurrencyInfo, "XXX", formatOptions);
      }
      else if ((price.Price < 0) && (!textOptions.HasFlag(PriceTextOptions.AllowNegativePrice)))
      {
        result = NOT_OFFERED_MSG_DEFAULT;
      }
      else
      {
        ICurrencyPrice convertedPrice = ConvertPriceInt(price, SelectedDisplayCurrencyInfo, CurrencyConversionRoundingType.Round);
        result = FormatPriceInt(convertedPrice, formatOptions);
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

      return FormatPriceInt(currencyPrice, options);
    }

    public string PriceFormat(ICurrencyPrice price, PriceFormatOptions options = PriceFormatOptions.None)
    {
      return FormatPriceInt(price, options);
    }

    private string GetPriceFormatCacheKey(ICurrencyInfo currencyInfo, string processedPrice, PriceFormatOptions options)
    {
      return string.Concat(currencyInfo.CurrencyType, "|", ((int)options).ToString(), "|", processedPrice);
    }

    private string FormatPriceInt(ICurrencyPrice currencyPrice, PriceFormatOptions options)
    {
      return FormatPriceInt(currencyPrice.CurrencyInfo, currencyPrice.Price.ToString(), options);
    }

    private string FormatPriceInt(ICurrencyInfo currencyInfoItem, string processedPrice, PriceFormatOptions options)
    {
      string formatKey = GetPriceFormatCacheKey(currencyInfoItem, processedPrice, options);
      if (_priceFormatCache.ContainsKey(formatKey))
      {
        return _priceFormatCache[formatKey];
      }
      else
      {
        string result = FormatPriceBuild(currencyInfoItem, processedPrice, options);
        _priceFormatCache[formatKey] = result;
        return result;
      }
    }

    private string FormatPriceBuild(ICurrencyInfo currencyInfoItem, string processedPrice, PriceFormatOptions options)
    {
      string workingPrice = processedPrice;

      bool isNegative = workingPrice.Contains("-");
      if (isNegative)
      {
        workingPrice = workingPrice.Replace("-", string.Empty);
      }

      int padCount = currencyInfoItem.DecimalPrecision + 1;
      workingPrice = workingPrice.PadLeft(padCount, '0');

      string decimalChars = string.Empty;
      string nonDecimalChars;
      string thousandsChars = string.Empty;
      string millionsChars = string.Empty;

      if (currencyInfoItem.DecimalPrecision > 0)
      {
        if (!options.HasFlag(PriceFormatOptions.DropDecimal))
        {
          decimalChars = currencyInfoItem.DecimalSeparator + workingPrice.Substring(workingPrice.Length - currencyInfoItem.DecimalPrecision);
        }
        nonDecimalChars = workingPrice.Substring(0, workingPrice.Length - currencyInfoItem.DecimalPrecision);
      }
      else
      {
        nonDecimalChars = workingPrice;
      }

      if ((nonDecimalChars.Length > 3) && (currencyInfoItem.ThousandsSeparator.Length > 0))
      {
        thousandsChars = nonDecimalChars.Substring(0, nonDecimalChars.Length - 3) + currencyInfoItem.ThousandsSeparator;
        nonDecimalChars = nonDecimalChars.Substring(nonDecimalChars.Length - 3);

        int threePlusSeparator = 3 + currencyInfoItem.ThousandsSeparator.Length;
        if ((thousandsChars.Length > threePlusSeparator))
        {
          millionsChars = thousandsChars.Substring(0, thousandsChars.Length - threePlusSeparator) + currencyInfoItem.ThousandsSeparator;
          thousandsChars = thousandsChars.Substring(thousandsChars.Length - threePlusSeparator);
        }
      }

      string currencySymbol;

      /// Be careful if trying to combine these.  The option flag has to take precendence over the
      /// global default property
      if (options.HasFlag(PriceFormatOptions.AsciiSymbol))
      {
        currencySymbol = currencyInfoItem.Symbol;
      }
      else if (options.HasFlag(PriceFormatOptions.HtmlSymbol))
      {
        currencySymbol = currencyInfoItem.SymbolHtml;
      }
      else if (CurrencyProviderOptions.UseHtmlCurrencySymbols)
      {
        currencySymbol = currencyInfoItem.SymbolHtml;
      }
      else
      {
        currencySymbol = currencyInfoItem.Symbol;
      }

      StringBuilder resultBuilder = new StringBuilder();
      if (!options.HasFlag(PriceFormatOptions.DropSymbol) && currencyInfoItem.SymbolPosition == CurrencySymbolPositionType.Prefix)
      {
        resultBuilder.Append(currencySymbol);
      }
      resultBuilder.Append(millionsChars);
      resultBuilder.Append(thousandsChars);
      resultBuilder.Append(nonDecimalChars);
      resultBuilder.Append(decimalChars);
      if (!options.HasFlag(PriceFormatOptions.DropSymbol) && currencyInfoItem.SymbolPosition == CurrencySymbolPositionType.Suffix)
      {
        resultBuilder.Append(currencySymbol);
      }

      if (isNegative)
      {
        if (options.HasFlag(PriceFormatOptions.NegativeParentheses))
        {
          resultBuilder.Insert(0, "(");
          resultBuilder.Append(")");
        }
        else
        {
          resultBuilder.Insert(0, "-");
        }
      }

      return resultBuilder.ToString();
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

    public ICurrencyInfo GetCurrencyInfo(string currencyType)
    {
      return CurrencyData.GetCurrencyInfo(currencyType);
    }

    public IEnumerable<ICurrencyInfo> CurrencyInfoList
    {
      get { return CurrencyData.CurrencyInfoList; }
    }

    public ICurrencyInfo GetValidCurrencyInfo(string currencyType)
    {
      ICurrencyInfo result = CurrencyData.GetCurrencyInfo(currencyType);
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
