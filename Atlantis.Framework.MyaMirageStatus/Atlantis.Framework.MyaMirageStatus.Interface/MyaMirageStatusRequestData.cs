using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaMirageStatus.Interface
{
  public class MyaMirageStatusRequestData : RequestData
  {
    private static TimeSpan _DEFAULTIMEOUT = TimeSpan.FromSeconds(3.0);

    public MyaMirageStatusRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = _DEFAULTIMEOUT;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("MyaMirageStatus is not a cacheable request.");
    }
  }
}
