using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDRequestAddIP.Interface
{
  public class HDVDRequestAddIpRequestData : RequestData 
  {
    public Guid AccountUid { get; set; }
    public TimeSpan RequestTimeout { get; set; }

    public HDVDRequestAddIpRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, Guid accountUid) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(10);
      AccountUid = accountUid;
    }

    
    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
