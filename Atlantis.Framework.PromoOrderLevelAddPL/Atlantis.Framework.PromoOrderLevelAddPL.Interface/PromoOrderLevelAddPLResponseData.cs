using System;
using System.Collections.Generic;
using System.Net;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoOrderLevelAddPL.Interface
{
  public enum RequestSuccessCode
  {
    Successful = 0,
    FailedInvalidPromoDate = 1,
    FailedInvalidRequestFormat = 2,
    FailedInsufficientDataForRequest = 3,
    FailedPromoCodeDoesNotExist = 4,
    FailedUnknown = 5
  }

  public class PromoOrderLevelAddPLResponseData : IResponseData
  {

    private string _responseXml = string.Empty;
    private AtlantisException _atlantisException = null;
    private RequestData _requestData = null;
    private RequestSuccessCode _isSuccess = RequestSuccessCode.Successful;

    public PromoOrderLevelAddPLResponseData(RequestData requestData, string responseXml)
    {
      this._requestData = requestData;
      this._responseXml = responseXml;
      this._isSuccess = RequestSuccessCode.Successful;
    }

    public PromoOrderLevelAddPLResponseData(RequestData requestData, PrivateLabelPromoException exception)
    {
      this._requestData = requestData;
      this._isSuccess = ParseRequestSuccessCodeFromException(exception);
      this._atlantisException = new AtlantisException(requestData, "ResellerOrderLevelPromo", exception.Message, Enum.GetName(typeof(PrivateLabelPromoExceptionReason), exception.Reason));
    }

    public PromoOrderLevelAddPLResponseData(RequestData requestData, string responseXml, PrivateLabelPromoException exception)
    {
      this._requestData = requestData;
      this._responseXml = responseXml;
      this._isSuccess = ParseRequestSuccessCodeFromException(exception);
      this._atlantisException = new AtlantisException(requestData, "ResellerOrderLevelPromo", exception.Message, Enum.GetName(typeof(PrivateLabelPromoExceptionReason), exception.Reason));
    }

    public PromoOrderLevelAddPLResponseData(RequestData requestData, string responseXml, AtlantisException exception)
    {
      this._atlantisException = new AtlantisException(requestData, "Atlantis.Framework.ResellerOrderLevelPromoResponseData", exception.Message, responseXml);
      this._isSuccess = RequestSuccessCode.FailedUnknown;
    }

    public PromoOrderLevelAddPLResponseData(RequestData requestData, WebExceptionStatus exception)
    {
      this._isSuccess = RequestSuccessCode.FailedUnknown;
      this._atlantisException = new AtlantisException(requestData, "Atlantis.Framework.ResellerOrderLevelPromoResponseData", Enum.GetName(typeof(WebExceptionStatus), exception), string.Empty);
    }

    public PromoOrderLevelAddPLResponseData(string responseXml, RequestData requestData, Exception exception)
    {
      this._atlantisException = new AtlantisException(requestData, "Atlantis.Framework.ResellerOrderLevelPromoResponseData", exception.Message, responseXml);
      this._isSuccess = RequestSuccessCode.FailedUnknown;
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

    private static RequestSuccessCode ParseRequestSuccessCodeFromException(PrivateLabelPromoException exception)
    {
      RequestSuccessCode result = RequestSuccessCode.Successful;
      if (exception != null)
      {
        switch (exception.Reason)
        {
          case PrivateLabelPromoExceptionReason.InvalidDateFormat:
          case PrivateLabelPromoExceptionReason.InvalidDateRange:
            result = RequestSuccessCode.FailedInvalidPromoDate;
            break;
          case PrivateLabelPromoExceptionReason.InvalidRequestFormat:
            result = RequestSuccessCode.FailedInvalidRequestFormat;
            break;
          case PrivateLabelPromoExceptionReason.RequiredDataMissing:
            result = RequestSuccessCode.FailedInsufficientDataForRequest;
            break;
          case PrivateLabelPromoExceptionReason.Unknown:
            result = RequestSuccessCode.FailedUnknown;
            break;
        }
      }

      return result;
    }
  }
}
