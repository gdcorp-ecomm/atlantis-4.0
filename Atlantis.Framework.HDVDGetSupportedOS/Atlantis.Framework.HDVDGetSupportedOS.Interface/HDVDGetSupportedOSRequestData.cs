using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetSupportedOS.Interface
{
  public class HDVDGetSupportedOSRequestData : RequestData
  {
   
    public HDVDGetSupportedOSRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, Guid accountUid) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
    }

    public TimeSpan RequestTimeout { get; set; }

    public Guid AccountUid { get; private set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("HDVDGetSupportedOS is not a cacheable request.");
    }

    #endregion
  }
}
