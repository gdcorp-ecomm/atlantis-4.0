﻿using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductFreeCreditsByResource.Interface
{
  public class ProductFreeCreditsByResourceRequestData : RequestData
  {
    #region Properties
    
    public int BillingResourceId { get; set; }
    public int ProductTypeId { get; set; }

    #endregion Properties

    public ProductFreeCreditsByResourceRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int timeoutSeconds
      , int billingResourceId
      , int productTypeId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(timeoutSeconds);
      BillingResourceId = billingResourceId;
      ProductTypeId = productTypeId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in ProductFreeCreditsByResourceRequestData");
    }

  }
}
