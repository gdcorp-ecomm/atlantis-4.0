using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Basket.Interface
{
  /// <summary>
  /// Request collector used to build up all items to be deleted from the basket in a single call to the basket service
  /// </summary>
  public interface IBasketDeleteRequest
  {
    /// <summary>
    /// Gets items to delete
    /// </summary>
    IEnumerable<IBasketDeleteItem> ItemsToDelete { get; }

    /// <summary>
    /// Adds item to delete to the delete request
    /// </summary>
    void AddItemToDelete(int rowId, int itemId);
  }
}
