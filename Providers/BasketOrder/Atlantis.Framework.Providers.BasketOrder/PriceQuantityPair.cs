namespace Atlantis.Framework.Providers.BasketOrder
{
  class PriceQuantityPair
  {
    public PriceQuantityPair(int quantity, double price)
    {
      Quantity = quantity;
      Price = price;
    }

    public int Quantity { get; private set; }
    public double Price { get; private set; }
  }
}
