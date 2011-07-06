using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.Providers.Interface.Products
{
  public interface IProductView
  {
    void CalculateSavings(IProduct baseProduct);
    void CalculateSavingsBasedOnCurrentPriceOnly(IProduct baseProduct);
    void CalculateSavingsWithAddtionalSavingsPercent(IProduct baseProduct, int additionalSavingsPercent);
    void CalculateSavingsWithFixedDiscountPrice(IProduct baseProduct, ICurrencyPrice fixedDiscountPrice);
    void CalculateSavingsWithFixedSalePrice(IProduct baseProduct, ICurrencyPrice fixedSalePrice);
    void CalculateSavingsWithMinimumSavingsPercent(IProduct baseProduct, int minimumSavingsPercent);
    bool IsDefault { get; set; }
    bool IsDiscounted { get; }
    string LabelText { get; set; }
    PriceRoundingType PriceRoundingMethod { get; set; }
    IProduct Product { get; }
    int Quantity { get; }
    int SavingsPercentage { get; }
    SavingsRoundingType SavingsRounding { get; set; }

    ICurrencyPrice MonthlyListPrice { get; }
    ICurrencyPrice MonthlyCurrentPrice { get; }
    ICurrencyPrice YearlyListPrice { get; }
    ICurrencyPrice YearlyCurrentPrice { get; }
    ICurrencyPrice TotalPrice { get; }
    ICurrencyPrice UnitPrice { get; }
  }
}
