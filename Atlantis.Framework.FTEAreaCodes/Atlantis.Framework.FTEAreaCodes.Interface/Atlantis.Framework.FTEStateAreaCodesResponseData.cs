using System;
using Atlantis.Framework.Interface;
using System.Collections;

namespace Atlantis.Framework.FTEAreaCodes.Interface
{
  public class FTEStateAreaCodesResponseData : IResponseData
  {
    #region Properties

    private AtlantisException _exception = null;
    private ArrayList _areaCodes;

    #endregion

    public FTEStateAreaCodesResponseData(ArrayList areaCodes)
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

    public ArrayList AreaCodes
    {
      get { return this._areaCodes; }
    }

    #endregion Public Methods
  }
}