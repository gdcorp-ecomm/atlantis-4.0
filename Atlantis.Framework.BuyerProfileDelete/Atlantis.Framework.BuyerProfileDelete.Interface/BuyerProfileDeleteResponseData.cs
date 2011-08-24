using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyerProfileDelete.Interface
{
  public class BuyerProfileDeleteResponseData :IResponseData
  {

    private AtlantisException _atlException = null;
    private bool _success = false;

    public BuyerProfileDeleteResponseData()
    {
      _success = true;
    }

    public BuyerProfileDeleteResponseData(RequestData oRequestData, Exception ex)
    {
      _success = false;
      _atlException = new AtlantisException(oRequestData, "BuyerProfileDeleteResponseData", ex.Message, string.Empty);
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
