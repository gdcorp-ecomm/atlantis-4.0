using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballLogImpressions.Interface
{
  public class FastballLogImpressionsRequestData : RequestData
  {
    public List<FastballTrafficEvent> FastballTrafficEvents { get; set; }

    public string MobileSessionGuid { get; set; }

    public string DeviceGuid { get; set; }

    public FastballLogImpressionsRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      List<FastballTrafficEvent>fastballTrafficEvents, string mobileSessionGuid, string deviceGuid)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ShopperID = shopperId;
      RequestTimeout = TimeSpan.FromSeconds(6);
      FastballTrafficEvents = fastballTrafficEvents;
      MobileSessionGuid = mobileSessionGuid;
      DeviceGuid = deviceGuid;
    }   

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
