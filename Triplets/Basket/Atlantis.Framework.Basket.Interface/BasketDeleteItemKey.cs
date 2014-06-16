using System.Globalization;

namespace Atlantis.Framework.Basket.Interface
{
  /// <summary>
  /// The BasketDeleteItemKey class represents an item to be deleted.
  /// </summary>
  public class BasketDeleteItemKey
  {
    /// <summary>
    /// Gets row id.
    /// </summary>
    public int RowId { get; private set; }

    /// <summary>
    /// Gets item id.
    /// </summary>
    public int ItemId { get; private set; }

    /// <summary>
    /// Initializes a new instance of the BasketDeleteItemKey class using
    /// row id and item id specified.
    /// </summary>
    /// <param name="rowId">Row id to be deleted.</param>
    /// <param name="itemId">Item id to be deleted.</param>
    public BasketDeleteItemKey(int rowId, int itemId)
    {
      RowId = rowId;
      ItemId = itemId;
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
      return string.Format("{0},{1}", RowId.ToString(CultureInfo.InvariantCulture),
        ItemId.ToString(CultureInfo.InvariantCulture));
    }
  }
}
