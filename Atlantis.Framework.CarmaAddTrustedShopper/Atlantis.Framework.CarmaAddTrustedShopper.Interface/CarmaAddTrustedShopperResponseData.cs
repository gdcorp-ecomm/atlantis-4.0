using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaAddTrustedShopper.Interface
{
  public class CarmaAddTrustedShopperResponseData : IResponseData
  {
    private AtlantisException _exception = null;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public CarmaAddTrustedShopperResponseData()
    { }

     public CarmaAddTrustedShopperResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public CarmaAddTrustedShopperResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "CarmaAddTrustedShopperResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in CarmaAddTrustedShopperResponseData");
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion
  }
}
