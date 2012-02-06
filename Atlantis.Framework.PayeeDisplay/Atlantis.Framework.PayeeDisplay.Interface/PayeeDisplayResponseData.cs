using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PayeeDisplay.Interface
{
  public class PayeeDisplayResponseData :IResponseData
  {
    private AtlantisException _atlException;

    public bool IsSuccess { get; private set; }
    private bool _payeeDisplay = false;

    public PayeeDisplayResponseData(bool showDisplay)
    {
      _payeeDisplay = showDisplay;
      IsSuccess = true;
    }

    public PayeeDisplayResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      _atlException = new AtlantisException(oRequestData, "PayeeDisplayResponseData", ex.Message, string.Empty);
    }

    public bool PayeeDisplay
    {
      get
      {
        return _payeeDisplay;
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
