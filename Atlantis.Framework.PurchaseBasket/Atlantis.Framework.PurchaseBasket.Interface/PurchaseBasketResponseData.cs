using System;
using Atlantis.Framework.Interface;
namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public class PurchaseBasketResponseData : IResponseData
  {
    private string _responseXml;
    private AtlantisException _exception = null;

    public PurchaseBasketResponseData(string sResponseXML)
    {
      _responseXml = sResponseXML;
    }

    public PurchaseBasketResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      _responseXml = sResponseXML;
      _exception = exAtlantis;
    }

    public PurchaseBasketResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      _responseXml = sResponseXML;
      _exception = new AtlantisException(oRequestData, oRequestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
    }

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
