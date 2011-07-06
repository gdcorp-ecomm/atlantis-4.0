
namespace Atlantis.Framework.Providers.Interface.Currency
{
  public interface ICurrencyPrice
  {
    int Price { get; }
    ICurrencyInfo CurrencyInfo { get; }
    CurrencyPriceType Type { get; }
  }
}
