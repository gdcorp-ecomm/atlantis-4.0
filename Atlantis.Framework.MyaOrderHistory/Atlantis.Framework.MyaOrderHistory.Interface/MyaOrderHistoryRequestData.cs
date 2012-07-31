using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaOrderHistory.Interface
{
  public class MyaOrderHistoryRequestData : RequestData
  {

    #region Properties

    public string StoredProcedureName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string DomainName { get; set; }
    public int ProductGroupId { get; set; }
    public int ProductTypeId { get; set; }
    public int PaymentProfileId { get; set; }

    public int PageNumber { get; set; }
    public int RowsPerPage { get; set; }
    public string SortDirection { get; set; }
    public string SortByColumn { get; set; }
    public int ReturnAll { get; set; }
        
    #endregion

    public MyaOrderHistoryRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  int pageNumber = 1, int rowsPerPage = 5, string sortColumn = SortColumn.DateEntered, string sortDirection = "desc", int returnAll = 0)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PageNumber = pageNumber;
      RowsPerPage = rowsPerPage;
      SortByColumn = sortColumn;
      SortDirection = sortDirection;
      ReturnAll = returnAll;
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("MyaOrderHistoryRequestData is not a cachable request.");
    }

    #endregion

  }

}
