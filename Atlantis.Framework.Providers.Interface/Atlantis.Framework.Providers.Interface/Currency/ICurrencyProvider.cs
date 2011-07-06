
namespace Atlantis.Framework.Providers.Interface.Currency
{
  public interface ICurrencyProvider
  {
    ICurrencyPrice GetCurrentPrice(int unifiedProductId);
    ICurrencyPrice GetCurrentPrice(int unifiedProductId, int shopperPriceType);
    ICurrencyPrice GetCurrentPriceByQuantity(int unifiedProductId, int quantity);
    ICurrencyPrice GetCurrentPriceByQuantity(int unifiedProductId, int quantity, int shopperPriceType);
    ICurrencyPrice GetListPrice(int unifiedProductId);
    ICurrencyPrice GetListPrice(int unifiedProductId, int shopperPriceType);
    string PriceText(ICurrencyPrice price, bool maskPrices);
    string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal);
    string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol);
    string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol, string notOfferedMessage);
    string PriceText(ICurrencyPrice price, bool maskPrices, CurrencyNegativeFormat negativeFormat);
    string PriceText(ICurrencyPrice price, bool maskPrices, bool dropDecimal, bool dropSymbol, CurrencyNegativeFormat negativeFormat);
    ICurrencyInfo SelectedDisplayCurrencyInfo { get; }
    string SelectedDisplayCurrencyType { get; set; }
    ICurrencyInfo SelectedTransactionalCurrencyInfo { get; }
    string SelectedTransactionalCurrencyType { get; }
    string PriceFormat(ICurrencyPrice price, bool dropDecimal, bool dropSymbol);
    string PriceFormat(ICurrencyPrice price, bool dropDecimal, bool dropSymbol, CurrencyNegativeFormat negativeFormat);
    bool IsProductOnSale(int unifiedProductId);
    bool IsCurrencyTransactionalForContext(ICurrencyInfo currencyToCheck);
  }
}
