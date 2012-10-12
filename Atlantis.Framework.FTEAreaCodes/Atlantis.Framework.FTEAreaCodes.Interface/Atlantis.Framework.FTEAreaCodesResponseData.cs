using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FTEAreaCodes.Interface
{
  public class FTEAreaCodesResponseData : IResponseData
  {
    #region Properties

    private AtlantisException _exception = null;
    private Dictionary<string, string> _dictionary;

    #endregion

    public FTEAreaCodesResponseData(Dictionary<string, string> dictionary)
    {
      this._dictionary = dictionary;
    }

    public FTEAreaCodesResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public FTEAreaCodesResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData, "FTEAreaCodesResponseData", exception.Message, requestData.ToString());
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

    public Dictionary<string, string> States
    {
      get { return this._dictionary; }
    }

    #endregion Public Methods
  }
}