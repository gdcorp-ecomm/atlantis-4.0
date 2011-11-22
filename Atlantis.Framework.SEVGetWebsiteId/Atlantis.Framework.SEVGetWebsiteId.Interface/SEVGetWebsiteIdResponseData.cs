using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SEVGetWebsiteId.Interface
{
  public class SEVGetWebsiteIdResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public Dictionary<int, SEVReplacementData> ReplacementDataDictionary { get; private set; }

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public SEVGetWebsiteIdResponseData(Dictionary<int, SEVReplacementData> replacementData)
    {
      ReplacementDataDictionary = replacementData;
    }

     public SEVGetWebsiteIdResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public SEVGetWebsiteIdResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "SEVGetWebsiteIdResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in SEVGetWebsiteIdResponseData");
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
