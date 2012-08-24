using System;
using System.Net;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoOrderLevelDeletePL.Interface
{
  public class PromoOrderLevelDeletePLResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private RequestData _request = null;
    private bool _isSuccess = true;
    private string _xmlResponse = null;

    public PromoOrderLevelDeletePLResponseData(RequestData request, string xmlResponse)
    {
      this._request = request;
      this._xmlResponse = xmlResponse;
    }

    public PromoOrderLevelDeletePLResponseData(RequestData request, string xmlResponse, AtlantisException exception)
    {
      this._request = request;
      this._xmlResponse = xmlResponse;
      this._exception = exception;
      this._isSuccess = false;
    }

    public PromoOrderLevelDeletePLResponseData(RequestData request, WebExceptionStatus webExceptionStatus)
    {
      this._request = request;
      this._exception = new AtlantisException(request, "PromoOrderLevelDeletePLRequest::ProcessInEngine", Enum.GetName(typeof(WebExceptionStatus), webExceptionStatus), string.Empty);
    }

    public PromoOrderLevelDeletePLResponseData(RequestData request, string xmlResponse, Exception exception)
    {
      this._request = request;
      this._xmlResponse = xmlResponse;
      this._exception = new AtlantisException(request, "OrderLevelPromoDeletePL::ExecuteEngine", exception.Message, exception.StackTrace);
    }

    public string ToXML()
    {
      return _request.ToXML();
    }

    public bool IsSuccess()
    {
      return this._isSuccess; 
    }

    public AtlantisException GetException()
    {
      return this._exception;
    }
  }
}
