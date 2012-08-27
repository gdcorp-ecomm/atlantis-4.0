using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobileAir2WebSms.Interface
{
  public class MobileAir2WebSmsRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(10);

    public string Recipient { get; set; }

    public string Body { get; set; }

    public string ReportingKey1 { get; set; }

    public string ReportingKey2 { get; set; }

    public MobileAir2WebSmsRequestData(string recipient, string body, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Body = body;
      Recipient = recipient;
      RequestTimeout = _requestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MobileAir2WebSms is not a cacheable request.");
    }
  }
}
