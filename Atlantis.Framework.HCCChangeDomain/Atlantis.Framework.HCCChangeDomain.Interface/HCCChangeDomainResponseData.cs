using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCChangeDomain.Interface
{
  public class HCCChangeDomainResponseData : IResponseData
  {
    HCCDomainMgmtResponse _response;
    AtlantisException _exception = null;
    string _resultXml = string.Empty;

    bool _success = false;

    public bool IsSuccess { get { return _success; } }


    public HCCChangeDomainResponseData() { }
    public HCCChangeDomainResponseData(HCCDomainMgmtResponse hccResponse)
    {
      _response = hccResponse;
      _success = hccResponse.GetResponseStatusCode() == 0;
    }

    public HCCChangeDomainResponseData(string xml)
    {
      _resultXml = xml;
      _success = true;
    }

    public HCCChangeDomainResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public HCCChangeDomainResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
        "HCCChangeDomainResponseData",
        exception.Message,
        string.Empty);
    }


    public HCCDomainMgmtResponse Response { get { return _response; } }

    public string ToXML()
    {
      if (_response != null)
      {
        _resultXml = _response.ToXML();
      }

      return _resultXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

  }
}
