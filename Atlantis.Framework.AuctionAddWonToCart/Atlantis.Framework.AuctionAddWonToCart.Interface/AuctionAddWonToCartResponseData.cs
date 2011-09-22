using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionAddWonToCart.Interface {
  public class AuctionAddWonToCartResponseData :IResponseData {
    private string _responseXml = string.Empty;
    private AtlantisException _ex = null;


    public bool IsSuccess { get; private set; }

    public AuctionAddWonToCartResponseData(RequestData requestData, Exception exception)
    {
      IsSuccess = false;
      _ex = new AtlantisException(requestData, exception.Source, exception.Message, string.Empty, exception);
    }

    public AuctionAddWonToCartResponseData(string responseData)
    {
      _responseXml =  string.Format("<response>{0}</response>", responseData);
      IsSuccess = false;

      if (!string.IsNullOrEmpty(responseData))
      {
        int responseVal;
        int.TryParse(responseData, out responseVal);

        IsSuccess = (responseVal == 1);
      }
      
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }
  
}
