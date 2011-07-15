
using System.Collections.Generic;
namespace Atlantis.Framework.DCCGetDomainByShopper.Interface.Paging
{
  public class ExpirationDatePaging : IDomainPaging
  {
    const string SORT_ORDER_FIELD = "expirationdate";
    const string BOUNDARY_FIELD = "boundaryExpirationDate";
    const string BOUNDARY_UNIQUIFIER_FIELD = "boundaryDomain";

    public string SearchTerm { get; set; }

    public int RowsPerPage { get; set; }

    public string SortOrderField
    {
      get { return SORT_ORDER_FIELD; }
    }

    public SortOrderType SortOrder { get; set; }

    public string BoundaryField
    {
      get { return BOUNDARY_FIELD; }
    }

    public string BoundaryFieldValue { get; set; }

    public string BoundaryUniquifierField { get { return BOUNDARY_UNIQUIFIER_FIELD; } }

    public string BoundaryUniquifierFieldValue { get; set; }

    public bool IncludeBoundary { get; set; }

    public bool NavigatingForward { get; set; }

    public int? ExpirationDays { get; set; }

    public int? StatusType { get; set; }

    public List<int> TldIdList { get; set; }

    public bool SummaryOnly { get; set; }


    public ExpirationDatePaging()
    {
      TldIdList = new List<int>();
      RowsPerPage = 1;
    }

    public ExpirationDatePaging(SortOrderType sortOrder)
      : this()
    {
      SortOrder = sortOrder;
    }

    public ExpirationDatePaging(int rowsPerPage, string boundryExpirationDate, SortOrderType sortOrder)
      : this()
    {
      RowsPerPage = rowsPerPage;
      BoundaryFieldValue = boundryExpirationDate;
      SortOrder = sortOrder;
    }
  }
}
