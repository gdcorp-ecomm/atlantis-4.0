using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrionGetUsage.Interface
{
  public class OrionGetUsageRequestData : RequestData
  {
    public const string BANDWIDTH_USAGETYPE = "BANDWIDTH";
    public const string DISKSPACE_USAGETYPE = "DISK_SPACE";
    public const string MINUTES_USAGETYPE = "minutes";

    public string OrionResourceId { get; set; }
    public string UsageType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    #region Ctrs
    public OrionGetUsageRequestData(string shopperId
      , string sourceUrl
      , string orderIo
      , string pathway
      , int pageCount
      , string orionResourceId
      , string usageType
      , DateTime startDate
      , DateTime endDate
      , TimeSpan requestTimeout)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      OrionResourceId = orionResourceId;
      UsageType = usageType;
      StartDate = startDate;
      EndDate = endDate;
      RequestTimeout = requestTimeout;

    }
    public OrionGetUsageRequestData(string shopperId
      , string sourceUrl
      , string orderIo
      , string pathway
      , int pageCount
      , string orionResourceId
      , string usageType
      , DateTime startDate
      , DateTime endDate)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      OrionResourceId = orionResourceId;
      UsageType = usageType;
      StartDate = startDate;
      EndDate = endDate;
      RequestTimeout = TimeSpan.FromSeconds(30d);

    }
    #endregion

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in OrionGetUsageRequestData");
    }
  }
}
