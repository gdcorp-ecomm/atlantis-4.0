using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaOrderHistory.Interface
{
  public class MyaOrderHistoryResponseData : IResponseData
  {
    private AtlantisException _atlException = null;

    public MyaOrderHistoryResponseData(int numberOfRecords, int numberOfPages, List<ReceiptItem> receiptDetails)
    {
      TotalRecords = numberOfRecords;
      TotalPages = numberOfPages;
      GetRecords = receiptDetails;
      _isSuccess = true;
    }

    public MyaOrderHistoryResponseData(AtlantisException exAtlantis)
    {
      TotalRecords = 0;
      TotalPages = 0;
      GetRecords = new List<ReceiptItem>();
      _atlException = exAtlantis;
    }

    public MyaOrderHistoryResponseData(RequestData oRequestData, Exception ex)
    {
      TotalRecords = 0;
      TotalPages = 0;
      GetRecords = new List<ReceiptItem>();
      _atlException = new AtlantisException(oRequestData, "MyaOrderHistoryResponseData", ex.Message, string.Empty);
    }

    private bool _isSuccess = false;
    public bool IsSuccess
    {
      get
      {
        return _isSuccess;
      }
    }

    public int TotalRecords { get; private set; }

    public int TotalPages { get; private set; }

    public List<ReceiptItem> GetRecords { get; private set; }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _atlException;
    }

    public string ToXML()
    {
      return string.Empty;
    }

    #endregion
  }
}
