using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ReceiptUpsell.Interface
{
  public class ReceiptUpsellRequestData : RequestData
  {
    private int _rowId;
    private int _productId;
    private int _quantity;
    private int _adjustedPrice;
    private int _newProductId;
    private int _newQuantity;
    private int _newAdjustedPrice;
    private string _offerDescription;
    private int _privateLabelId;

    public int RowId
    {
      get { return _rowId; }
    }

    public int ProductId
    {
      get { return _productId; }
    }

    public int Quantity
    {
      get { return _quantity; }
    }

    public int AdjustedPrice
    {
      get { return _adjustedPrice; }
    }

    public int NewProductId
    {
      get { return _newProductId; }
    }

    public int NewQuantity
    {
      get { return _newQuantity; }
    }

    public int NewAdjustedPrice
    {
      get { return _newAdjustedPrice; }
    }

    public string OfferDescription
    {
      get { return _offerDescription; }
    }

    public int PrivateLabelId
    {
      get { return _privateLabelId; }
    }

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(3);

    public ReceiptUpsellRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      int rowId, int productId, int quantity, int adjustedPrice, 
      int newProductId, int newQuantity, int newAdjustedPrice, 
      string offerDescription, int privateLabelId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _rowId = rowId;
      _productId = productId;
      _quantity = quantity;
      _adjustedPrice = adjustedPrice;
      _newProductId = newProductId;
      _newQuantity = newQuantity;
      _newAdjustedPrice = newAdjustedPrice;
      _offerDescription = offerDescription;
      _privateLabelId = privateLabelId;
      RequestTimeout = _requestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("ReceiptUpsell is not a cacheable request.");
    }
  }
}
