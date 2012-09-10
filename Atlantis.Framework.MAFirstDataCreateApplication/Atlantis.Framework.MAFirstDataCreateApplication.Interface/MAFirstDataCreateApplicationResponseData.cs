using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MAFirstDataCreateApplication.Interface
{
  public class MAFirstDataCreateApplicationResponseData : IResponseData
  {
    private AtlantisException _exception = null;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public MAFirstDataCreateApplicationResponseData()
    { }

     public MAFirstDataCreateApplicationResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public MAFirstDataCreateApplicationResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "MAFirstDataCreateApplicationResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in MAFirstDataCreateApplicationResponseData");
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion
  }
}
