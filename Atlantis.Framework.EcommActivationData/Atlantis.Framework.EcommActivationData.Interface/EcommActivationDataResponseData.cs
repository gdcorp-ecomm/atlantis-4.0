using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommActivationData.Interface
{
  public class EcommActivationDataResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public EcommActivationDataResponseData(string responseXML)
    {
      this._success = true;
      this._resultXML = responseXML;
    }

    public EcommActivationDataResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;      
    }

    public EcommActivationDataResponseData(string responseXML, AtlantisException atlantisException)
    {
      this._exception = atlantisException;
      this._resultXML = responseXML;
    }

    public EcommActivationDataResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "EcommActivationDataResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
