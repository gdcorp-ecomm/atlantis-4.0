using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.Providers.Interface.Products
{
  public interface IProduct
  {
    /// <summary>
    /// The unified product id
    /// </summary>
    int ProductId { get; }

    /// <summary>
    /// The number of periods from the product database. This represents the number of recurring
    /// periods this product is for.  Example: if it is 2 and the Duration Unit is months, then
    /// the database price for this product will be 'per 2 months'
    /// </summary>
    int Duration { get; }

    /// <summary>
    /// <see cref="RecurringPaymentUnitType"/>
    /// </summary>
    RecurringPaymentUnitType DurationUnit { get; }

    /// <summary>
    /// <see cref="IProductInfo"/>
    /// </summary>
    IProductInfo Info { get; }

    /// <summary>
    /// Number of months 
    /// </summary>
    int Months { get; }

    /// <summary>
    /// Number of Years
    /// </summary>
    double Years { get; }

    /// <summary>
    /// Number of Quarters
    /// </summary>
    double Quarters { get; }

    /// <summary>
    /// Number of Half Years
    /// </summary>
    double HalfYears { get; }

    /// <summary>
    /// true if the product is marked as 'on sale' in the catalog (regardless of what prices are)
    /// </summary>
    bool IsOnSale { get; }

    /// <summary>
    /// true if product is marked as 'on sale' in the catalog (regardless of what prices are)
    /// </summary>
    /// <param name="shopperPriceType">shopper price type to use. If not given, will use shoppers price type</param>
    /// <param name="transactionCurrency">currencyInfo to use, if null then shoppers selected transactionCurrency will be used.<see cref="ICurrencyInfo"/></param>
    /// <param name="isc">isc to use, if null then the sitecontext isc will be used</param>
    /// <returns>true if product is on sale</returns>
    bool GetIsOnSale(int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null);

    /// <summary>
    /// Gets the ListPrice for the product for the products native durationUnit and shoppers selected transaction currency
    /// </summary>
    ICurrencyPrice ListPrice { get; }
    
    /// <summary>
    /// Gets the ListPrice for the product
    /// </summary>
    /// <param name="durationUnit">durationUnit of returned price<see cref="RecurringPaymentUnitType"/></param>
    /// <param name="shopperPriceType">shopper price type to use. If not given, will use shoppers price type</param>
    /// <param name="transactionCurrency">currencyInfo to use, if null then shoppers selected transactionCurrency will be used.<see cref="ICurrencyInfo"/></param>
    /// <param name="roundingType">avoid using this when possible.<see cref="PriceRoundingType"/></param>
    /// <returns>ListPrice for product</returns>
    ICurrencyPrice GetListPrice(RecurringPaymentUnitType durationUnit, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, PriceRoundingType roundingType = PriceRoundingType.RoundFractionsUpProperly);

    /// <summary>
    /// Gets the CurrentPrice for the product for the products native durationUnit and shoppers selected transaction currency
    /// </summary>
    ICurrencyPrice CurrentPrice { get; }

    /// <summary>
    /// Gets the CurrentPrice for the product
    /// </summary>
    /// <param name="durationUnit">durationUnit of returned price<see cref="RecurringPaymentUnitType"/></param>
    /// <param name="shopperPriceType">shopper price type to use. If not given, will use shoppers price type</param>
    /// <param name="transactionCurrency">currencyInfo to use, if null then shoppers selected transactionCurrency will be used.<see cref="ICurrencyInfo"/></param>
    /// <param name="isc">isc to use, if null then the sitecontext isc will be used</param>
    /// <param name="roundingType">avoid using this when possible.<see cref="PriceRoundingType"/></param>
    /// <returns>CurrentPrice for product</returns>
    ICurrencyPrice GetCurrentPrice(RecurringPaymentUnitType durationUnit, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, string isc = null, PriceRoundingType roundingType = PriceRoundingType.RoundFractionsUpProperly);

    /// <summary>
    /// Gets the CurrentPrice for the product based on given quantity for the products native durationUnit and shoppers selected transaction currency
    /// Note that the price returned is NOT quantity * price, it is still the price for 1 durationUnit
    /// There are known issues with this method actually giving back proper quanity based sale pricing
    /// </summary>
    /// <returns>CurrentPrice for product</returns>
    ICurrencyPrice GetCurrentPriceByQuantity(int quantity);

    /// <summary>
    /// Gets the CurrentPrice for the product based on given quantity
    /// Note that the price returned is NOT quantity * price, it is still the price for 1 durationUnit
    /// There are known issues with this method actually giving back proper quanity based sale pricing
    /// </summary>
    /// <param name="quantity">quantity to use for any quantity discounts (not reliable)</param>
    /// <param name="durationUnit">durationUnit of returned price<see cref="RecurringPaymentUnitType"/></param>
    /// <param name="shopperPriceType">shopper price type to use. If not given, will use shoppers price type</param>
    /// <param name="transactionCurrency">currencyInfo to use, if null then shoppers selected transactionCurrency will be used.<see cref="ICurrencyInfo"/></param>
    /// <param name="roundingType">avoid using this when possible.<see cref="PriceRoundingType"/></param>
    /// <returns>CurrentPrice for product</returns>
    ICurrencyPrice GetCurrentPriceByQuantity(int quantity, RecurringPaymentUnitType durationUnit, int shopperPriceType = -1, ICurrencyInfo transactionCurrency = null, PriceRoundingType roundingType = PriceRoundingType.RoundFractionsUpProperly);

  }
}
