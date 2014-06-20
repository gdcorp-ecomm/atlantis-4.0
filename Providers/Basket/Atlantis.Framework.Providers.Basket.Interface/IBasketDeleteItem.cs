namespace Atlantis.Framework.Providers.Basket.Interface
{
  /// <summary>
  /// Interface for basket delete item
  /// </summary>
  public interface IBasketDeleteItem
  {
    /// <summary>
    /// Gets row id of the item to delete
    /// </summary>
    int RowId { get; }

    /// <summary>
    /// Gets item id of the item to delete
    /// </summary>
    int ItemId { get; }
  }
}
