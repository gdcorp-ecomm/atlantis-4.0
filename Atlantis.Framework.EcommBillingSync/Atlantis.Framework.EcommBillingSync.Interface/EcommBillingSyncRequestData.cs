﻿using System;
using System.Collections.Generic;
using System.IO;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommBillingSync.Interface
{
  public class EcommBillingSyncRequestData : RequestData
  {
    #region Private Constants
    private const int ADD_ITEM_REQUEST_TYPE = 4;
    #endregion 

    #region Properties
    public int AddItemRequestType { get; private set; }
    public List<BillingSyncProduct> BillingSyncProducts { get; private set; }
    public string ClientIp { get; private set; }
    public string ItemTrackingCode { get; private set; }
    public int PrivateLabelId { get; private set; }
    public string SelectedTransactionalCurrencyType { get; private set; }
    public DateTime SyncDate { get; private set; }
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
      , int addItemRequestType = ADD_ITEM_REQUEST_TYPE)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      CheckForInvalidInput(billingSyncProducts, syncDate);

      AddItemRequestType = addItemRequestType;
      BillingSyncProducts = billingSyncProducts;
      ClientIp = clientIp;
      ItemTrackingCode = itemTrackingCode;
      PrivateLabelId = privateLabelId;
      RequestTimeout = TimeSpan.FromSeconds(30);
      SelectedTransactionalCurrencyType = selectedTransactionalCurrencyType;
      SyncDate = syncDate;
    }

    #region Invalid Request
    private static void CheckForInvalidInput(List<BillingSyncProduct> billingSyncProducts, DateTime syncDate)
    {
      if (syncDate.Day < 1 || syncDate.Day > 28)
      {
        throw new InvalidDataException("Synchronization date must be between 1 & 28 inclusive");
      }

      var invalidBsp = billingSyncProducts.Find(bsp => !bsp.AllowRenewals);
      if (invalidBsp != null)
      {
        throw new InvalidDataException(string.Format("Sychronization list includes product ineligible for renewal - (ResourceId: {0} | OrionId: {1})", invalidBsp.BillingResourceId, invalidBsp.OrionId));
      }
    }
    #endregion

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EcommBillingSyncRequestData");     
    }
  }
}
