using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCIsUsernameValid.Interface
{
  public class HCCIsUsernameValidRequestData : RequestData
  {
    static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public HCCIsUsernameValidRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  string username)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      UserName = username;
      RequestTimeout = _requestTimeout;
    }

    public string UserName { get; set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in HCCIsUsernameValidRequestData");     
    }


  }
}
