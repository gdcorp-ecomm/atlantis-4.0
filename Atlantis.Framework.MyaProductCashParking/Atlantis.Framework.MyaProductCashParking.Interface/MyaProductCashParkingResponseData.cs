using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaProduct.Interface;

namespace Atlantis.Framework.MyaProductCashParking.Interface
{
  public class MyaProductCashParkingResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;
    public IList<CashParkingProduct> CashParkingProducts { get; private set; }
    public IPagingResult PagingResult { get; private set; }
    public bool IsSuccess
    {
      get { return _atlantisException == null; }
    }

    public MyaProductCashParkingResponseData(IList<CashParkingProduct> cashParkingProducts, int totalRecord, int totalPages)
    {
      CashParkingProducts = cashParkingProducts;
      PagingResult = new CashParkingPagingResult(totalRecord, totalPages);
    }

    public MyaProductCashParkingResponseData(AtlantisException atlantisException)
    {
      CashParkingProducts = new List<CashParkingProduct>(1);
      PagingResult = new CashParkingPagingResult(0, 0);
      _atlantisException = atlantisException;
    }

    public MyaProductCashParkingResponseData(RequestData requestData, Exception ex)
    {
      CashParkingProducts = new List<CashParkingProduct>(1);
      PagingResult = new CashParkingPagingResult(0, 0);
      _atlantisException = new AtlantisException(requestData
        , MethodBase.GetCurrentMethod().DeclaringType.FullName
        , string.Format("MyaProductCashParking Error: {0}", ex.Message)
        , ex.Data.ToString()
        , ex);
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion
  }
}
