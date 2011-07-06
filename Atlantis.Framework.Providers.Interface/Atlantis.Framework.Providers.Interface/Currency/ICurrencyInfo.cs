namespace Atlantis.Framework.Providers.Interface.Currency
{
  public interface ICurrencyInfo
  {
    string CurrencyType { get; }
    int DecimalPrecision { get; }
    string DecimalSeparator { get; }
    string Description { get; }
    string DescriptionPlural { get; }
    double ExchangeRate { get; }
    double ExchangeRatePricing { get; }
    double ExchangeRateOperating { get; }
    string Symbol { get; }
    string SymbolHtml { get; }
    CurrencySymbolPositionType SymbolPosition { get; }
    string ThousandsSeparator { get; }
    bool IsTransactional { get; }
  }
}
