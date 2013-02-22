using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommBillingSync.Interface
{
  public class EcommBillingSyncResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly string _resultXml = string.Empty;
    public readonly List<BillingSyncErrorData> BillingSyncErrors = null;
    public bool IsSuccess { get; private set; }

    public EcommBillingSyncResponseData(List<BillingSyncErrorData> billingSyncErrors)
    {
      IsSuccess = true;
      BillingSyncErrors = billingSyncErrors;
    }

    public EcommBillingSyncResponseData(AtlantisException atlantisException, List<BillingSyncErrorData> billingSyncErrors)
    {
      IsSuccess = false;
      BillingSyncErrors = billingSyncErrors;
      _exception = atlantisException;
    }

    public EcommBillingSyncResponseData(RequestData requestData, Exception exception, List<BillingSyncErrorData> billingSyncErrors)
    {
      IsSuccess = false;
      BillingSyncErrors = billingSyncErrors;
      _exception = new AtlantisException(requestData
        , "EcommBillingSyncResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _resultXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
