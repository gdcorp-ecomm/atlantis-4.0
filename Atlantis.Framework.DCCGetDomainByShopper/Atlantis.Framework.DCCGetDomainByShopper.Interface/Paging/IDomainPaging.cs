
using System.Collections.Generic;
namespace Atlantis.Framework.DCCGetDomainByShopper.Interface.Paging
{
  public interface IDomainPaging
  {
    string SearchTerm { get; set; }

    int RowsPerPage { get; set; }

    string SortOrderField { get; }

    SortOrderType SortOrder { get; set; }

    string BoundaryField { get; }

    string BoundaryFieldValue { get; set; }

    string BoundaryUniquifierField { get; }

    string BoundaryUniquifierFieldValue { get; set; }

    bool IncludeBoundary { get; set; }

    bool NavigatingForward { get; set; }

    int? ExpirationDays { get; set; }

    int? StatusType { get; set; }

    List<int> TldIdList { get; set; }

    bool SummaryOnly { get; set; }
  }
}
