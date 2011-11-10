using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HXGetAccountInfo.Interface
{
  public class HXGetAccountInfoRequestData : RequestData
  {
    public string AccountUid { get; set; }

    public HXGetAccountInfoRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string accountUid)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      if (string.IsNullOrEmpty(accountUid)) { throw new ArgumentException("accountUid cannot be null or empty"); }
      AccountUid = accountUid;
      RequestTimeout = TimeSpan.FromSeconds(6.0);
    }

    public override string GetCacheMD5()
    {
      // It is not expected that this request will use Session or Data cache
      throw new NotImplementedException("HxGetAccountInfo is not a cacheable Request");
    }

    public override string ToXML()
    {
      return string.Format(@"<HXGetAccountInfoRequestData><ShopperId>{0}</ShopperId><AccountUid>{1}</AccountUid></HXGetAccountInfoRequestData>", ShopperID, AccountUid);
    }
  }
}
