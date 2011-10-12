using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.GoodAsGoldReport.Interface.Abstract;

namespace Atlantis.Framework.GoodAsGoldReport.Interface.Concrete
{
  public class GAGPagingResult : IPageResult
  {
    public int TotalPages { get; set; }

    public int TotalRecords { get; set; }

    public GAGPagingResult(int _totalPages, int _totalRecords)
    {
      TotalPages = _totalPages;
      TotalRecords = _totalRecords;
    }
  }
}
