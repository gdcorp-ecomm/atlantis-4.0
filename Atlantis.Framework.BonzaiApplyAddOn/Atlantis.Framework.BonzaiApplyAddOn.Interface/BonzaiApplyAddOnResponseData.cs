using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonzaiApplyAddOn.Interface
{
  public class BonzaiApplyAddOnResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly string _resultXml = string.Empty;

    public bool IsSuccess { get; private set; }

    public BonzaiApplyAddOnResponseData()
    {
      IsSuccess = true;
    }

     public BonzaiApplyAddOnResponseData(AtlantisException atlantisException)
     {
       IsSuccess = false;
       _exception = atlantisException;
     }

    public BonzaiApplyAddOnResponseData(RequestData requestData, Exception exception)
    {
      IsSuccess = false;
      _exception = new AtlantisException(requestData,
                                         "BonzaiApplyAddOnResponseData",
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
