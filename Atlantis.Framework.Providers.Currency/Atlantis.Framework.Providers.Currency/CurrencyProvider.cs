using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Interface.PromoData;

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

    private static ICurrencyInfo GetValidCurrencyInfoItem(string currencyType)
    {
      ICurrencyInfo result = CurrencyData.GetCurrencyInfo(currencyType);
      if (result == null)
      {
        result = CurrencyData.GetCurrencyInfo(CURRENCY_TYPE_USD);
        if (result == null)
        {
          string message = "Critical currency error. Could not get valid currency info for USD or " + HttpUtility.HtmlEncode(currencyType) + ".";
          throw new Exception(message);
        }
      }
      return result;
    }

    #endregion

    private const string CURRENCY_TYPE_USD = "USD";
    private const string NOT_OFFERED_MSG_DEFAULT = "Product not offered.";
    private const string CURRENCY_PREFERENCE_KEY = "gdshop_currencyType";
    private const string LEGACY_CURRENCY_COOKIE_PREFIX = "currency";
    protected const string LEGACY_CURRENCY_COOKIE_PORTABLE_SOURCE_STR_KEY = "potableSourceStr";

    private ISiteContext _siteContext;
    private ISiteContext SiteContext
    {
      get
      {
        if(_siteContext == null)
        {
          _siteContext = Container.Resolve<ISiteContext>();
        }

        return _siteContext;
      }
    }

    private IShopperContext _shopperContext;
    private IShopperContext ShopperContext
    {
      get
      {
        if(_shopperContext == null)
        {
          _shopperContext = Container.Resolve<IShopperContext>();
        }

        return _shopperContext;
      }
    }

    private IShopperPreferencesProvider _shopperPreferences;
    private IShopperPreferencesProvider ShopperPreferences
    {
      get
      {
        if (_shopperPreferences == null && Container.CanResolve<IShopperPreferencesProvider>())
        {
          _shopperPreferences = Container.Resolve<IShopperPreferencesProvider>();
        }
        return _shopperPreferences;
      }
    }

    private string _legacyCurrencyCookieName;
    private string LegacyCurrencyCookieName
    {
      get
      {
        if(string.IsNullOrEmpty(_legacyCurrencyCookieName))
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

    private readonly Dictionary<string, string> _priceTextCache;
    
    private ICurrencyInfo _selectedDisplayCurrencyInfo;
    private ICurrencyInfo _selectedTransactionalCurrencyInfo;
    private string _selectedTransactionalCurrencyType;

    public CurrencyProvider(IProviderContainer providerContainer) : base(providerContainer)
    {
      _priceTextCache = new Dictionary<string, string>();
    }

    #region Selected Currency Settings

    /// <summary>
    /// Returns or sets the shoppers selected Currency type
    /// </summary>
    public string SelectedDisplayCurrencyType
    {
      get
      {
        string result = null;

        if (ShopperPreferences != null)
        {
          if (ShopperPreferences.HasPreference(CURRENCY_PREFERENCE_KEY))
          {
            result = ShopperPreferences.GetPreference(CURRENCY_PREFERENCE_KEY, string.Empty);
          }
        }
        else if (UseLegacyCookies)
        {
          // If the ShopperPreferences provider could not be resolved, we have to manually grab the legacy cookie value
          result = LegacyCurrencyCookieValue;
        }

        if ((string.IsNullOrEmpty(result)) || (!IsValidCurrencyType(result)))
        {
          result = CURRENCY_TYPE_USD;
        }
        return result;
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
          _selectedDisplayCurrencyInfo = GetValidCurrencyInfoItem(SelectedDisplayCurrencyType);
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
          _isMultiCurrencyActiveForContext = MultiCurrencyContexts.GetIsContextIdActive(SiteContext.ContextId);
        }
        return _isMultiCurrencyActiveForContext.Value;
      }
    }

    public bool IsCurrencyTransactionalForContext(ICurrencyInfo currencyToCheck)
    {
      bool isTransactional = false;
      if (currencyToCheck.CurrencyType == "USD")
      {
        isTransactional = true;
      }
      else
      {
        isTransactional = (currencyToCheck.IsTransactional) && (IsMultiCurrencyActiveForContext);
      }
      return isTransactional;
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
          _selectedTransactionalCurrencyType = CURRENCY_TYPE_USD;
          if (IsCurrencyTransactionalForContext(SelectedDisplayCurrencyInfo))
          {
            _selectedTransactionalCurrencyType = SelectedDisplayCurrencyInfo.CurrencyType;
          }
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
          _selectedTransactionalCurrencyInfo = GetValidCurrencyInfoItem(SelectedTransactionalCurrencyType);
        }
        return _selectedTransactionalCurrencyInfo;
      }
    }

    #endregion

    #region Pricing Functions

    private const string _catalogPriceError = "33";
    private void LogMissingCatalogPrice(string call, int unifiedProductId, int shopperPriceType, int quantity, int privateLabelId, string currencyType)
    {
      string message = "Catalog Price Missing (USD conversion used)";
      string data = string.Concat(call, ":uid=", unifiedProductId.ToString(), ":plid=", privateLabelId.ToString(), ":cur=", currencyType, ":pricetype=", shopperPriceType.ToString(), ":q=", quantity.ToString());
      AtlantisException ex = new AtlantisException(call, _catalogPriceError, message, data, SiteContext, ShopperContext);
      Engine.Engine.LogAtlantisException(ex);
    }

    /// <summary>
    /// Gets the List price of a product
    /// </summary>
    /// <param name="unifiedProductId">unified Product Id</param>
    /// <param name="shopperPriceType">shopper price type</param>
    /// <returns></returns>
    public ICurrencyPrice GetListPrice(int unifiedProductId, int shopperPriceType)
    {
      int listPrice;
      bool wasConverted;
      
      bool success = DataCache.DataCache.GetListPriceEx(SiteContext.PrivateLabelId, unifiedProductId, shopperPriceType, SelectedTransactionalCurrencyType, out listPrice, out wasConverted);

      CurrencyPriceType type = CurrencyPriceType.Transactional;
      if (wasConverted)
      {
        type = CurrencyPriceType.Converted;
        LogMissingCatalogPrice("CurrencyProvider.GetListPriceEx", unifiedProductId, shopperPriceType, 1, SiteContext.PrivateLabelId, SelectedTransactionalCurrencyType);
      }

      ICurrencyPrice result = new CurrencyPrice(listPrice, SelectedTransactionalCurrencyInfo, type);
      return result;
    }

    /// <summary>
    /// Gets the List price of a product
    /// </summary>
    /// <param name="unifiedProductId">unified Product Id</param>
    /// <returns></returns>
    public ICurrencyPrice GetListPrice(int unifiedProductId)
    {
      return GetListPrice(unifiedProductId, ShopperContext.ShopperPriceType);
    }

    /// <summary>
    /// Gets the Current (PromoPrice) price of a product
    /// </summary>
    /// <param name="unifiedProductId">unified Product Id</param>
    /// <param name="shopperPriceType">shopper price type</param>
    /// <returns></returns>
    public ICurrencyPrice GetCurrentPrice(int unifiedProductId, int shopperPriceType)
    {
      CurrencyPriceType type;
      int currentPrice = GetProductCurrentPrice(unifiedProductId, shopperPriceType, out type);
      int promoPrice = HasPromoData ? GetPromoPrice(unifiedProductId, currentPrice, shopperPriceType) : currentPrice;
      ICurrencyPrice result = new CurrencyPrice(promoPrice, SelectedTransactionalCurrencyInfo, type);
      return result;
    }

    private int GetProductCurrentPrice(int unifiedProductId, int shopperPriceType, out CurrencyPriceType currencyPriceType)
    {
      int currentPrice;
      bool wasConverted;

      bool success = DataCache.DataCache.GetPromoPriceEx(SiteContext.PrivateLabelId, unifiedProductId, 
        shopperPriceType, SelectedTransactionalCurrencyType, out currentPrice, out wasConverted);

      currencyPriceType = CurrencyPriceType.Transactional;

      if (wasConverted)
      {
        currencyPriceType = CurrencyPriceType.Converted;
        LogMissingCatalogPrice("CurrencyProvider.GetPromoPriceEx", unifiedProductId, shopperPriceType, 1, 
          SiteContext.PrivateLabelId, SelectedTransactionalCurrencyType);
      }

      return currentPrice;
    }

    /// <summary>
    /// Gets the Current (PromoPrice) price of a product
    /// </summary>
    /// <param name="unifiedProductId">unified Product Id</param>
    /// <returns></returns>
    public ICurrencyPrice GetCurrentPrice(int unifiedProductId)
    {
      return GetCurrentPrice(unifiedProductId, ShopperContext.ShopperPriceType);
    }

    /// <summary>
    /// Gets the Current (PromoPrice) price of a product
    /// </summary>
    /// <param name="unifiedProductId">unified Product Id</param>
    /// <param name="quantity">quantity of product</param>
    /// <param name="shopperPriceType">shopper price type</param>
    /// <returns></returns>
    public ICurrencyPrice GetCurrentPriceByQuantity(int unifiedProductId, int quantity, int shopperPriceType)
    {
      CurrencyPriceType type;
      int currentPrice = GetProductCurrentPriceByQuantity(unifiedProductId, quantity, shopperPriceType,  out type);
      int promoPrice = HasPromoData ? GetPromoPrice(unifiedProductId, currentPrice, shopperPriceType) : currentPrice;
      ICurrencyPrice result = new CurrencyPrice(promoPrice, SelectedTransactionalCurrencyInfo, type);
      return result;
    }

    private int GetProductCurrentPriceByQuantity(int unifiedProductId, int quantity, 
      int shopperPriceType, out CurrencyPriceType currencyPriceType)
    {
      int currentPrice;
      bool wasConverted;

      bool success = DataCache.DataCache.GetPromoPriceByQtyEx(SiteContext.PrivateLabelId, unifiedProductId, shopperPriceType, quantity, SelectedTransactionalCurrencyType, out currentPrice, out wasConverted);

      currencyPriceType = CurrencyPriceType.Transactional;

      if (wasConverted)
      {
        currencyPriceType = CurrencyPriceType.Converted;
        LogMissingCatalogPrice("CurrencyProvider.GetPromoPriceByQtyEx", unifiedProductId, shopperPriceType, quantity, SiteContext.PrivateLabelId, SelectedTransactionalCurrencyType);
      }

      return currentPrice;
    }

    /// <summary>
    /// Gets the Current (PromoPrice) price of a product
    /// </summary>
    /// <param name="unifiedProductId">unified Product Id</param>
    /// <param name="quantity">quantity of product</param>
    /// <returns></returns>
    public ICurrencyPrice GetCurrentPriceByQuantity(int unifiedProductId, int quantity)
    {
      return GetCurrentPriceByQuantity(unifiedProductId, quantity, ShopperContext.ShopperPriceType);
    }

    /// <summary>
    /// Returns true if product is on sale for the currently selected currency type
    /// </summary>
    /// <param name="unifiedProductId">unified Product Id</param>
    /// <returns>true of the product is on sale for the selected currency type</returns>
    public bool IsProductOnSale(int unifiedProductId)
    {
      bool result = DataCache.DataCache.IsProductOnSaleForCurrency(SiteContext.PrivateLabelId, 
        unifiedProductId, SelectedTransactionalCurrencyType);

      if ((!result) && (HasPromoData))
      {
        result = IsPromoSale(unifiedProductId, ShopperContext.ShopperPriceType);
      }
      return result;
    }

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
      return ProcessPrice(price, maskPrices, dropDecimal, dropSymbol, notOfferedMessage);
    }

    public string PriceText(ICurrencyPrice price, bool maskPrices, CurrencyNegativeFormat negativeFormat)
    {
      return PriceText(price, maskPrices, false, false, negativeFormat);
    }

    public string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol, CurrencyNegativeFormat negativeFormat)
    {
      return ProcessPrice(price, maskPrices, dropDecimal, dropSymbol, NOT_OFFERED_MSG_DEFAULT, negativeFormat);
    }

    private string GetPriceTextCacheKey(ICurrencyInfo displayCurrencyInfo, ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol, string notOfferedMessage, CurrencyNegativeFormat negativeFormat)
    {
      return string.Concat(displayCurrencyInfo.CurrencyType, "|", price.CurrencyInfo.CurrencyType, price.Price.ToString(), price.Type.ToString(), maskPrices.ToString(), dropDecimal.ToString(), dropSymbol.ToString(), notOfferedMessage, negativeFormat.ToString());
    }

    private string ProcessPrice(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol, string notOfferedMessage)
    {
      return ProcessPrice(price, maskPrices, dropDecimal, dropSymbol, notOfferedMessage, CurrencyNegativeFormat.NegativeNotAllowed);
    }

    private string ProcessPrice(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol, string notOfferedMessage, CurrencyNegativeFormat negativeFormat)
    {
      string result;

      string cacheKey = GetPriceTextCacheKey(SelectedDisplayCurrencyInfo, price, maskPrices, dropDecimal, dropSymbol, notOfferedMessage, negativeFormat);
      if (_priceTextCache.ContainsKey(cacheKey))
      {
        result = _priceTextCache[cacheKey];
      }
      else
      {
        if (maskPrices)
        {
          result = FormatPrice(SelectedDisplayCurrencyInfo, "XXX", dropDecimal, dropSymbol, negativeFormat);
        }
        else if ((price.Price < 0) && (negativeFormat == CurrencyNegativeFormat.NegativeNotAllowed))
        {
          result = notOfferedMessage;
        }
        else
        {
          ICurrencyPrice convertedPrice = ConvertPrice(price, SelectedDisplayCurrencyInfo);
          result = FormatPrice(convertedPrice, dropDecimal, dropSymbol, negativeFormat);
        }

        _priceTextCache[cacheKey] = result;
      }

      return result;
    }

    /// <summary>
    /// Returns the formatted price based on its currency type. NOTE this method is for use when you need
    /// to force a display of a price while ignoring the shoppers selected currency display setting.
    /// </summary>
    /// <param name="currencyPrice">Price with currency that you want to display</param>
    /// <param name="dropDecimal">true to drop the decimal and any digits after it.</param>
    /// <param name="dropSymbol">true to not have the symbol</param>
    /// <returns></returns>
    public string PriceFormat(ICurrencyPrice currencyPrice, bool dropDecimal, bool dropSymbol)
    {
      return PriceFormat(currencyPrice, dropDecimal, dropSymbol, CurrencyNegativeFormat.Minus);
    }

    /// <summary>
    /// Returns the formatted price based on its currency type. NOTE this method is for use when you need
    /// to force a display of a price while ignoring the shoppers selected currency display setting.
    /// </summary>
    /// <param name="currencyPrice">Price with currency that you want to display</param>
    /// <param name="dropDecimal">true to drop the decimal and any digits after it.</param>
    /// <param name="dropSymbol">true to not have the symbol</param>
    /// <param name="negativeFormat">If set to parentheses negatives will be in (parentheses), otherwise they will start with -minus</param>
    /// <returns></returns>
    public string PriceFormat(ICurrencyPrice currencyPrice, bool dropDecimal, bool dropSymbol, CurrencyNegativeFormat negativeFormat)
    {
      return FormatPrice(currencyPrice, dropDecimal, dropSymbol, negativeFormat);
    }

    private static string FormatPrice(ICurrencyPrice currencyPrice, bool dropDecimal, bool dropSymbol, CurrencyNegativeFormat negativeFormat)
    {
      return FormatPrice(currencyPrice.CurrencyInfo, currencyPrice.Price.ToString(), dropDecimal, dropSymbol, negativeFormat);
    }

    private static string FormatPrice(ICurrencyInfo currencyInfoItem, string processedPrice, bool dropDecimal, bool dropSymbol, CurrencyNegativeFormat negativeFormat)
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
        if (!dropDecimal)
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
      if (CurrencyProviderOptions.UseHtmlCurrencySymbols)
      {
        currencySymbol = currencyInfoItem.SymbolHtml;
      }
      else
      {
        currencySymbol = currencyInfoItem.Symbol;
      }

      StringBuilder resultBuilder = new StringBuilder();
      if (!dropSymbol && currencyInfoItem.SymbolPosition == CurrencySymbolPositionType.Prefix)
      {
        resultBuilder.Append(currencySymbol);
      }
      resultBuilder.Append(millionsChars);
      resultBuilder.Append(thousandsChars);
      resultBuilder.Append(nonDecimalChars);
      resultBuilder.Append(decimalChars);
      if (!dropSymbol && currencyInfoItem.SymbolPosition == CurrencySymbolPositionType.Suffix)
      {
        resultBuilder.Append(currencySymbol);
      }

      if (isNegative)
      {
        if (negativeFormat == CurrencyNegativeFormat.Parentheses)
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

    #region Static Methods

    public static ICurrencyPrice ConvertPrice(ICurrencyPrice priceToConvert, ICurrencyInfo targetCurrencyInfo)
    {
      return ConvertPrice(priceToConvert, targetCurrencyInfo, ConversionRoundingType.Round);
    }

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
            int convertedPrice = Int32.MaxValue;
            if (convertedDouble < Int32.MaxValue)
            {
              convertedPrice = Convert.ToInt32(convertedDouble);
            }
            result = new CurrencyPrice(convertedPrice, targetCurrencyInfo, CurrencyPriceType.Converted);
          }
        }
      }

      return result;
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

    private int GetPromoPrice(int unifiedProductId, int currentPrice, int shopperPriceType)
    {
      int? promoPrice = GetProductPromoPrice(unifiedProductId, currentPrice, shopperPriceType);

      if (!promoPrice.HasValue)
      {
        return currentPrice;
      }
      else
      {
        return promoPrice.Value;
      }
    }

    private int? GetProductPromoPrice(int unifiedProductId, int currentPrice, int shopperPriceType)
    {
      string key = string.Concat(unifiedProductId.ToString(),
        "|", shopperPriceType.ToString(),
        "|", SelectedTransactionalCurrencyType);

      return GetPromoPriceFromDictionary(unifiedProductId, currentPrice, key);
    }

    private int? GetPromoPriceFromDictionary(int unifiedProductId, int currentPrice, string key)
    {
      int? promoPrice;

      if (!this._productPromoPriceByShopperAndCurrencyTypes.TryGetValue(key, out promoPrice))
      {
        promoPrice = CalculatePromoPrice(unifiedProductId, currentPrice);

        if (promoPrice.HasValue && (promoPrice < currentPrice))
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

    private Dictionary<string, int?> _productPromoPriceByShopperAndCurrencyTypes
      = new Dictionary<string, int?>(StringComparer.InvariantCultureIgnoreCase);

    private bool IsPromoSale(int unifiedProductId, int shopperPriceType)
    {
      int? promoPrice;
      string key = string.Concat(unifiedProductId.ToString(), 
        "|", shopperPriceType.ToString(), 
        "|", SelectedTransactionalCurrencyType);

      if (!this._productPromoPriceByShopperAndCurrencyTypes.TryGetValue(key, out promoPrice))
      {
        CurrencyPriceType type;
        int currentPrice = GetProductCurrentPrice(unifiedProductId, ShopperContext.ShopperPriceType, out type);
        promoPrice = GetPromoPriceFromDictionary(unifiedProductId, currentPrice, key);
      }

      return promoPrice.HasValue;
    }

    private int? CalculatePromoPrice(int unifiedProductId, int currentPrice)
    {
      string awardType;
      int? price = null;

      if (PromoData != null)
      {
        IPromoData pd = PromoData.GetProductPromoData();

        if (pd != null)
        {
          int awardAmound = pd.GetAwardAmount(unifiedProductId,
            SelectedTransactionalCurrencyType, out awardType);

          if (awardType.Equals("AmountOff", StringComparison.InvariantCultureIgnoreCase))
          {
            price = currentPrice - awardAmound;
          }
          else if (awardType.Equals("PercentOff", StringComparison.InvariantCultureIgnoreCase))
          {
            price = Convert.ToInt32(currentPrice * (100 - awardAmound) / 100);
          }
          else if (awardType.Equals("SetAmount", StringComparison.InvariantCultureIgnoreCase)
            && (currentPrice > awardAmound))
          {
            price = awardAmound;
          }
        }
      }

      return price;
    }

    #endregion Promo Pricing
  }
}
