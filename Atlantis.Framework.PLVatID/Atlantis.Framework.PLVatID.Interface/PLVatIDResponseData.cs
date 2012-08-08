using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PLVatID.Interface
{
  public class PLVatIDResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public string VATID
    {
      get;
      set;
    }

    public PLVatIDResponseData(string vatId)
    {
      VATID = vatId;
    }

    public PLVatIDResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public PLVatIDResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "PLVatIDResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
