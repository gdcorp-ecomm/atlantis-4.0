using System;
using System.Data;
using System.Data.SqlClient;

namespace Atlantis.Framework.MYAGetProduct.Impl
{
  #region Enums
  internal enum SortDirection
  {
    ASC,
    DESC
  }
  #endregion

  [Serializable]
  internal class PagingInfo
  {
    #region Properties

    private bool _returnAll;
    private SortDirection _sortDirection;
    private string _sortField;
    private int _currentPage;
    private int _rowsPerPage;
    private int _numberOfRecords;
    private int _numberOfPages;
    private bool _hasOutputParameters = true;
    private bool _hasPagingInfo = true;
    private bool _hasReturnAllFlag = true;

    internal string SortField
    {
      get { return _sortField; }
      set { _sortField = value; }
    }

    internal int NumberOfRecords
    {
      get { return _numberOfRecords; }
      set { _numberOfRecords = value; }
    }

    internal int NumberOfPages
    {
      get { return _numberOfPages; }
      set { _numberOfPages = value; }
    }

    internal bool HasOutputParameters
    {
      get { return _hasOutputParameters; }
      set { _hasOutputParameters = value; }
    }

    internal bool HasPagingInfo
    {
      get { return _hasPagingInfo; }
      set { _hasPagingInfo = value; }
    }

    internal bool HasReturnAllFlag
    {
      get { return _hasReturnAllFlag; }
      set { _hasReturnAllFlag = value; }
    }
    #endregion

    #region Constructor
    public PagingInfo(Atlantis.Framework.MYAGetProduct.Interface.PageHelper.PagingInfo pi)
    {
      _returnAll = pi.ReturnAll;
      _sortDirection = (SortDirection)pi.SortDirection;
      _sortField = pi.SortField;
      _currentPage = pi.CurrentPage;
      _rowsPerPage = pi.RowsPerPage;
    }
    #endregion

    #region Methods
    internal void AddParameters(SqlCommand data)
    {
      if (_hasPagingInfo)
      {
        if (_hasOutputParameters)
        {
          AddInputAndOutputParameters(data);
        }
        else
        {
          AddInputParametersOnly(data);
        }
      }
    }

    private void AddInputAndOutputParameters(SqlCommand data)
    {
      if (this.SortField != null)
      {
        data.Parameters.Add(new SqlParameter("@sortcol", this.SortField));
      }
      data.Parameters.Add(new SqlParameter("@sortdir", _sortDirection.ToString()));
      data.Parameters.Add(new SqlParameter("@pageno", _currentPage));
      data.Parameters.Add(new SqlParameter("@rowsperpage", _rowsPerPage));
      data.Parameters.Add(new SqlParameter("@returnAllFlag", _returnAll ? 1 : 0));
      data.Parameters.Add(new SqlParameter("@totalrecords", SqlDbType.Int, 0));
      data.Parameters["@totalrecords"].Direction = ParameterDirection.Output;
      data.Parameters.Add(new SqlParameter("@totalpages", SqlDbType.Int, 0));
      data.Parameters["@totalpages"].Direction = ParameterDirection.Output;
    }

    private void AddInputParametersOnly(SqlCommand data)
    {
      if (this.SortField != null)
      {
        data.Parameters.Add(new SqlParameter("@sortcol", this.SortField));
      }
      data.Parameters.Add(new SqlParameter("@sortdir", _sortDirection.ToString()));
      data.Parameters.Add(new SqlParameter("@pageno", _currentPage));
      data.Parameters.Add(new SqlParameter("@rowsperpage", _rowsPerPage));
      if (_hasReturnAllFlag)
        data.Parameters.Add(new SqlParameter("@returnAllFlag", _returnAll ? 1 : 0));
    }

    internal void RetreiveOutputParameters(SqlCommand data)
    {
      this.NumberOfRecords = Convert.ToInt32(data.Parameters["@totalrecords"].Value);
      this.NumberOfPages = Convert.ToInt32(data.Parameters["@totalpages"].Value);
    }
    #endregion
  }
}