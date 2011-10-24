using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DeleteItem.Interface
{
  public class DeleteItemRequestData : RequestData
  {
    List<DeleteItemKey> _itemsToDelete = new List<DeleteItemKey>();

    // **************************************************************** //

    public DeleteItemRequestData(string sShopperID,
                                 string sSourceURL,
                                 string sOrderID,
                                 string sPathway,
                                 int iPageCount)
                                 : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
    }

    // **************************************************************** //

    public DeleteItemRequestData(string sShopperID,
                             string sSourceURL,
                             string sOrderID,
                             string sPathway,
                             int iPageCount,
                             IEnumerable<DeleteItemKey> itemKeys)
                            : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      _itemsToDelete.AddRange(itemKeys);
    }

    string _basketType = "gdshop";
    public string BasketType
    {
      get { return _basketType; }
      set { _basketType = value; }
    }

    bool _isManager = false;
    public bool IsManager
    {
      get { return _isManager; }
      set { _isManager = value; }
    }

    public void AddItem(int rowId, int itemId)
    {
      _itemsToDelete.Add(new DeleteItemKey(rowId, itemId));
    }

    public void AddItems(IEnumerable<DeleteItemKey> itemKeys)
    {
      _itemsToDelete.AddRange(itemKeys);
    }

    public string ItemKeysToDelete
    {
      get
      {
        string result = string.Empty;
        List<string> itemsToDelete = _itemsToDelete.ConvertAll<string>(
          new Converter<DeleteItemKey, string>(
            delegate(DeleteItemKey item) { return item.ToString(); }
            ));
        result = string.Join("|", itemsToDelete.ToArray());
        return result;
      }
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("DeleteItem is not a cacheable request.");
    }

    #endregion

  }
}
