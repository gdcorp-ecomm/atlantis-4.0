using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetEmailUsage.Interface
{
  public class HDVDGetEmailUsageRequestData : RequestData
  {
    private readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(15);

    public string AccountGuid { get; set; }

    public HDVDGetEmailUsageRequestData(
      string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountGuid) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountGuid = accountGuid;
      RequestTimeout = _requestTimeout;
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("HDVDGetEmailUsageRequest is not a cacheable request");
    }

    #endregion
  }
}
