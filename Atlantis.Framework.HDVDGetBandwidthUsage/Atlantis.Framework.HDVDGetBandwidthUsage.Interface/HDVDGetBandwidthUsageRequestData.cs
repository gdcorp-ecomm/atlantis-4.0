using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetBandwidthUsage.Interface
{
  public class HDVDGetBandwidthUsageRequestData : RequestData
  {
    public HDVDGetBandwidthUsageRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string accountUid) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
    }

    public string AccountUid { get; set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in HDVDGetBandwidthUsageRequestData");     
    }


  }
}
