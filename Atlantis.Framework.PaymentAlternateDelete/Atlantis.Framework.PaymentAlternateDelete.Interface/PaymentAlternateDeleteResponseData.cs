using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PaymentAlternateDelete.Interface
{
  public class PaymentAlternateDeleteResponseData: IResponseData
  {
    private AtlantisException _atlException;

    private bool _success = false;

    public PaymentAlternateDeleteResponseData()
    {
      _success = true;
    }

    public PaymentAlternateDeleteResponseData(RequestData oRequestData, Exception ex)
    {
      _atlException = new AtlantisException(oRequestData, "PaymentAlternateDeleteResponseData", ex.Message, string.Empty);
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
