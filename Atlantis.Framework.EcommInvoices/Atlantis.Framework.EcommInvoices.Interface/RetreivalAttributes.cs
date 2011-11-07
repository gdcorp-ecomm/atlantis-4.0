using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.EcommInvoices.Interface
{
  public class RetrievalAttributes
  {
    public int Filter { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public string SortColumn {get;set;}
    public string SortDirection { get; set; }

    public RetrievalAttributes(int filter = InvoiceStatus.All, int pageSize = 5, int currentPage = 1, string sortColumn = "create_date", string sortDirection = "desc")
    {
      Filter = filter;
      PageSize = pageSize;
      CurrentPage = currentPage;
      SortColumn = sortColumn;
      SortDirection = sortDirection;
    }
  }
}
