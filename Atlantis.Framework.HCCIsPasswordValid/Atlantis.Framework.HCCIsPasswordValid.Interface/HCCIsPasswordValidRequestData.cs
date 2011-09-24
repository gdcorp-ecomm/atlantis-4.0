using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCIsPasswordValid.Interface
{
  public class HCCIsPasswordValidRequestData : RequestData
  {
    static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public HCCIsPasswordValidRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string password)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Password = password;
      RequestTimeout = _requestTimeout;
    }
    
    public string Password { get; set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in HCCIsPasswordValidRequestData");
    }


  }
}
