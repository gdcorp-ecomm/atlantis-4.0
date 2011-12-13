using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ModifyItem.Interface
{
  public class ModifyItemRequestData : RequestData
  {
    // **************************************************************** //

    string m_sIndex=string.Empty;
    string m_sQuantity=string.Empty;
    List<ModifyItemKey> _modifyItemKey = new List<ModifyItemKey>();

    // **************************************************************** //

    public ModifyItemRequestData(string sShopperID,
                                  string sSourceURL,
                                  string sOrderID,
                                  string sPathway,
                                  int iPageCount)
                                  : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = new TimeSpan(0, 0, 10);
    }

    // **************************************************************** //

    public ModifyItemRequestData(string sShopperID,
                              string sSourceURL,
                              string sOrderID,
                              string sPathway,
                              int iPageCount,
                              string sIndex,
                              string sQuantity)
                              : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      m_sIndex = sIndex;
      m_sQuantity = sQuantity;
      RequestTimeout = new TimeSpan(0, 0, 10);
    }
    public ModifyItemRequestData(string sShopperID,
                           string sSourceURL,
                           string sOrderID,
                           string sPathway,
                           int iPageCount,
                           string basketType,
                           int rowID,
                           int itemID,
                           int newQuantity)
        : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
        _basketType = basketType;
        AddItem(rowID, itemID, newQuantity);
        RequestTimeout = new TimeSpan(0, 0, 10);
    }
    string _basketType = string.Empty;
    public string BasketType
    {
      get { return _basketType; }
      set { _basketType = value; }
    }

    public string Index
    {
      get { return m_sIndex; }
      set { m_sIndex = value; }
    }

    public string Quantity
    {
      get { return m_sQuantity; }
      set { m_sQuantity = value; }
    }
    public int ItemCount
    {
        get { return _modifyItemKey.Count; }
    }
    public void AddItem(int rowID,int itemID,int newQuantity)
    {
        _modifyItemKey.Add(new ModifyItemKey(rowID,itemID,newQuantity));
    }

    public void AddItems(IEnumerable<ModifyItemKey> itemKeys)
    {
        _modifyItemKey.AddRange(itemKeys);
    }
    public void ClearItems()
    {
        _modifyItemKey.Clear();
    }
    public string ItemXML()
    {
        StringBuilder Serializer = new StringBuilder();
        Serializer.Append("<dictionary>");
        foreach (ModifyItemKey currentParm in _modifyItemKey)
        {
            Serializer.Append("<item name='");
            Serializer.Append(currentParm.RowId);
            Serializer.Append("' item_id='");
            Serializer.Append(currentParm.ItemId);
            Serializer.Append("' >");
            Serializer.Append(currentParm.NewQuantity);
            Serializer.Append("</item>");
        }
        Serializer.Append("</dictionary>");
        return Serializer.ToString();
    }
    public string ItemParameters
    {
        get
        {
            string result = string.Empty;
            List<string> itemsToDelete = _modifyItemKey.ConvertAll<string>(
              new Converter<ModifyItemKey, string>(
                delegate(ModifyItemKey item) { return item.ToString(); }
                ));
            result = string.Join("|", itemsToDelete.ToArray());
            return result;
        }
    }
    // **************************************************************** //

    #region RequestData Members

    // **************************************************************** //

    public override string GetCacheMD5()
    {
      throw new Exception("ModifyItem is not a cacheable request.");
    }

    // **************************************************************** //

    #endregion

    // **************************************************************** //
  }
}
