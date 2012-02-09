using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDUpdBandwidthOverProt.Interface
{
  public class HDVDUpdBandwidthOverProtRequestData : RequestData
  {
    public HDVDUpdBandwidthOverProtRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, bool suspend, bool isEnabled, string accountUid) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      Suspend = suspend;
      IsEnabled = isEnabled;
      AccountUid = accountUid;
    }

    public string AccountUid { get; set; }

    public bool IsEnabled { get; set; }

    public bool Suspend { get; set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in HDVDUpdBandwidthOverProtRequestData");     
    }


  }
}
