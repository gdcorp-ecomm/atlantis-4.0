using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.UpdateItem.Interface;
using Atlantis.Framework.DataCache;
namespace Atlantis.Framework.UpdateItem.Interface
{
  public class UpdateItemRequestData : RequestData
  {
    List<UpdateItemParameter> _updateParameters = new List<UpdateItemParameter>();
    private int _privateLabelID = 0;
    string _customXML = string.Empty;
    public UpdateItemRequestData(string shopperID,
                            string sourceURL,
                            string orderID,
                            string pathway,
                            int pageCount,
                            int privateLabelId)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _privateLabelID = privateLabelId;
    }
    public UpdateItemRequestData(string shopperID,
                            string sourceURL,
                            string orderID,
                            string pathway,
                            int pageCount,
                            int newProductId,
                            int newQuantity,
                            int rowID,
                            int itemID,
                            int privateLabelId
                            )
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _privateLabelID = privateLabelId;
      List<UpdateItemParameter> parmList = new List<UpdateItemParameter>();
      parmList.Add(new UpdateItemParameter("pf_id", newProductId.ToString()));
      parmList.Add(new UpdateItemParameter("quantity", newQuantity.ToString()));
      RowID = rowID;
      ItemID = itemID;
      _updateParameters.AddRange(parmList);
    }
    public UpdateItemRequestData(string shopperID,
                            string sourceURL,
                            string orderID,
                            string pathway,
                            int pageCount,
                            int privateLabelId,
                           IEnumerable<UpdateItemParameter> itemKeys)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _privateLabelID = privateLabelId;
      _updateParameters.AddRange(itemKeys);
    }
    public string ItemXML()
    {
      //<itemChangeRequest><field name="pf_id" value="5602" /><field name="quantity" value="1" /></itemChangeRequest>
      //<field name="pf_id" value="5602" /><field name="quantity" value="1" />
      StringBuilder Serializer = new StringBuilder();
      Serializer.Append("<itemChangeRequest>");
      if (!string.IsNullOrEmpty(_customXML))
        Serializer.Append(_customXML);
      foreach (UpdateItemParameter currentParm in _updateParameters)
      {
        Serializer.Append("<field name=\"");
        Serializer.Append(currentParm.FieldName);
        Serializer.Append("\" value=\"");
        if (currentParm.FieldName == "pf_id")
        {
          int unifiedPfId = 0;
          int.TryParse(currentParm.FieldValue, out unifiedPfId);
          int nonUnifiedPFID = DataCache.DataCache.GetPFIDByUnifiedID(unifiedPfId, _privateLabelID);
          Serializer.Append(nonUnifiedPFID);
        }
        else
        {
          Serializer.Append(currentParm.FieldValue);
        }
        Serializer.Append("\" />");
      }
      Serializer.Append("</itemChangeRequest>");
      return Serializer.ToString();
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
    int _rowID = 0;
    public int RowID
    {
      get { return _rowID; }
      set { _rowID = value; }
    }
    int _itemID = 0;
    public int ItemID
    {
      get { return _itemID; }
      set { _itemID = value; }
    }
    public string CustomXML
    {
      get
      {
        return _customXML;
      }
      set
      {
        _customXML = value;
      }
    }
    public void AddItem(string fieldName, string fieldValue)
    {
      _updateParameters.Add(new UpdateItemParameter(fieldName, fieldValue));
    }

    public void AddItems(IEnumerable<UpdateItemParameter> itemKeys)
    {
      _updateParameters.AddRange(itemKeys);
    }
    public void AddItems(IEnumerable<UpdateItemParameter> itemKeys, string customXML)
    {
      _updateParameters.AddRange(itemKeys);
      _customXML = customXML;
    }
    public string ItemParameters
    {
      get
      {
        string result = string.Empty;
        List<string> itemsToDelete = _updateParameters.ConvertAll<string>(
          new Converter<UpdateItemParameter, string>(
            delegate(UpdateItemParameter item) { return item.ToString(); }
            ));
        result = string.Join("|", itemsToDelete.ToArray());
        return result;
      }
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("UpdateItem is not a cacheable request.");
    }

    #endregion

  }
}
