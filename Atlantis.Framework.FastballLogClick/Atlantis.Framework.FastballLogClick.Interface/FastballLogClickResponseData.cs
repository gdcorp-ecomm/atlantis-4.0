using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballLogClick.Interface
{
  public class FastballLogClickResponseData : IResponseData
  {
    private AtlantisException _exception = null;

    public FastballLogClickResponseData()
    {
    }

    public FastballLogClickResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(
        requestData, "Atlantis.Framework.FastballLogClick", ex.Message, ex.StackTrace, ex);
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
