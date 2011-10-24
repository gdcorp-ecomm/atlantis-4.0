using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GiftCardBalance.Interface
{
  public class GiftCardBalanceResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;
    private int _accountBalance = 0;

    public int AccountBalance
    {
      get
      {
        return _accountBalance;
      }
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public GiftCardBalanceResponseData(int giftCardBalance)
    {
      _accountBalance = giftCardBalance;
      _success = true;
    }

    public GiftCardBalanceResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public GiftCardBalanceResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "GiftCardBalanceResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("INFO");
      xtwRequest.WriteAttributeString("AcctBalance", System.Convert.ToString(_accountBalance));
      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
