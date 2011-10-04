using System;
using Atlantis.Framework.Interface;
namespace Atlantis.Framework.TransferBasket.Interface
{
  public class TransferBasketResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _responseXml;

    public TransferBasketResponseData(string sResponseXML)
    {
      _responseXml = sResponseXML;
    }

    public TransferBasketResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      _responseXml = sResponseXML;
      _exception = exAtlantis;
    }

    public TransferBasketResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
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
