using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.TargetedShopperService.Interface
{
  public class ShopperXidDecodeRequestData : RequestData
  {
    private const int DEFAULT_TIMEOUT = 5;
    public string EncodedXid { get; set; }

    public ShopperXidDecodeRequestData(string encodedXid)
    {
      EncodedXid = encodedXid;
      RequestTimeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(EncodedXid);
    }
  }
}
