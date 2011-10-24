using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballLogOrder.Interface
{
  public class FastballLogOrderResponseData : IResponseData
  {
    private AtlantisException _exception;
    private bool _isSuccess;

    #region Constructors

    public FastballLogOrderResponseData(bool success)
    {
      _isSuccess = success;
    }

    public FastballLogOrderResponseData(bool success, AtlantisException exAtlantis)
    {
      _isSuccess = success;
      _exception = exAtlantis;
    }

    public FastballLogOrderResponseData(bool success, RequestData oRequestData, Exception ex)
    {
      _isSuccess = success;
      _exception = new AtlantisException(oRequestData, oRequestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
    }

    #endregion

    public bool IsSuccess
    {
      get
      {
        return _isSuccess;
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
