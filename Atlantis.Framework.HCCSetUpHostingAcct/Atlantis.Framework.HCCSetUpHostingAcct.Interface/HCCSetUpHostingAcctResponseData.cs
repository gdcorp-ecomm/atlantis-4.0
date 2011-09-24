using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCSetUpHostingAcct.Interface
{
  public class HCCSetUpHostingAcctResponseData : IResponseData
  {
    HCCSetUpHostingAccountResponse _hccResponse;
    AtlantisException _exception = null;
    string _resultXml = string.Empty;

    bool _success = false;

    public bool IsSuccess { get { return _success; } }

    public HCCSetUpHostingAcctResponseData(HCCSetUpHostingAccountResponse hccResponse)
    {
      _hccResponse = hccResponse;
      _success = true;
    }

    public HCCSetUpHostingAcctResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public HCCSetUpHostingAcctResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
        "HCCSetUpHostingAcctResponseData",
        exception.Message,
        string.Empty);        
    }

    public string ToXML()
    {
      if (_hccResponse != null)
      {
        _resultXml = _hccResponse.ToXML();
      }      
      
      return _resultXml;
    }

    public HCCSetUpHostingAccountResponse Response { get { return _hccResponse; } }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
