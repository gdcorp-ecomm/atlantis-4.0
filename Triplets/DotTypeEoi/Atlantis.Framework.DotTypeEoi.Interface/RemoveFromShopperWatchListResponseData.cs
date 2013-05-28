using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class RemoveFromShopperWatchListResponseData : IResponseData
  {
    private readonly string _responseXml;
    private readonly AtlantisException _exception;
    private readonly bool _isSuccess;

    public RemoveFromShopperWatchListResponseData(AtlantisException exception)
    {
      _exception = exception;
      _isSuccess = false;
    }

    public RemoveFromShopperWatchListResponseData(string responseXml)
    {
      _exception = null;
      _responseXml = responseXml;
      _isSuccess = true;
    }

    public RemoveFromShopperWatchListResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
      _isSuccess = false;
    }

    public RemoveFromShopperWatchListResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(requestData, "RemoveFromShopperWatchListResponseData", ex.Message, requestData.ToXML());
      _isSuccess = false;
    }

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public string ResponseMessage
    {
      get { return _responseXml; }
    }
  }
}
