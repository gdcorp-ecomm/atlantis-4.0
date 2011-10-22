using System;

using Atlantis.Framework.Interface;
namespace Atlantis.Framework.ClearBasket.Interface
{
  public class ClearBasketResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _responseXml;

    public ClearBasketResponseData(string sResponseXML)
    {
      _responseXml = sResponseXML;
    }

    public ClearBasketResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      _responseXml = sResponseXML;
      _exception = exAtlantis;
    }

    public ClearBasketResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      _responseXml = sResponseXML;
      _exception = new AtlantisException(oRequestData, oRequestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
    }

    public bool IsSuccess
    {
      get 
      { 
        return _responseXml.IndexOf("success", StringComparison.OrdinalIgnoreCase) > -1; 
      }
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
