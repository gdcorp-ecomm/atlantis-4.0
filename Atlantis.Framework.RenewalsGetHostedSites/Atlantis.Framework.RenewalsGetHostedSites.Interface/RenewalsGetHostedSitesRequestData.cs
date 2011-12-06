using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RenewalsGetHostedSites.Interface
{
  public class RenewalsGetHostedSitesRequestData : RequestData
  {
    private int _pagenumber = 1;
    public int PageNumber
    {
      get
      {
        return _pagenumber;
      }
      set
      {
        if (value > 0)
        {
          _pagenumber = value;
        }
      }
    }

    private int _rowsperpage = 300;
    public int RowsPerPage
    {
      get
      {
        return _rowsperpage;
      }
      set
      {
        if (value > 0)
        {
          _rowsperpage = value;
        }
      }
    }

    private string _sortcolumn = "product_name";
    public string SortColumn
    {
      get
      {
        return _sortcolumn;
      }
      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          _sortcolumn = value;
        }
      }
    }

    private string _sortdirection = "asc";
    public string SortDirection
    {
      get
      {
        return _sortdirection;
      }
      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          _sortdirection = value;
        }
      }
    }

    public RenewalsGetHostedSitesRequestData(
      string shopperID,
      string sourceURL,
      string orderID,
      string pathway,
      int pageCount)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public RenewalsGetHostedSitesRequestData(
      string shopperID,
      string sourceURL,
      string orderID,
      string pathway,
      int pageCount, int pageNumber, int rowsPerPage, string sortColumn, string sortDirection)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      PageNumber = pageNumber;
      RowsPerPage = rowsPerPage;
      SortColumn = sortColumn;
      SortDirection = sortDirection;
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      return string.Empty;
    }

    #endregion
  }
}
