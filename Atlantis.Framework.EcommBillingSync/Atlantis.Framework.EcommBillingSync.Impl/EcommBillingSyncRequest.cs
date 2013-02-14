using System;
using System.Collections.Generic;
using Atlantis.Framework.EcommBillingSync.Impl.Helper_Classes;
using Atlantis.Framework.Interface;
using Atlantis.Framework.EcommBillingSync.Interface;

namespace Atlantis.Framework.EcommBillingSync.Impl
{
  public class EcommBillingSyncRequest : IRequest
  {
    #region Properties
    private CartUtility _cartUtil;
    private CartUtility CartUtil
    {
      get { return _cartUtil ?? (_cartUtil = new CartUtility()); }
    }

    private SyncUtility _syncUtil;
    private SyncUtility SyncUtil
    {
      get { return _syncUtil ?? (_syncUtil = new SyncUtility()); }
    }

    private DateTime? _newBillDate;
    private DateTime NewBillDate
    {
      get { return _newBillDate ?? (_newBillDate = SyncUtil.SetAnnualBillDate(BillingSyncProducts, SyncMonth, SyncDay)).Value; }
    }

    private List<BillingSyncProduct> BillingSyncProducts { get; set; }
    private int PrivateLabelId { get; set; }
    private bool SyncListIncludesAnnualProduct { get; set; }
    private int SyncMonth { get; set; }
    private int SyncDay { get; set; }
    private EcommBillingSyncRequestData EbsRequest { get; set; }
    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommBillingSyncResponseData responseData = null;

      try
      {
        var request = (EcommBillingSyncRequestData) requestData;

        SetRequestProperties(request);

        var cartResponse = PostBillingSyncToCart(request);
        if (cartResponse.Success)
        {
          responseData = new EcommBillingSyncResponseData();
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new EcommBillingSyncResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EcommBillingSyncResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Private Methods
    private void SetRequestProperties(EcommBillingSyncRequestData request)
    {
      EbsRequest = request;
      BillingSyncProducts = request.BillingSyncProducts;
      PrivateLabelId = request.PrivateLabelId;
      SyncMonth = request.SyncDate.Month;
      SyncDay = request.SyncDate.Day;
      SyncListIncludesAnnualProduct = BillingSyncProducts.Find(bsp => bsp.RecurringPaymentType.Equals("annual")) != null;
    }

    private CartResponse PostBillingSyncToCart(EcommBillingSyncRequestData ebsRequest)
    {
      var request = CartUtil.CreateAddItemRequest(ebsRequest);
      var i = 1;

      foreach (var bsp in ebsRequest.BillingSyncProducts)
      {
        var items = CreateItems(bsp, i++, ebsRequest.ItemTrackingCode);
        foreach (var item in items)
        {
          CartUtil.AddToCartRequest(request, item);  
        }        
      }

      return CartUtil.PostToCart(request, ebsRequest);
    }

    private IEnumerable<AddToCartItem> CreateItems(BillingSyncProduct bsp, int index, string itemTrackingCode)
    {
      const int QUANTITY = 1;
      List<AddToCartItem> addOnItems;
      string addOnGuidId;

      var items = new List<AddToCartItem>();
      var newBillDate = SyncListIncludesAnnualProduct ? NewBillDate : SyncUtil.GetNewExpirationDate(bsp.OriginalBillingDate, bsp.RecurringPaymentType, SyncMonth, SyncDay);
      var duration = SyncUtil.GetDuration(bsp.OriginalBillingDate, newBillDate, bsp.RecurringPaymentType);
      var durationHash = SyncUtil.GetDurationHash(PrivateLabelId, bsp.RenewalPfId, Convert.ToDouble(duration));
      var item = new AddToCartItem(string.Format("{0}_{1}", bsp.BillingResourceId, index), bsp.RenewalPfId, bsp.BillingResourceId, QUANTITY, itemTrackingCode, duration, durationHash);
      item[AddToCartItemProperty.TARGET_EXPIRATION_DATE] = newBillDate.ToString();

      if (SyncUtil.BillingSyncProductHasAddOns(EbsRequest, bsp.BillingResourceId, bsp.OrionId, bsp.Namespace, PrivateLabelId, index, duration, durationHash, out addOnItems, out addOnGuidId))
      {
        item[AddToCartItemProperty.GROUP_ID] = addOnGuidId;
        item[AddToCartItemProperty.PARENT_GROUP_ID] = addOnGuidId;
        items.Add(item);
        items.AddRange(addOnItems);
      }
      else
      {
        items.Add(item);
      }

      return items;
    }

    #endregion
  }
}
