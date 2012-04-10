using System;
using System.Data;
using System.Data.SqlClient;

namespace Atlantis.Framework.MYAGetProduct.Interface.PageHelper
{
  #region Enums
  public enum SortDirection
  {
    ASC,
    DESC
  }
  #endregion

  [Serializable]
  public class PagingInfo
  {
    #region Properties

    private bool _returnAll = false;
    private SortDirection _sortDirection = SortDirection.ASC;
    private string _sortField;
    private int _currentPage = 1;
    private int _rowsPerPage = 20;
    private int _numberOfRecords = 0;
    private int _numberOfPages = 0;

    public bool ReturnAll
    {
      get { return _returnAll; }
      set { _returnAll = value; }
    }

    public SortDirection SortDirection
    {
      get { return _sortDirection; }
      set { _sortDirection = value; }
    }

    public string SortField
    {
      get { return _sortField; }
      set { _sortField = value; }
    }

    public int CurrentPage
    {
      get { return _currentPage; }
      set { _currentPage = value; }
    }

    public int RowsPerPage
    {
      get { return _rowsPerPage; }
      set { _rowsPerPage = value; }
    }

    public int NumberOfRecords
    {
      get { return _numberOfRecords; }
      set { _numberOfRecords = value; }
    }

    public int NumberOfPages
    {
      get { return _numberOfPages; }
      set { _numberOfPages = value; }
    }
    #endregion
    public PagingInfo()
    {
    }

  }
}