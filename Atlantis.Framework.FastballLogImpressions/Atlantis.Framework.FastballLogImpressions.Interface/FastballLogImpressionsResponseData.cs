using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballLogImpressions.Interface
{
  public class FastballLogImpressionsResponseData : IResponseData
  {
    private AtlantisException _exception = null;

    public FastballLogImpressionsResponseData()
    {
    }

    public FastballLogImpressionsResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(
        requestData, "Atlantis.Framework.FastballLogImpressions", ex.Message, ex.StackTrace, ex);
      IsSuccess = false;
    }

    public bool IsSuccess { get; set; }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
