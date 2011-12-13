using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RemoveItemLog.Interface
{
  public class RemoveItemLogRequestData : RequestData
  {

    private int _rowId;
    private int _productId;
    bool _isGroup = false;
    private int _adjustedPrice;
    private int _discountedPrice;
    private string _itemTrackingCode = string.Empty;

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(2);

    public int AdjustedPrice
    {
      get { return _adjustedPrice; }
      set { _adjustedPrice = value; }
    }
    
    public int DiscountedPrice 
    {
      get { return _discountedPrice; }
      set { _discountedPrice = value; }
    }

    public string ItemTrackingCode
    {
      get { return _itemTrackingCode; }
      set { _itemTrackingCode = value; }
    }

    public int Quantity
    {
      get;
      set;
    }

    public int Duration
    {
      get;
      set;
    }

    public int RowId
    {
      get { return _rowId; }
    }

    public int ProductId
    {
      get { return _productId; }
    }

    public bool IsGroup
    {
      get { return _isGroup; }
    }

    public RemoveItemLogRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      int rowId, int productId, bool isGroup,string itemTrackingCode,int adjustedPrice,int discountedPrice,
      int quantity,int duration)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      AdjustedPrice = adjustedPrice;
      DiscountedPrice = discountedPrice;
      _rowId = rowId;
      _productId = productId;
      _isGroup = isGroup;
      Quantity = quantity;
      Duration = duration;
      ItemTrackingCode = itemTrackingCode;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("RemoveItemLog is not a cacheable request.");
    }
  }
}
