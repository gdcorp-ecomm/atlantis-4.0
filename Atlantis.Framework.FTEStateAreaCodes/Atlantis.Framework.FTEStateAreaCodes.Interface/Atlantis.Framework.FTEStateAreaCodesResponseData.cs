using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FTEStateAreaCodes.Interface
{
  public class FTEStateAreaCodesResponseData : IResponseData
  {
    #region Properties

    private AtlantisException _exception = null;
    private List<string> _areaCodes;

    #endregion

    public FTEStateAreaCodesResponseData(List<string> areaCodes)
    {
      this._areaCodes = areaCodes;
    }

    public FTEStateAreaCodesResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public FTEStateAreaCodesResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData, "FTEStateAreaCodesResponseData", exception.Message, requestData.ToString());
    }

    #region Public Methods

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public List<string> AreaCodes
    {
      get { return this._areaCodes; }
    }

    #endregion Public Methods
  }
}
