using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GeoByIP.Interface
{
  public class GeoByIPRequestData : RequestData
  {
    public string IpAddress { get; private set; }
    public LookupTypeEnum LookupType { get; set; }

    public GeoByIPRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string ipAddress, LookupTypeEnum lookupType)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      IpAddress = ipAddress;
      LookupType = lookupType;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GeoByIPRequestData is not a cacheable request.");
    }
  }
}
