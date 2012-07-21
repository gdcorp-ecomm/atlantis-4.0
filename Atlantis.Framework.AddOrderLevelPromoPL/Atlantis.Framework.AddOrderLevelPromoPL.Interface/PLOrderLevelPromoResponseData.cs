using System;
using System.Net;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddOrderLevelPromoPL.Interface
{
  public class PLOrderLevelPromoResponseData : IResponseData
  {
    public enum RequestSuccessCode
    {
      Success = 0,
      Failed = 1,
      FailedPromoAlreadyExists = 2,
      FailedInvalidRequestFormat = 3,
      FailedInvalidDateSpecification = 4,
      FailedInvalidAwardSpecification = 5,
      FailedInvalidCurrencySpecification = 6,
      FailedInvalidPromoIdOrVersion = 7
    }

    private string _responseXml = string.Empty;
    private AtlantisException _atlantisException = null;
    private RequestData _requestData = null;
    private RequestSuccessCode _isSuccess = RequestSuccessCode.Success;

    public PLOrderLevelPromoResponseData(RequestData requestData, string responseXml)
    {
      this._requestData = requestData;
      this._responseXml = responseXml;
    }

    public PLOrderLevelPromoResponseData(RequestData requestData, string responseXml, AtlantisException exception)
    {
      this._atlantisException = new AtlantisException(requestData, "Atlantis.Framework.ResellerOrderLevelPromoResponseData", exception.Message, responseXml);
      this._isSuccess = RequestSuccessCode.Failed;
    }

    public PLOrderLevelPromoResponseData(RequestData requestData, WebExceptionStatus exception)
    {
      this._isSuccess = RequestSuccessCode.Failed;
      this._atlantisException = new AtlantisException(requestData, "Atlantis.Framework.ResellerOrderLevelPromoResponseData", Enum.GetName(typeof(WebExceptionStatus), exception), string.Empty);
    }

    public PLOrderLevelPromoResponseData(string responseXml, RequestData requestData, Exception exception)
    {
      this._atlantisException = new AtlantisException(requestData, "Atlantis.Framework.ResellerOrderLevelPromoResponseData", exception.Message, responseXml);
      this._isSuccess = RequestSuccessCode.Failed;
    }

    public PLOrderLevelPromoResponseData(RequestData requestData, PLOrderLevelPromoException exception)
    {
      this._requestData = requestData;
      this._atlantisException = new AtlantisException(requestData, "PLOrderLevelPromo", exception.Message, Enum.GetName(typeof(PLOrderLevelPromoExceptionReason), exception.ExceptionReason));
    }

    public PLOrderLevelPromoResponseData(RequestData requestData, string responseXml, PLOrderLevelPromoException exception)
    {
      this._requestData = requestData;
      this._responseXml = responseXml;
      this._atlantisException = new AtlantisException(requestData, "PLOrderLevelPromo", exception.Message, Enum.GetName(typeof(PLOrderLevelPromoExceptionReason), exception.ExceptionReason));
      this._isSuccess = ParseRequestSuccessCodeFromException(exception);
    }

    public RequestSuccessCode IsSuccess()
    {
      return this._isSuccess;
    }

    public AtlantisException GetException()
    {
      return this._atlantisException;
    }

    public string ToXML()
    {
      return this._responseXml;
    }

    private static RequestSuccessCode ParseRequestSuccessCodeFromException(PLOrderLevelPromoException exception)
    {
      RequestSuccessCode result = RequestSuccessCode.Success;
      if (exception != null)
      {
        switch (exception.ExceptionReason)
        {
          case  PLOrderLevelPromoExceptionReason.InvalidDateFormat:
          case  PLOrderLevelPromoExceptionReason.InvalidDateRange:
            result = RequestSuccessCode.FailedInvalidDateSpecification;
            break;
          case  PLOrderLevelPromoExceptionReason.ImproperRequestFormat:
            result = RequestSuccessCode.FailedInvalidRequestFormat;
            break;
          case PLOrderLevelPromoExceptionReason.InvalidCurrencySpecification:
            result = RequestSuccessCode.FailedInvalidCurrencySpecification;
            break;
          case PLOrderLevelPromoExceptionReason.InvalidOrUnspecifiedAward:
            result = RequestSuccessCode.FailedInvalidAwardSpecification;
            break;
          case PLOrderLevelPromoExceptionReason.InvalidPromoGeneric:
            result = RequestSuccessCode.FailedInvalidPromoIdOrVersion;
            break;
          case PLOrderLevelPromoExceptionReason.Unknown:
          case  PLOrderLevelPromoExceptionReason.Other:
            result = RequestSuccessCode.Failed;
            break;
        }
      }

      return result;
    }
  }
}
