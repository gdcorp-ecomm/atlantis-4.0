using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PaymentAlternateAddUpdate.Interface
{
  public class PaymentAlternateAddUpdateResponseData : IResponseData
  {
    private AtlantisException _atlException;

    private bool _success = false;

    public PaymentAlternateAddUpdateResponseData()
    {
      _success = true;
    }

    public PaymentAlternateAddUpdateResponseData(RequestData oRequestData, Exception ex)
    {
      _atlException = new AtlantisException(oRequestData, "PaymentAlternateAddUpdateResponseData", ex.Message, string.Empty);
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
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
