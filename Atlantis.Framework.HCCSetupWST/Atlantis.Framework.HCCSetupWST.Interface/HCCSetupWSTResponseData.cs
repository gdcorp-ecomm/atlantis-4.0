using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCSetupWST.Interface
{
  public class HCCSetupWSTResponseData : IResponseData
  {
    readonly HCCSetUpHostingAccountResponse _hccResponse;
    readonly AtlantisException _exception = null;
    string _resultXml = string.Empty;

    readonly bool _success;

    public bool IsSuccess { get { return _success; } }

    public HCCSetupWSTResponseData(HCCSetUpHostingAccountResponse hccResponse)
    {
      _hccResponse = hccResponse;
      _success = true;
    }

    public HCCSetupWSTResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public HCCSetupWSTResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
        "HCCSetupWSTResponseData",
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

