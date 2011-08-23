using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaRemoveTrustedShopper.Interface
{
  public class CarmaRemoveTrustedShopperResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public CarmaRemoveTrustedShopperResponseData()
    { }

     public CarmaRemoveTrustedShopperResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public CarmaRemoveTrustedShopperResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "CarmaRemoveTrustedShopperResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in CarmaRemoveTrustedShopperResponseData");
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion
  }
}
