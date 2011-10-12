using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.GoodAsGoldReport.Interface.Abstract;
using Atlantis.Framework.GoodAsGoldReport.Interface.Concrete;


namespace Atlantis.Framework.GoodAsGoldReport.Interface 
{
  public class GoodAsGoldReportRequestData : RequestData
  {
    private DateTime _startDate;
    private DateTime _endDate;


    public GoodAsGoldReportRequestData(string shopperId, string sourceURL, string orderId, string pathway,
                                       int pageCount, DateTime startDate, DateTime endDate, int pageSize = 5,
                                       int currentPage = 1, string sortColumn = "displayDate", string sortDirection = "asc", string filter = "none")
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _startDate = startDate;
      _endDate = endDate;
      PageInfo = new GAGPagingInfo();
      PageInfo.PageSize = pageSize;
      PageInfo.CurrentPage = currentPage;
      SortColumn = sortColumn;
      SortDirection = sortDirection;
      Filter = filter;
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

    public IPageInfo PageInfo { get; set; }

    public string SortColumn { get; set; }

    public string SortDirection { get; set; }

    public string Filter { get; set; }
  }
}
