using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaAccountList.Interface.Abstract;
using Atlantis.Framework.MyaAccountList.Interface.Concrete;

namespace Atlantis.Framework.MyaAccountList.Interface
{
  public class MyaAccountListRequestData : RequestData
  {
    
    public MyaAccountListRequestData(string shopperId, string sourceURL, string orderId, string pathway,
                                       int pageCount, string storedProcName, int pageSize = 5, 
                                       int currentPage = 1, string sortDirection = "asc",
                                       int? daysTillExpiration = null, int returnFreeListOnly = 0, int returnAllFlag = 0,  
                                       string filter = null)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      StoredProcName = storedProcName;
      PageInfo = new AccountListPagingInfo();
      PageInfo.PageSize = pageSize;
      PageInfo.CurrentPage = currentPage;
      SortDirection = sortDirection;
      Filter = filter;
      ReturnFreeListOnly = returnFreeListOnly;
      DaysTillExpiration = daysTillExpiration;
      ReturnAll = returnAllFlag;
    }

    public IPageInfo PageInfo { get; set; }

    public string SortColumn { get; set; }

    public string SortDirection { get; set; }

    public string Filter { get; set; }
    
    public string StoredProcName { get; set; }

    public int ReturnFreeListOnly { get; set; }

    public int? DaysTillExpiration { get; set; }

    public int ReturnAll { get; set; }

    public override string GetCacheMD5()
    {
      throw new Exception("MyaAccountList is not a cacheable request.");
    }

  }
}
