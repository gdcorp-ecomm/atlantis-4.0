using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RenewalsGetServiceRecords.Interface
{
  public class RenewalsGetServiceRecordsRequestData : RequestData
  {
    private int _daystoexpiration = 90;
    public int DaysToExpiration
    {
      get
      {
        return _daystoexpiration;
      }
      set
      {
        if (value > 0)
        {
          _daystoexpiration = value;
        }
      }
    }

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

    private int _rowsperpage = 500;
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

    private string _sortcolumn = "description";
    public string SortColumn
    {
      get
      {
        return _sortcolumn;
      }
      set
      {
        _sortcolumn = value;
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
          if (value.Length > 5)
          {
            _sortdirection = value.Substring(0, 5);
          }
          else
          {
            _sortdirection = value;
          }
        }
      }
    }

    private int _allrecords = 1;
    public int AllRecords
    {
      get
      {
        return _allrecords;
      }
      set
      {
        _allrecords = value;
      }
    }

    private int _syncableonly = 0;
    public int SyncableOnly
    {
      get
      {
        return _syncableonly;
      }
      set
      {
        _syncableonly = value;
      }
    }

    private DateTime _promostartdate = DateTime.Now;
    public DateTime PromoStartDate
    {
      get
      {
        return _promostartdate;
      }
      set
      {
        _promostartdate = value;
      }
    }

    public RenewalsGetServiceRecordsRequestData(
      string shopperID,
      string sourceURL,
      string orderID,
      string pathway,
      int pageCount)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public RenewalsGetServiceRecordsRequestData(
      string shopperID,
      string sourceURL,
      string orderID,
      string pathway,
      int pageCount, int daysToExpiration, int pageNumber, string sortColumn, string sortDirection, int allRecords, int syncableOnly, DateTime promoStartDate)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      DaysToExpiration = daysToExpiration;
      PageNumber = pageNumber;
      SortColumn = sortColumn;
      SortDirection = sortDirection;
      AllRecords = allRecords;
      SyncableOnly = syncableOnly;
      PromoStartDate = promoStartDate;
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      return string.Empty;
    }

    #endregion
  }
}
