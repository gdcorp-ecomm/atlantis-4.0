using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.TargetedShopperService.Interface
{
  public class ShopperXidEncodeRequestData : RequestData
  {
    private const int DEFAULT_TIMEOUT = 5;
    public string DecodedXid { get; set; }

    public ShopperXidEncodeRequestData(string decodedXid)
    {
      DecodedXid = decodedXid;
      RequestTimeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(DecodedXid);
    }
  }
}
