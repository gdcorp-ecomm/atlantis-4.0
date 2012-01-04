using System;
using System.Data;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaAccountList.Interface.Abstract;
using Atlantis.Framework.MyaAccountList.Interface.Concrete;

namespace Atlantis.Framework.MyaAccountList.Interface
{
  public class MyaAccountListResponseData : IResponseData
  {
    private AtlantisException _atlException = null;
    private bool _success = false;
    private DataSet _productList = null;

    public IPageResult PageTotals { get; set; }

    public MyaAccountListResponseData(DataSet returnAccountList, int totalPages, int totalRecords)
    {
      PageTotals = new AccountListPagingResult(totalPages, totalRecords);
      _productList = returnAccountList;
      _success = (returnAccountList != null);
    }

    public MyaAccountListResponseData(MyaAccountListRequestData requestData, Exception ex)
    {
      _success = false;
      string data = string.Format("StoredProc: {0} | FreeOnly: {1} | ReturnAll: {2} | Filter: {3} | DaysToExpire: {4}", requestData.StoredProcName, requestData.ReturnFreeListOnly, requestData.ReturnAll, requestData.Filter, requestData.DaysTillExpiration);
      _atlException = new AtlantisException(requestData, "MyaAccountListResponseData", ex.Message, data);
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public DataSet AccountListDataSet
    {
      get { return _productList; }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _productList.GetXml();
    }

    #endregion

    public AtlantisException GetException()
    {
      return _atlException;
    }

  }
}
