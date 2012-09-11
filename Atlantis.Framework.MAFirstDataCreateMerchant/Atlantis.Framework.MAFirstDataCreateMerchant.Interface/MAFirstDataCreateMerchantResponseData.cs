using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MAFirstDataCreateMerchant.Interface
{
  public class MAFirstDataCreateMerchantResponseData : IResponseData
  {
    private AtlantisException _exception = null;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public MAFirstDataCreateMerchantResponseData()
    { }

    public MAFirstDataCreateMerchantResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "MAFirstDataCreateMerchantResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in MAFirstDataCreateMerchantResponseData");
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion
  }
}
