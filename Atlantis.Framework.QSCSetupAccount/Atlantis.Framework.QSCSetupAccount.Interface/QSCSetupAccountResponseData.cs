using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCSetupAccount.Interface
{
  public class QSCSetupAccountResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public QSCSetupAccountResponseData(int resultCode,string responseMessage)
    {
      switch (resultCode)
      {
        case 1:
          _success = true;
          break;
        case 0:
          _success = true;
          break;
        case -1:
          this._exception = new AtlantisException("Setup QSC Account","0","Orion GUID is invalid",responseMessage,null,null);
          break;
        case -2:
          this._exception = new AtlantisException("Setup QSC Account", "0", "Domain name is invalid", responseMessage, null, null);
          break;
        case -3:
          this._exception = new AtlantisException("Setup QSC Account", "0", "Email is invalid", responseMessage, null, null);
          break;
        case -4:
          this._exception = new AtlantisException("Setup QSC Account", "0", "Company name is invalid", responseMessage, null, null);
          break;
        case -5:
          this._exception = new AtlantisException("Setup QSC Account", "0", "Themeid is invalid", responseMessage, null, null);
          break;
        case -100:
          this._exception = new AtlantisException("Setup QSC Account", "0", "Technical difficulties - please retry", responseMessage, null, null);
          break;
        case -101:
          this._exception = new AtlantisException("Setup QSC Account", "0", "Account is already setup", responseMessage, null, null);
          break;
        case -102:
          this._exception = new AtlantisException("Setup QSC Account", "0", "Account is alredy being setup", responseMessage, null, null);
          break;
        case -103:
          this._exception = new AtlantisException("Setup QSC Account", "0", "Domain name is alredy in use", responseMessage, null, null);
          break;
        case -104:
          this._exception = new AtlantisException("Setup QSC Account", "0", "Technical difficulties - next try again", responseMessage, null, null);
          break;
      }
    }

     public QSCSetupAccountResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public QSCSetupAccountResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "QSCSetupAccountResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
