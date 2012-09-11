using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.EcommInstorePending.Interface
{
  public class EcommInstorePendingResponseData : IResponseData
  {
    AtlantisException _exception = null;

    public InstorePendingResult Result { get; set; }
    public int ResultCode { get; set; }
    public int Amount { get; set; }
    public string TransactionalCurrencyType { get; set; }
    public string ResultMessage { get; set; }

    private void SetResult()
    {
      Result = InstorePendingResult.UnknownResult;
      if (Enum.IsDefined(typeof(InstorePendingResult), ResultCode))
      {
        Result = (InstorePendingResult)ResultCode;
      }
    }

    public EcommInstorePendingResponseData(int resultCode, string resultMessage, int amount, string transactionalCurrencyType)
    {
      Amount = amount;
      ResultCode = resultCode;
      ResultMessage = resultMessage;
      if (!string.IsNullOrEmpty(transactionalCurrencyType))
      {
        TransactionalCurrencyType = transactionalCurrencyType;
      }
      else
      {
        TransactionalCurrencyType = "USD";
      }

      SetResult();
    }

    public EcommInstorePendingResponseData(RequestData requestData, Exception ex)
    {
      Amount = 0;
      TransactionalCurrencyType = "USD";
      ResultCode = -1;
      ResultMessage = string.Empty;
      SetResult();
      _exception = new AtlantisException(requestData, "EcommInstorePendingResponseData.ctor", ex.Message + ex.StackTrace, requestData.ToXML(), ex);
    }

    public string ToXML()
    {
      XElement xml = new XElement("EcommInstorePendingResponseData",
        new XAttribute("resultcode", ResultCode),
        new XAttribute("amount", Amount),
        new XAttribute("transactionalcurrencytype", TransactionalCurrencyType));
      return xml.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
