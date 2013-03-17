using System;
using System.Collections.Generic;
using System.IO;
using Atlantis.Framework.Interface;
using BillingSyncErrorType = Atlantis.Framework.EcommBillingSync.Interface.BillingSyncErrorData.BillingSyncErrorType;

namespace Atlantis.Framework.EcommBillingSync.Interface
{
  public class EcommBillingSyncRequestData : RequestData
  {
    #region Private Constants
    private const int ADD_ITEM_REQUEST_TYPE = 4;
    private const int ECOMM_PRODUCT_ADDONS_TYPE = 652;
    #endregion 

    #region Properties
    public int AddItemRequestType { get; private set; }
    public List<BillingSyncProduct> BillingSyncProducts { get; private set; }
    public string ClientIp { get; private set; }
    public int EcommProductAddOnsRequestType { get; private set; }
    public string ItemTrackingCode { get; private set; }
    public int PrivateLabelId { get; private set; }
    public string SelectedTransactionalCurrencyType { get; private set; }
    public DateTime SyncDate { get; private set; }
    public List<BillingSyncErrorData> BillingSyncErrors { get; private set; }
    public bool RequestContainsInvalidSyncDate { get; private set; }
    #endregion

    public EcommBillingSyncRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , List<BillingSyncProduct> billingSyncProducts
      , DateTime syncDate
      , string itemTrackingCode
      , string selectedTransactionalCurrencyType
      , string clientIp
      , int privateLabelId
      , int addItemRequestType = ADD_ITEM_REQUEST_TYPE
      , int ecommProductAddOnsRequestType = ECOMM_PRODUCT_ADDONS_TYPE)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      CheckForInvalidInput(billingSyncProducts, syncDate);

      AddItemRequestType = addItemRequestType;
      BillingSyncProducts = billingSyncProducts;
      ClientIp = clientIp;
      EcommProductAddOnsRequestType = ecommProductAddOnsRequestType;
      ItemTrackingCode = itemTrackingCode;
      PrivateLabelId = privateLabelId;
      RequestTimeout = TimeSpan.FromSeconds(120);
      SelectedTransactionalCurrencyType = selectedTransactionalCurrencyType;
      SyncDate = syncDate;
    }

    #region Invalid Request
    private void CheckForInvalidInput(List<BillingSyncProduct> billingSyncProducts, DateTime syncDate)
    {
      BillingSyncErrors = new List<BillingSyncErrorData>();
      RequestContainsInvalidSyncDate = false;
      InvalidDataException ide;

      var billingSyncProductsCopy = new List<BillingSyncProduct>(billingSyncProducts);

      if (syncDate.Day < 1 || syncDate.Day > 28)
      {
        ide = new InvalidDataException("Synchronization date must be between 1 & 28 inclusive");
        BillingSyncErrors.Add(new BillingSyncErrorData(BillingSyncErrorType.InvalidSyncDate, ide));
        RequestContainsInvalidSyncDate = true;
        return;
      }

      var invalidBspsByFlag = billingSyncProductsCopy.FindAll(bsp => !bsp.AllowRenewals);
      foreach (var bsp in invalidBspsByFlag)
      {
        billingSyncProducts.Remove(bsp);
        ide = new InvalidDataException(string.Format("BillingSyncProduct is ineligible for renewal"));
        BillingSyncErrors.Add(new BillingSyncErrorData(BillingSyncErrorType.RenewalNotAllowedByFlag, ide, bsp));
      }

      var invalidBspsByRecurringType = billingSyncProductsCopy.FindAll(bsp => String.Compare(bsp.RecurringPaymentType, "onetime", StringComparison.OrdinalIgnoreCase) == 0);
      foreach (var bsp in invalidBspsByRecurringType)
      {
        billingSyncProducts.Remove(bsp);
        ide = new InvalidDataException(string.Format("BillingSyncProduct has ineligible 'onetime' recurringType"));
        BillingSyncErrors.Add(new BillingSyncErrorData(BillingSyncErrorType.RenewalNotAllowedByRecurringType, ide, bsp));
      }
    }
    #endregion

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EcommBillingSyncRequestData");     
    }
  }
}
