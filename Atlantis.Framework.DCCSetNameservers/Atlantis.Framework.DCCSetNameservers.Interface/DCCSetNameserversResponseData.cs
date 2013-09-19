using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetNameservers.Interface
{
  public class DCCSetNameserversResponseData : IResponseData
  {
    private readonly bool _isSuccess;
    public bool IsSuccess
    {
      get { return (_exception == null && _isSuccess); }
    }

    public ValidationErrors Errors { get; private set; }
    
    public DCCSetNameserversResponseData(bool isSuccess, string responseXml)
    {
      _isSuccess = isSuccess;
      _responseXml = responseXml;

      if (!isSuccess)
      {
        Errors = new ValidationErrors(responseXml);
        if (DataCache.DataCache.GetAppSetting("ATLANTIS.SET_NAMESERVERS_LOG_ERRORS") != "0")
        {
          var ex = new AtlantisException("DCCSetNameserversResponseData", 0, "DCCSetNameserversResponseData returned false", responseXml);
          Engine.Engine.LogAtlantisException(ex);
        }
      }
    }

    public DCCSetNameserversResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public DCCSetNameserversResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                   "DCCSetNameserversResponseData",
                                   ex.Message,
                                   ex.StackTrace);
    }

    private readonly AtlantisException _exception;
    public AtlantisException GetException()
    {
      return _exception;
    }

    private readonly string _responseXml;
    public string ToXML()
    {
      return _responseXml;
    }
  }
}
