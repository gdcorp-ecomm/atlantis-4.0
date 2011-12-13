using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ReceiptSurvey.Interface
{
  public class ReceiptSurveyResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;

    public bool IsSuccess
    {
      get;
      private set;
    }

    #region Constructors

    public ReceiptSurveyResponseData()
    {
      IsSuccess = true;
    }

     public ReceiptSurveyResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public ReceiptSurveyResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "ReceiptSurveyResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #endregion

    #region IResponseData Members

    public string ToXML()
    {
      return string.Concat("<Result>", IsSuccess, "</Result>");
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
