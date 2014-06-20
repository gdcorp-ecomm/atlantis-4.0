using System.Collections.Generic;
using Atlantis.Framework.Providers.Basket.Interface;

namespace Atlantis.Framework.Providers.Basket
{
  public class BasketDeleteRequest : IBasketDeleteRequest
  {
    private readonly IList<IBasketDeleteItem> _itemsToDelete;

    public IEnumerable<IBasketDeleteItem> ItemsToDelete 
    {
      get { return _itemsToDelete; }
    }

    public void AddItemToDelete(int rowId, int itemId)
    {
      _itemsToDelete.Add(new BasketDeleteItem(rowId, itemId));
    }

    internal BasketDeleteRequest()
    {
      _itemsToDelete = new List<IBasketDeleteItem>();
    }
  }
}
