using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Interface.Currency
{
  public interface ICurrencyProvider
  {
    /// <summary>
    /// Looks up the current price of a product.
    /// If pricetype is not given, it will use the shoppers pricetype
    /// If currencyInfo is not given it will use the shoppers selected transaction currency
    /// If the given currencytype is not transactional, the price will be converted to the given currencytype
    /// If isc is not given, it will use the ISiteContext.ISC
    /// </summary>
    /// <param name="unifiedProductId">unified product id</param>
    /// <param name="shopperPriceType">shopper price type to use.  If not given the shoppers pricetype will be used</param>
    /// <param name="transactionCurrency">currencyType to use. If not given the shoppers transactional currencyType will be used.<see cref="ICurrencyInfo"/></param>
    /// <param name="isc">isc to use. If not given the site context ISC will be used.</param>
    /// <returns>The current price of a product.</returns>
    ICurrencyPrice GetCurrentPrice(int unifiedProductId, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null);

    /// <summary>
    /// Looks up the current price of a product for a specfic region's PriceGroupId 
    /// If pricetype is not given, it will use the shoppers pricetype
    /// If currencyInfo is not given it will use the shoppers selected transaction currency
    /// If the given currencytype is not transactional, the price will be converted to the given currencytype
    /// If isc is not given, it will use the ISiteContext.ISC
    /// </summary>
    /// <param name="unifiedProductId">unified product id</param>
    /// <param name="priceGroupId">PriceGroupId for region</param>
    /// <param name="shopperPriceType">shopper price type to use.  If not given the shoppers pricetype will be used</param>
    /// <param name="transactionCurrency">currencyType to use. If not given the shoppers transactional currencyType will be used.<see cref="ICurrencyInfo"/></param>
    /// <param name="isc">isc to use. If not given the site context ISC will be used.</param>
    /// <returns>The current price of a product.</returns>
    ICurrencyPrice GetRegionalCurrentPrice(int unifiedProductId, int priceGroupId, int shopperPriceType = -1,
      ICurrencyInfo transactionCurrency = null, string isc = null);

    /// <summary>
    /// Looks up the current price of a product.
    /// If pricetype is not given, it will use the shoppers pricetype
    /// If currencyInfo is not given it will use the shoppers selected transaction currency
    /// If the given currencytype is not transactional, the price will be converted to the given currencytype
    /// If isc is not given, it will use the ISiteContext.ISC
    /// </summary>
    /// <param name="unifiedProductId">unified product id</param>
    /// <param name="quantity">quantity of product</param>
    /// <param name="shopperPriceType">shopper price type to use.  If not given the shoppers pricetype will be used</param>
    /// <param name="transactionCurrency">currencyType to use. If not given the shoppers transactional currencyType will be used.<see cref="ICurrencyInfo"/></param>
    /// <returns>The current price of a product.</returns>
    ICurrencyPrice GetCurrentPriceByQuantity(int unifiedProductId, int quantity, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null);

    /// <summary>
    /// Looks up the current price of a product for a specfic region's PriceGroupId
    /// If pricetype is not given, it will use the shoppers pricetype
    /// If currencyInfo is not given it will use the shoppers selected transaction currency
    /// If the given currencytype is not transactional, the price will be converted to the given currencytype
    /// If isc is not given, it will use the ISiteContext.ISC
    /// </summary>
    /// <param name="unifiedProductId">unified product id</param>
    /// <param name="priceGroupId">PriceGroupId for region</param>
    /// <param name="quantity">quantity of product</param>
    /// <param name="shopperPriceType">shopper price type to use.  If not given the shoppers pricetype will be used</param>
    /// <param name="transactionCurrency">currencyType to use. If not given the shoppers transactional currencyType will be used.<see cref="ICurrencyInfo"/></param>
    /// <returns>The current price of a product.</returns>
    ICurrencyPrice GetRegionalCurrentPriceByQuantity(int unifiedProductId, int priceGroupId, int quantity, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null);

    /// <summary>
    /// Looks up the list price of a product.
    /// If pricetype is not given, it will use the shoppers pricetype
    /// If currencyInfo is not given it will use the shoppers selected transaction currency
    /// If the given currencytype is not transactional, the price will be converted to the given currencytype
    /// If isc is not given, it will use the ISiteContext.ISC
    /// </summary>
    /// <param name="unifiedProductId">unified product id</param>
    /// <param name="shopperPriceType">shopper price type to use.  If not given the shoppers pricetype will be used</param>
    /// <param name="transactionCurrency">currencyType to use. If not given the shoppers transactional currencyType will be used.<see cref="ICurrencyInfo"/></param>
    /// <returns>The list price of a product.</returns>
    ICurrencyPrice GetListPrice(int unifiedProductId, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null);

    /// <summary>
    /// Looks up the list price of a product for a specfic region's PriceGroupId
    /// If pricetype is not given, it will use the shoppers pricetype
    /// If currencyInfo is not given it will use the shoppers selected transaction currency
    /// If the given currencytype is not transactional, the price will be converted to the given currencytype
    /// If isc is not given, it will use the ISiteContext.ISC
    /// </summary>
    /// <param name="unifiedProductId">unified product id</param>
    /// <param name="priceGroupId">PriceGroupId for region</param>
    /// <param name="shopperPriceType">shopper price type to use.  If not given the shoppers pricetype will be used</param>
    /// <param name="transactionCurrency">currencyType to use. If not given the shoppers transactional currencyType will be used.<see cref="ICurrencyInfo"/></param>
    /// <returns>The list price of a product.</returns>
    ICurrencyPrice GetRegionalListPrice(int unifiedProductId, int priceGroupId, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null);

    /// <summary>
    /// Converts a price to the shoppers display currency, then formats it for display based on currency info.
    /// </summary>
    /// <param name="price">Price to format</param>
    /// <param name="maskPrices">true will output the price with X's instead of numbers ($X.XX)</param>
    /// <returns>Price text for display</returns>
    [Obsolete("Please use the PriceText method with options flags.")]
    string PriceText(ICurrencyPrice price, bool maskPrices);

    /// <summary>
    /// Converts a price to the shoppers display currency, then formats it for display based on currency info.
    /// </summary>
    /// <param name="price">Price to format</param>
    /// <param name="maskPrices">true will output the price with X's instead of numbers ($X.XX)</param>
    /// <param name="dropDecimal">will show price without any decimal places.</param>
    /// <returns>Price text for display</returns>
    [Obsolete("Please use the PriceText method with options flags.")]
    string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal);

    /// <summary>
    /// Converts a price to the shoppers display currency, then formats it for display based on currency info.
    /// </summary>
    /// <param name="price">Price to format</param>
    /// <param name="maskPrices">true will output the price with X's instead of numbers ($X.XX)</param>
    /// <param name="dropDecimal">will show price without any decimal places.</param>
    /// <param name="dropSymbol">will show price wihtout the symbol</param>
    /// <returns>Price text for display</returns>
    [Obsolete("Please use the PriceText method with options flags.")]
    string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol);

    /// <summary>
    /// Converts a price to the shoppers display currency, then formats it for display based on currency info.
    /// </summary>
    /// <param name="price">Price to format</param>
    /// <param name="maskPrices">true will output the price with X's instead of numbers ($X.XX)</param>
    /// <param name="dropDecimal">will show price without any decimal places.</param>
    /// <param name="dropSymbol">will show price wihtout the symbol</param>
    /// <param name="notOfferedMessage">override the default message that is returned if the price is negative</param>
    /// <returns>Price text for display</returns>
    [Obsolete("Please use the PriceText method with options flags. Overriding default not offered message per method is no longer supported.")]
    string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol, string notOfferedMessage);

    /// <summary>
    /// Converts a price to the shoppers display currency, then formats it for display based on currency info.
    /// </summary>
    /// <param name="price">Price to format</param>
    /// <param name="maskPrices">true will output the price with X's instead of numbers ($X.XX)</param>
    /// <param name="negativeFormat">will output negative prices with the given format</param>
    /// <returns>Price text for display</returns>
    [Obsolete("Please use the PriceText method with options flags.")]
    string PriceText(ICurrencyPrice price, bool maskPrices, CurrencyNegativeFormat negativeFormat);

    /// <summary>
    /// Converts a price to the shoppers display currency, then formats it for display based on currency info.
    /// </summary>
    /// <param name="price">Price to format</param>
    /// <param name="maskPrices">true will output the price with X's instead of numbers ($X.XX)</param>
    /// <param name="dropDecimal">will show price without any decimal places.</param>
    /// <param name="dropSymbol">will show price wihtout the symbol</param>
    /// <param name="negativeFormat">will output negative prices with the given format</param>
    /// <returns>Price text for display</returns>
    [Obsolete("Please use the PriceText method with options flags.")]
    string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol, CurrencyNegativeFormat negativeFormat);

    /// <summary>
    /// Converts a price to the shoppers display currency, then formats it for display based on currency info.
    /// </summary>
    /// <param name="price">Price to format</param>
    /// <param name="textOptions"><see cref="PriceTextOptions"/></param>
    /// <param name="formatOptions"><see cref="PriceFormatOptions"/></param>
    /// <returns>Price text for display</returns>
    string PriceText(ICurrencyPrice price, PriceTextOptions textOptions = PriceTextOptions.None, PriceFormatOptions formatOptions = PriceFormatOptions.None);

    /// <summary>
    /// Converts a price to the shoppers display currency, then formats it for display based on currency info.
    /// </summary>
    /// <param name="price">Price to format</param>
    /// <param name="textOptions"><see cref="PriceTextOptions" /></param>
    /// <param name="formatOptions"><see cref="PriceFormatOptions" /></param>
    /// <param name="symbolFormatter">The <see cref="ISymbolFormatter"/> used for formatting the currency symbol.</param>
    /// <returns>Price text for display</returns>
    string PriceText(ICurrencyPrice price, ISymbolFormatter symbolFormatter, PriceTextOptions textOptions = PriceTextOptions.None, PriceFormatOptions formatOptions = PriceFormatOptions.None);

    /// <summary>
    /// Formats a price based on the currency info for display
    /// Negative prices will show with a minus sign
    /// </summary>
    /// <param name="price">Price to format</param>
    /// <param name="dropDecimal">will show price without any decimal places.</param>
    /// <param name="dropSymbol">will show price without the symbol</param>
    /// <returns>Price text for display</returns>
    [Obsolete("Please use the PriceFormat method with options flags.")]
    string PriceFormat(ICurrencyPrice price, bool dropDecimal, bool dropSymbol);

    /// <summary>
    /// Formats a price based on the currency info for display
    /// </summary>
    /// <param name="price">Price to format</param>
    /// <param name="dropDecimal">will show price without any decimal places.</param>
    /// <param name="dropSymbol">will show price without the symbol</param>
    /// <param name="negativeFormat">will output negative prices with the given format</param>
    /// <returns>Price text for display</returns>
    [Obsolete("Please use the PriceFormat method with options flags.")]
    string PriceFormat(ICurrencyPrice price, bool dropDecimal, bool dropSymbol, CurrencyNegativeFormat negativeFormat);

    /// <summary>
    /// Formats a price based on the currency info for display
    /// </summary>
    /// <param name="price">Price to format</param>
    /// <param name="priceOptions"><see cref="PriceFormatOptions"/></param>
    /// <returns>Price text for display</returns>
    string PriceFormat(ICurrencyPrice price, PriceFormatOptions priceOptions = PriceFormatOptions.None);

    /// <summary>
    /// Formats a price based on the currency info for display
    /// </summary>
    /// <param name="price">Price to format</param>
    /// <param name="priceOptions"><see cref="PriceFormatOptions"/></param>
    /// <param name="symbolFormatter">The <see cref="ISymbolFormatter"/> used for formatting the currency symbol.</param>
    /// <returns></returns>
    string PriceFormat(ICurrencyPrice price, PriceFormatOptions options, ISymbolFormatter symbolFormatter);

    /// <summary>
    /// Returns true if a product is marked as 'onsale'
    /// </summary>
    /// <param name="unifiedProductId">unified product id</param>
    /// <param name="shopperPriceType">shopper price type to use.  If not given the shoppers pricetype will be used</param>
    /// <param name="transactionCurrency">currencyType to use. If not given the shoppers transactional currencyType will be used.<see cref="ICurrencyInfo"/></param>
    /// <param name="isc">isc to use. If not given the site context ISC will be used.</param>
    /// <returns>true of the product is marked as 'onsale'</returns>
    bool IsProductOnSale(int unifiedProductId, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null);

    /// <summary>
    /// Returns true if a product is marked as 'onsale' for a specific region's PriceGroupId
    /// </summary>
    /// <param name="unifiedProductId">unified product id</param>
    /// <param name="priceGroupId">PriceGroupId for a region</param>
    /// <param name="shopperPriceType">shopper price type to use.  If not given the shoppers pricetype will be used</param>
    /// <param name="transactionCurrency">currencyType to use. If not given the shoppers transactional currencyType will be used.<see cref="ICurrencyInfo"/></param>
    /// <param name="isc">isc to use. If not given the site context ISC will be used.</param>
    /// <returns>true of the product is marked as 'onsale'</returns>
    bool IsRegionalProductOnSale(int unifiedProductId, int priceGroupId, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null);

    /// <summary>
    /// Returns the shoppers selected display currency info
    /// </summary>
    ICurrencyInfo SelectedDisplayCurrencyInfo
    {
      get;
    }

    /// <summary>
    /// Returns the shoppers selected display currency type.  This can also be set to update the shoppers display currency type.
    /// </summary>
    string SelectedDisplayCurrencyType
    {
      get;
      set;
    }

    /// <summary>
    /// Returns the shoppers transactional currency info based on their selected display currency info
    /// </summary>
    ICurrencyInfo SelectedTransactionalCurrencyInfo
    {
      get;
    }

    /// <summary>
    /// Returns the shoppers transactional currency type based on their selected display currency info.
    /// </summary>
    string SelectedTransactionalCurrencyType
    {
      get;
    }

    /// <summary>
    /// Because the IsTransactional flag is not the source of record for all contexts,
    /// this method will validate all business rules to indicate if a currencytype is transactional
    /// for the current context
    /// </summary>
    /// <param name="currencyToCheck">currency info to check</param>
    /// <returns>true if the currency is transactional for the current context</returns>
    bool IsCurrencyTransactionalForContext(ICurrencyInfo currencyToCheck);

    /// <summary>
    /// Returns the currency info for the given currency type.
    /// </summary>
    /// <param name="currencyType">currency type</param>
    /// <returns>currency info for the given type, or null if not found.</returns>
    ICurrencyInfo GetCurrencyInfo(string currencyType);

    /// <summary>
    /// Returns the currency info for the given currency type.
    /// Returns USD info if given type is not valid.
    /// </summary>
    /// <param name="currencyType">currencyType</param>
    /// <returns>currency info for the given type, or USD info if not found.</returns>
    ICurrencyInfo GetValidCurrencyInfo(string currencyType);

    /// <summary>
    /// Gets an enumerable list of currency info data
    /// </summary>
    IEnumerable<ICurrencyInfo> CurrencyInfoList
    {
      get;
    }

    /// <summary>
    /// Creates a new ICurrencyPrice object
    /// </summary>
    /// <param name="price">price</param>
    /// <param name="currencyInfo">currency info that the given price is.</param>
    /// <param name="currencyPriceType">whether the price is transactional or 'converted'</param>
    /// <returns>ICurrencyPrice object</returns>
    ICurrencyPrice NewCurrencyPrice(int price, ICurrencyInfo currencyInfo, CurrencyPriceType currencyPriceType);

    /// <summary>
    /// Creates an ICurrencyPrice based on a given USD price
    /// </summary>
    /// <param name="usdPrice">USD price to use</param>
    /// <param name="currencyInfo">optional ICurrencyInfo. If not given the output will be in the shoppers selected transactional currency</param>
    /// <param name="conversionRoundingType">rounding type to use during conversion</param>
    /// <returns>ICurrencyPrice based on the given usd price in the shoppers selected transactional currency or the given currency</returns>
    ICurrencyPrice NewCurrencyPriceFromUSD(int usdPrice, ICurrencyInfo currencyInfo = null, CurrencyConversionRoundingType conversionRoundingType = CurrencyConversionRoundingType.Round);

    /// <summary>
    /// Converts a price to the target price.
    /// When going from USD to another currency, will only convert if the priceToConvert is Transactional
    /// When going from non-USD, will only convert to USD, and only if the priceToConvert is Transactional
    /// non-USD to another non-USD is not currently supported.
    /// </summary>
    /// <param name="priceToConvert">price to convert</param>
    /// <param name="targetCurrencyInfo">target currency to convert to</param>
    /// <param name="conversionRoundingType">rounding type to use during conversion</param>
    /// <returns></returns>
    ICurrencyPrice ConvertPrice(ICurrencyPrice priceToConvert, ICurrencyInfo targetCurrencyInfo, CurrencyConversionRoundingType conversionRoundingType = CurrencyConversionRoundingType.Round);

  }
}
