using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCRemoveDomain.Interface
{
  public class HCCRemoveDomainResponseData : IResponseData
  {
    private readonly AtlantisException _exception = null;
    private string _resultXml = string.Empty;
    private readonly bool _success = false;
    private readonly HCCDomainMgmtResponse _response;

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public HCCDomainMgmtResponse Response
    {
      get { return _response; }
    }

    public HCCRemoveDomainResponseData(HCCDomainMgmtResponse response)
    {
      _success = true;
      _response = response;
    }

    public HCCRemoveDomainResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public HCCRemoveDomainResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HCCRemoveDomainResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      if (Response != null)
      {
        _resultXml = Response.ToXML();
      }

      return _resultXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
