using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonzaiRemoveAddOn.Interface
{
  public class BonzaiRemoveAddOnResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly string _resultXml = string.Empty;

    public bool IsSuccess { get; private set; }

    public BonzaiRemoveAddOnResponseData()
    {
      IsSuccess = true;
    }

     public BonzaiRemoveAddOnResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public BonzaiRemoveAddOnResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                         "BonzaiRemoveAddOnResponseData",
                                         exception.Message,
                                         requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
