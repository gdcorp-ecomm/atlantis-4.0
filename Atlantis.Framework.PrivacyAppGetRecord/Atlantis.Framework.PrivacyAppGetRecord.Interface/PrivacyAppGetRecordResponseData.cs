using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PrivacyAppGetRecord.Interface
{
  public class PrivacyAppGetRecordResponseData : IResponseData
  {
    private string _responseXml = string.Empty;
    private bool _isValid = false;
    private AtlantisException _ex;

    public PrivacyAppGetRecordResponseData(string responseXML)
    {
      _responseXml = responseXML;
      _isValid = true;

    }

    public bool IsValid
    {
      get { return _isValid; }
    }

    public string ResponseXML
    {
      get { return _responseXml; }
    }

    public PrivacyAppGetRecordResponseData(string responseXml, AtlantisException ex)
    {
      _responseXml = responseXml;
      _isValid = false;
      _ex = ex;
    }

    public PrivacyAppGetRecordResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _isValid = false;
      _ex = new AtlantisException(oRequestData, "PrivacyAppGetRecordResponseData", ex.Message, oRequestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXml;
    }
    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }


}
