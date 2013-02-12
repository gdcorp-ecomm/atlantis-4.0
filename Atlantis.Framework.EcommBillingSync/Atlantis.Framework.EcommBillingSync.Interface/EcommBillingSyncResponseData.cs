using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommBillingSync.Interface
{
  public class EcommBillingSyncResponseData : IResponseData
  {
    private readonly AtlantisException _exception = null;
    private readonly string _resultXml = string.Empty;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public EcommBillingSyncResponseData()
    { }

     public EcommBillingSyncResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public EcommBillingSyncResponseData(RequestData requestData, Exception exception)
    {
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
