using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WhoIsGetInfo.Interface
{
  public class WhoIsGetInfoRequestData : RequestData
  {
    private TimeSpan _requestTimeout = new TimeSpan(0, 0, 10);
    public TimeSpan RequestTimeout
    {
      get { return _requestTimeout; }
      set { _requestTimeout = value; }
    }
    public int PrivateLabelId { get; set; }
    public string DomainToLookup { get; set; }
    public string ClientIp { get; set; }
    public bool WasCaptchaEntered { get; set; }

    public WhoIsGetInfoRequestData(string shopperId,
                                        string sourceUrl,
                                        string orderId,
                                        string pathway,
                                        int pageCount,
                                        string domainName,
                                        int privateLabelId,
                                        string clientIp,
                                        bool wasCaptchaEntered)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      DomainToLookup = domainName;
      PrivateLabelId = privateLabelId;
      ClientIp = clientIp;
      WasCaptchaEntered = wasCaptchaEntered;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
