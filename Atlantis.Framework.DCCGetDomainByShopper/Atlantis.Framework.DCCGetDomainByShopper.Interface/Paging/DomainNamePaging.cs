
using System.Collections.Generic;
namespace Atlantis.Framework.DCCGetDomainByShopper.Interface.Paging
{
  public class DomainNamePaging : IDomainPaging
  {
    const string SORT_ORDER_FIELD = "domainName";
    const string BOUNDARY_FIELD = "boundaryDomain";

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

    public bool NavigatingForward { get; set; }

    public string BoundaryUniquifierField 
    {
      get { return null; }
    }

    public string BoundaryUniquifierFieldValue { get; set; }

    public bool IncludeBoundary { get; set; }

    public int? ExpirationDays { get; set; }

    public int? StatusType { get; set; }

    public List<int> TldIdList { get; set; }

    public bool SummaryOnly { get; set; }


    public DomainNamePaging()
    {
      TldIdList = new List<int>();
      RowsPerPage = 1;
    }

    public DomainNamePaging(SortOrderType sortOrder) : this()
    {
      SortOrder = sortOrder;
    }

    public DomainNamePaging(int rowsPerPage, string boundryDomainName, SortOrderType sortOrder) : this()
    {
      RowsPerPage = rowsPerPage;
      BoundaryFieldValue = boundryDomainName;
      SortOrder = sortOrder;
    }
  }
}
