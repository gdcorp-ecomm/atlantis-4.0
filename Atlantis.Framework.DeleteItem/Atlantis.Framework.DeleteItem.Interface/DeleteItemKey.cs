
namespace Atlantis.Framework.DeleteItem.Interface
{
  public class DeleteItemKey
  {
    private int _rowId;
    private int _itemId;

    public int RowId
    {
      get { return _rowId; }
    }

    public int ItemId
    {
      get { return _itemId; }
    }

    public override string ToString()
    {
      return RowId.ToString() + "," + ItemId.ToString();
    }

    public DeleteItemKey(int rowId, int itemId)
    {
      _rowId = rowId;
      _itemId = itemId;
    }
  }
}
