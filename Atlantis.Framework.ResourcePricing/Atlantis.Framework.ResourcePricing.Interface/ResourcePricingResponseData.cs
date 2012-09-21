using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ResourcePricing.Interface
{
  public class ResourcePricingResponseData:IResponseData
  {
    private readonly AtlantisException _atlantisException;
    private readonly string _responseXml;
    private readonly RequestData _request;

    public bool IsSuccess { get; private set; }

    public ResourcePricingResponseData(RequestData request, string responseXml) 
    {
      IsSuccess = true;
      _request = request;
      _responseXml = responseXml;
    }

    public ResourcePricingResponseData(string responseXml, AtlantisException atlantisException)
    {
      IsSuccess = false;
      _responseXml = responseXml;
      _atlantisException = atlantisException;
    }

    public ResourcePricingResponseData(AtlantisException atlantisException)
    {
      IsSuccess = false;
      _atlantisException = atlantisException;
    }

    public ResourcePricingResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      _atlantisException = new AtlantisException(requestData
        , requestData.GetType().ToString()
        , string.Format("ResourcePricingResponseData Error: {0}", ex.Message)
        , ex.StackTrace
        , ex);                                   
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion IResponseData Members

  }
}
