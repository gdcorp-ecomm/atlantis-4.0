using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShortUrl.Interface
{
  public class ShortUrlRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(10);

    public string Url { get; set; }

    public ShortUrlRequestData(string url, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Url = url;
      RequestTimeout = _requestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("ShortUrl is not a cacheable request.");
    }
  }
}
