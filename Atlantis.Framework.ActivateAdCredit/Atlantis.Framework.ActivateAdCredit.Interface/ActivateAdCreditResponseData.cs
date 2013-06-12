using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ActivateAdCredit.Interface
{
  public class ActivateAdCreditResponseData : IResponseData
  {
    
    #region Properties

    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }
    #endregion

    public ActivateAdCreditResponseData(string xml)
    {
      _resultXML = xml;

    }

    public ActivateAdCreditResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "ActivateAdCreditResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    public bool IsCouponOutOfStock
    {
      get { return _resultXML.Contains("OutOfInventory"); }
    }

    public bool IsErrorXml
    {
      get { return _resultXML.Contains("<Error>"); }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
