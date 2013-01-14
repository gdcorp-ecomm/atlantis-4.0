using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PaymentProfileLastChargeDate.Interface
{
  public class PaymentProfileLastChargeDateResponseData : IResponseData
  {

    private AtlantisException _atlException;
    private bool _hasDate = false;
    private DateTime _chargedDate;
    public bool IsSuccess { get; private set; }
 

    public PaymentProfileLastChargeDateResponseData(bool hasDate, DateTime chargedDate)
    {
      _hasDate = hasDate;
      _chargedDate = chargedDate;
      IsSuccess = true;
    }

    public PaymentProfileLastChargeDateResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      _atlException = new AtlantisException(oRequestData, "PaymentProfileLastChargeDateResponseData", ex.Message, string.Empty);
    }

    public bool HasDate
    {
      get { return _hasDate; }
    }

    public DateTime LastChargedDate
    {
      get { return _chargedDate; }
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _atlException;
    }

    #endregion

  }
}
