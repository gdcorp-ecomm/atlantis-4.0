using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyerProfileSetDefault.Interface
{
  public class BuyerProfileSetDefaultResponseData : IResponseData
  {
    private AtlantisException _atlException = null;
    private bool _success = false;

    public BuyerProfileSetDefaultResponseData(int result)
    {
      _success = (result == 0);
    }

    public BuyerProfileSetDefaultResponseData(RequestData oRequestData, Exception ex)
    {
      _success = false;
      _atlException = new AtlantisException(oRequestData, "BuyerProfileSetDefaultResponseData", ex.Message, string.Empty);
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
