using Atlantis.Framework.Providers.Basket.Interface;

namespace Atlantis.Framework.Providers.Basket
{
  public class BasketDeleteItem : IBasketDeleteItem
  {
    public int RowId { get; private set; }

    public int ItemId { get; private set; }

    internal BasketDeleteItem(int rowId, int itemId)
    {
      RowId = rowId;
      ItemId = itemId;
    }
  }
}
