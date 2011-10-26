using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PremiumDNS.Interface
{
  public class GetPremiumDNSStatusResponseData : IResponseData
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

    public bool HasPremiumDNS { get; private set; }

    public GetPremiumDNSStatusResponseData(bool status)
    {
      try
      {
        _success = true;
        HasPremiumDNS = status;
      } catch
      {
        _success = false;
      }
    }

     public GetPremiumDNSStatusResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
       _success = false;
    }

    public GetPremiumDNSStatusResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "GetPremiumDNSStatusResponseData",
                                   exception.Message,
                                   requestData.ToXML());
      _success = false;
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
