﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperSegment.Interface
{
  public class ShopperSegmentRequestData : RequestData
  {

    public ShopperSegmentRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    { }

    
    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in ShopperSegmentRequestData");
    }


  }
}
