using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GoodAsGoldReport.Interface 
{
  public class GoodAsGoldReportRequestData : RequestData
  {
    private DateTime _startDate;
    private DateTime _endDate;

    public GoodAsGoldReportRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, DateTime startDate, DateTime endDate)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {

      _startDate = startDate;
      _endDate = endDate;

    }

    public DateTime StateDate
    {
      get { return _startDate; }
    }

    public DateTime EndDate
    {
      get { return _endDate; }
    }

    public override string GetCacheMD5()
    {
      throw new Exception("GoodAsGoldReport is not a cacheable request.");
    }

  }
}
