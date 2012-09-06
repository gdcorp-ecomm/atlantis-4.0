using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetEmailPodDetails.Interface
{
  public class ECCGetEmailPodDetailsRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(8);

    public string EmailAddress { get; set; }

    public ECCGetEmailPodDetailsRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string emailAddresss) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      EmailAddress = emailAddresss;
      RequestTimeout = _requestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("ECCGetEmailPodDetails is not a cacheable request.");
    }
  }
}
