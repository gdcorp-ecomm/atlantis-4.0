using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.Providers.Interface.Products
{
  public interface IProduct
  {
    int Duration { get; }
    RecurringPaymentUnitType DurationUnit { get; }
    IProductInfo Info { get; }
    bool IsOnSale { get; }
    int Months { get; }
    int ProductId { get; }
    double Years { get; }
    ICurrencyPrice ListPrice { get; }
    ICurrencyPrice CurrentPrice { get; }
    ICurrencyPrice GetCurrentPriceByQuantity(int quantity);
  }
}
