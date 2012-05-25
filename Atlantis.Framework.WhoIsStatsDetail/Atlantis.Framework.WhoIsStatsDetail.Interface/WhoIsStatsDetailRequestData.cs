using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WhoIsStatsDetail.Interface
{
  public class WhoIsStatsDetailRequestData : RequestData
  {
    private int statsLogIDValue = 0;
    private string logDetailValue = string.Empty;
    private DateTime startTimeValue = DateTime.Now;
    private DateTime endTimeValue = DateTime.Now;
    private int elapsedTimeValue = 0;

    public int StatsLogId
    {
      get { return statsLogIDValue; }
      set { statsLogIDValue = value; }
    }

    public string LogDetail
    {
      get { return logDetailValue; }
      set { logDetailValue = value; }
    }

    public DateTime StartTime
    {
      get { return startTimeValue; }
      set { startTimeValue = value; }
    }

    public DateTime EndTime
    {
      get { return endTimeValue; }
      set { endTimeValue = value; }
    }

    public int ElapsedTime
    {
      get { return elapsedTimeValue; }
      set { elapsedTimeValue = value; }
    }

    public WhoIsStatsDetailRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
        int statsLogID, string logDetail, DateTime startTime, DateTime endTime, int elapsedTime)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      statsLogIDValue = statsLogID;
      logDetailValue = logDetail;
      startTimeValue = startTime;
      endTimeValue = endTime;
      elapsedTimeValue = elapsedTime;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("WhoIsStatsDetail is not a cacheable Request.");
    }
  }
}

