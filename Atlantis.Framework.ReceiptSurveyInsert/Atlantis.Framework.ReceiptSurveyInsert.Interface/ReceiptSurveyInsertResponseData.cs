using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ReceiptSurveyInsert.Interface
{
  public class ReceiptSurveyInsertResponseData : IResponseData
  {
    private AtlantisException _aex;

    public bool IsSuccess { get; private set; }

    #region Constructors
    public ReceiptSurveyInsertResponseData(bool isSuccess)
    {
      IsSuccess = isSuccess;
    }

    public ReceiptSurveyInsertResponseData(RequestData requestData, Exception ex)
    {
      _aex = new AtlantisException(requestData, "ReceiptSurveyInsertResponseData", ex.Message, ex.StackTrace);
      IsSuccess = false;
    }

    public ReceiptSurveyInsertResponseData(AtlantisException aex)
    {
      _aex = aex;
      IsSuccess = false;
    }
    #endregion

    #region Interface Methods

    public string ToXML()
    {
      string xml = string.Concat("<success>", IsSuccess.ToString(), "</success>");
      return xml;
    }

    public AtlantisException GetException()
    {
      return _aex;
    }

    #endregion
  }
}
