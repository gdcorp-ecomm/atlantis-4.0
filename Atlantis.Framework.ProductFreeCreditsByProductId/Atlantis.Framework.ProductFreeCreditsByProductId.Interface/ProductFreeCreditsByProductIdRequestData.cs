﻿using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductFreeCreditsByProductId.Interface
{
  public class ProductFreeCreditsByProductIdRequestData : RequestData
  {
    #region Properties
    
    public int UnifiedProductId { get; set; }
    public int PrivateLabelId { get; set; }

    #endregion Properties

    public ProductFreeCreditsByProductIdRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int timeoutSeconds
      , int unifiedProductId
      , int privateLabelId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(timeoutSeconds);
      UnifiedProductId = unifiedProductId;
      PrivateLabelId = privateLabelId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in ProductFreeCreditsByProductIdRequestData");
    }

  }
}
