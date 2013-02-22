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

    private int PrivateLabelId { get; set; }
    private int SyncMonth { get; set; }
    private int SyncDay { get; set; }
    private EcommBillingSyncRequestData EbsRequest { get; set; }
    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommBillingSyncResponseData responseData = null;

      try
      {
        var request = (EcommBillingSyncRequestData)requestData;
        SetRequestProperties(request);

        if (!EbsRequest.RequestContainsInvalidSyncDate && EbsRequest.BillingSyncProducts.Count > 0)
        {
          var cartResponse = PostBillingSyncToCart();
          if (cartResponse.Success)
          {
            responseData = new EcommBillingSyncResponseData(EbsRequest.BillingSyncErrors);
          }
        }
        else
        {
          var aex = new AtlantisException(requestData
        , "EcommBillingSyncRequest::RequestHandler"
        , "Request contained no valid products, or an invalid sync date"
        , EbsRequest.ToXML());
          responseData = new EcommBillingSyncResponseData(aex, EbsRequest.BillingSyncErrors);
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new EcommBillingSyncResponseData(exAtlantis, EbsRequest.BillingSyncErrors);
      }

      catch (Exception ex)
      {
        responseData = new EcommBillingSyncResponseData(requestData, ex, EbsRequest.BillingSyncErrors);
      }

      return responseData;
    }

    #region Private Methods
    private void SetRequestProperties(EcommBillingSyncRequestData request)
    {
      EbsRequest = request;
      PrivateLabelId = request.PrivateLabelId;
      SyncMonth = request.SyncDate.Month;
      SyncDay = request.SyncDate.Day;      
    }

    private CartResponse PostBillingSyncToCart()
    {
      var request = CartUtil.CreateAddItemRequest(EbsRequest);
      var i = 1;

      foreach (var bsp in EbsRequest.BillingSyncProducts)
      {
        var items = CreateItems(bsp, i++, EbsRequest.ItemTrackingCode);
        foreach (var item in items)
        {
          CartUtil.AddToCartRequest(request, item);  
        }        
      }

      return CartUtil.PostToCart(request, EbsRequest);
    }

    private IEnumerable<AddToCartItem> CreateItems(BillingSyncProduct bsp, int index, string itemTrackingCode)
    {
      const int QUANTITY = 1;
      List<AddToCartItem> addOnItems;
      string addOnGuidId;
      string customXml;

      var items = new List<AddToCartItem>();      
      var newBillDate = SyncUtil.GetNewExpirationDate(bsp.OriginalBillingDate, bsp.RecurringPaymentType, SyncMonth, SyncDay);
      var duration = SyncUtil.GetDuration(bsp.OriginalBillingDate, newBillDate, bsp.RecurringPaymentType);
      var renewalProductId = bsp.RenewalPeriods.Equals(1) ? bsp.RenewalProductId : SyncUtil.GetRenewalProductId(bsp.RenewalProductId, bsp.RecurringPaymentType, PrivateLabelId);
      var durationHash = SyncUtil.GetDurationHash(EbsRequest, renewalProductId, Convert.ToDouble(duration), bsp);
      var item = new AddToCartItem(string.Format("{0}_{1}", bsp.BillingResourceId, index), renewalProductId, bsp.BillingResourceId, QUANTITY, itemTrackingCode, duration, durationHash);
      item[AddToCartItemProperty.TARGET_EXPIRATION_DATE] = newBillDate.ToString();

      if (SyncUtil.BillingSyncProductHasAddOns(EbsRequest, bsp, PrivateLabelId, index, duration, out addOnItems, out addOnGuidId))
      {
        item[AddToCartItemProperty.GROUP_ID] = addOnGuidId;
        item[AddToCartItemProperty.PARENT_GROUP_ID] = addOnGuidId;
        items.Add(item);
        items.AddRange(addOnItems);
      }
      else if (!string.IsNullOrEmpty(customXml = SyncUtil.BuildCustomXmlIfNecessary(bsp.Namespace, bsp.ProductNameInfo, bsp.BillingResourceId, duration)))
      {
        item.CustomXml = customXml;
        items.Add(item);
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
