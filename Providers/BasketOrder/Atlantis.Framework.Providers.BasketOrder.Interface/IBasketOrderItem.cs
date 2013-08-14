namespace Atlantis.Framework.Providers.BasketOrder.Interface
{
  public interface IBasketOrderItem
  {
    string SKU { get; }
    string ProductName { get; }
    string ProductCategory { get; }
    double UnitPrice { get; }
    string UnitPriceUsdFormatted { get; }
    int Quantity { get; }
    void AddItem(int quantity, double unitPriceUsd);
  }
}