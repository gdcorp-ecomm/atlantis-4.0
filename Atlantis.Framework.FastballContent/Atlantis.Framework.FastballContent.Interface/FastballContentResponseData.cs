using System;
using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.FastballContent.Interface
{
  public class FastballContentResponseData : IResponseData
  {
    #region Fields

    private readonly AtlantisException _exception = null;
    private string _responseData = string.Empty;
    private List<OfferResponse> _responses;
    private bool _success = false;

    #endregion

    #region Constructors

    public FastballContentResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(
        requestData, "FastballContentResponseData", exception.Message + exception.StackTrace, requestData.ToXML());
    }

    public FastballContentResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public FastballContentResponseData(string responseData)
    {
      _success = true;
      _responseData = responseData;
    }

    #endregion

    #region Public Properties

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public List<OfferResponse> OfferResponses
    {
      get
      {
        if (_responses == null)
        {
          _responses = new List<OfferResponse>();
        }
        return _responses;
      }
    }

    public string ResponseData
    {
      get
      {
        return _responseData;
      }
    }

    #endregion

    #region Public Methods

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return string.Empty;
    }

    #endregion

  }
}
