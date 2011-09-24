using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCIsUsernameValid.Interface
{
  public class HCCIsUsernameValidResponseData : IResponseData
  {
    HCCIsUsernameValidResponse _hccResponse;
    AtlantisException _exception = null;
    string _resultXml = string.Empty;

    bool _success = false;

    public bool IsSuccess { get { return _success; } }

    public HCCIsUsernameValidResponseData(HCCIsUsernameValidResponse hccResponse)
    {
      _hccResponse = hccResponse;
      _success = true;
    }

    public HCCIsUsernameValidResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public HCCIsUsernameValidResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
        "HCCIsUsernameValidResponseData",
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

    public HCCIsUsernameValidResponse Response { get { return _hccResponse; } }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
