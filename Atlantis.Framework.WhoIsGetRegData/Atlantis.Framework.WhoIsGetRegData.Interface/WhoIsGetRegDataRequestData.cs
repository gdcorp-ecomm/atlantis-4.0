using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.WhoIsGetRegData.Interface
{
  public class WhoIsGetRegDataRequestData : RequestData
  {
    private TimeSpan _requestTimeout = new TimeSpan(0, 0, 10);

    public new TimeSpan RequestTimeout
    {
      get
      {
        return this._requestTimeout;
      }
      set
      {
        this._requestTimeout = value;
      }
    }

    public string DomainToLookup { get; set; }
    public string ClientIp { get; set; }
    public bool WasCaptchaEntered { get; set; }

    public WhoIsGetRegDataRequestData(string shopperId, 
      string sourceUrl, 
      string orderId, 
      string pathway, 
      int pageCount, 
      string domainName,
      string clientIp,
      bool wasCaptchaEntered)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      DomainToLookup = domainName;
      ClientIp = clientIp;
      WasCaptchaEntered = wasCaptchaEntered;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
