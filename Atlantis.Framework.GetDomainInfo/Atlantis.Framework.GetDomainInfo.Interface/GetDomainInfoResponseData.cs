using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;


namespace Atlantis.Framework.GetDomainInfo.Interface
{
  public class GetDomainInfoResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public GetDomainInfoResponseData()
    {
    }

    public GetDomainInfoResponseData(string resultXML)
    {
      _resultXML = resultXML;
      _success = true;
    }

    public GetDomainInfoResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public GetDomainInfoResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "GetDomainInfoResponseData",
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

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }
    #endregion

    #region Implementation of ISessionSerializableResponse

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      _success = true;
      _resultXML = sessionData;
    }


    #endregion


  }
}

