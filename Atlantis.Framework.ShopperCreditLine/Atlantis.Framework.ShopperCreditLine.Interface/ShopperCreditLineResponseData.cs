using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperCreditLine.Interface
{
  public class ShopperCreditLineResponseData : IResponseData
  {
    private string _responseXml;
    private AtlantisException _exception = null;
    private bool _hasLineOfCredit = false;

    public ShopperCreditLineResponseData(string sResponseXML, bool hasLineOfCredit)
    {
      _responseXml = sResponseXML;
      _hasLineOfCredit = hasLineOfCredit;
    }

    public ShopperCreditLineResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      _responseXml = sResponseXML;
      _exception = exAtlantis;
    }

    public ShopperCreditLineResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      _responseXml = sResponseXML;
      _exception = new AtlantisException(oRequestData, oRequestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
    }

    public bool HasLineOfCredit
    {
      get { return _hasLineOfCredit; }
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
