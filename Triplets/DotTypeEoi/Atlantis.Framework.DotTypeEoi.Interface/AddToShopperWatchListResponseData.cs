using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class AddToShopperWatchListResponseData : IResponseData
  {
    private readonly string _responseXml;
    private readonly AtlantisException _exception;
    private readonly bool _isSuccess;

    public AddToShopperWatchListResponseData(AtlantisException exception)
    {
      _exception = exception;
      _isSuccess = false;
    }

    public AddToShopperWatchListResponseData(string responseXml)
    {
      _exception = null;
      _responseXml = responseXml;
      _isSuccess = true;
    }

    public AddToShopperWatchListResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
      _isSuccess = false;
    }

    public AddToShopperWatchListResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(requestData, "AddToShopperWatchListResponseData", ex.Message, requestData.ToXML());
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
