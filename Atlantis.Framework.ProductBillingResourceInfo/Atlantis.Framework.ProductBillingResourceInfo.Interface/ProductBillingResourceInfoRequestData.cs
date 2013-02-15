using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductBillingResourceInfo.Interface
{
  public class ProductBillingResourceInfoRequestData : RequestData
  {
    public int PageNumber { get; set; }
    public int RowsPerPage { get; set; }
    public string SortByColumn { get; set; }
    public int ReturnAll { get; set; }
    public string SortDirection { get; set; }
    public List<string> ProfileList { get; set; }
    public int OnlyNulls { get; set; }
    public string InfoFilter { get; set; }
    public int BillingDateFilter { get; set; }
    public int DeptFilter { get; set; }
    public List<string> NameSpaceFilterList { get; set; }

    public ProductBillingResourceInfoRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount, List<string> profileList = null,
                                  int pageNumber = 1, int rowsPerPage = 25, string sortColumn = ColumnName.Info, string sortDirection = "asc", int onlyNulls = 0, string infoFilter = null, int billingDateFilter = -1, int deptFilter = -1, List<string> namespaceFilterList = null, int returnAll = 0)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ProfileList = new List<string>(1);
      ProfileList = profileList;
      PageNumber = pageNumber;
      RowsPerPage = rowsPerPage;
      SortByColumn = sortColumn;
      SortDirection = sortDirection;
      OnlyNulls = onlyNulls;
      InfoFilter = infoFilter;
      BillingDateFilter = billingDateFilter;
      DeptFilter = deptFilter;

      NameSpaceFilterList = new List<string>();
      NameSpaceFilterList = namespaceFilterList;

      ReturnAll = returnAll;

    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("ProductBillingResourceInfoRequestData is not a cachable request.");
    }

    #endregion
  }
}
