using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EasyDBGetUsage.Interface
{
  public class EasyDBGetUsageRequestData : RequestData
  {
    #region Properties

    /// <summary>
    /// Default of 5 seconds
    /// </summary>
    public int PrivateLabelId { get; private set; }
    public string AccountUid { get; private set; }

    #endregion

    public EasyDBGetUsageRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int privateLabelId
      , string accountUid)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      PrivateLabelId = privateLabelId;
      AccountUid = accountUid;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EasyDBGetUsageRequestData");
    }
  }
}
