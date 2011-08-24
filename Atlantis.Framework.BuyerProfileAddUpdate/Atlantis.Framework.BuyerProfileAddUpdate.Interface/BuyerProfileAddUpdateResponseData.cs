using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyerProfileAddUpdate.Interface
{
  public class BuyerProfileAddUpdateResponseData : IResponseData
  {
    private AtlantisException _atlException = null;
    private bool _success = false;

    public BuyerProfileAddUpdateResponseData()
    {
      _success = true;
    }

    public BuyerProfileAddUpdateResponseData(RequestData oRequestData, Exception ex)
    {
      _success = false;
      _atlException = new AtlantisException(oRequestData, "BuyerProfileAddUpdateResponseData", ex.Message, string.Empty);
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    #endregion

    public AtlantisException GetException()
    {
      return _atlException;
    }

  }

}
