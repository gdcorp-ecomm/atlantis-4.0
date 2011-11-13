using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaRecentNamespaces.Interface
{
  public class MyaRecentNamespacesRequestData : RequestData
  {
    private static TimeSpan _DEFAULTIMEOUT = TimeSpan.FromSeconds(10.0);
    public DateTime FromDate { get; set; }

    public MyaRecentNamespacesRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, DateTime fromDate)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = _DEFAULTIMEOUT;
      FromDate = fromDate;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("MyaRecentNamespaces is not a cacheable request.");
    }
  }
}
