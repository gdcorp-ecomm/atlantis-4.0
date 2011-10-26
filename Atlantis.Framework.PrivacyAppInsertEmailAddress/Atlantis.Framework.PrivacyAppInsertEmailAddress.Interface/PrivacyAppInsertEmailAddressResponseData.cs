using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PrivacyAppInsertEmailAddress.Interface
{
  public class PrivacyAppInsertEmailAddressResponseData : IResponseData
  {
    private string _errorMessage = string.Empty;
    private string _emailHash = string.Empty;
    private int _result = 0;
    private AtlantisException _ex;

    public PrivacyAppInsertEmailAddressResponseData(string emailHash, string errorMessage)
    {
      if(string.IsNullOrEmpty(errorMessage))
      {
        _emailHash = emailHash;
        _result = 1;
      }

      _errorMessage = errorMessage;
    }

    public int Result 
    {
      get { return _result; }
    }

    public string ErrorValue
    {
      get { return _errorMessage; }
    }

    public string EmailHash
    {
      get { return _emailHash; }
    }

    public PrivacyAppInsertEmailAddressResponseData(string emailEmailHash, string errorMessage, AtlantisException ex)
    {
      _emailHash = emailEmailHash;
      _errorMessage = errorMessage;
      _result = 0;
      _ex = ex;
    }

    public PrivacyAppInsertEmailAddressResponseData(string emailEmailHash, string errorMessage, RequestData oRequestData, Exception ex)
    {
      _emailHash = emailEmailHash;
      _errorMessage = errorMessage;
      _result = 0;
      _ex = new AtlantisException(oRequestData, "PrivacyAppInsertEmailAddressResponseData", ex.Message, oRequestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }
    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion

  }
}
