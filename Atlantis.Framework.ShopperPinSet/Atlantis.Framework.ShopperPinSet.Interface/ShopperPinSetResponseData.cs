using System;

using Atlantis.Framework.Interface;
namespace Atlantis.Framework.ShopperPinSet.Interface
{
  public class ShopperPinSetResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private bool _isPinSet = false;

    private void SetResult(string result)
    {
      if (result == "1")
        _isPinSet = true;
      else
        _isPinSet = false;
    }

    public bool IsPinSet
    {
      get
      {
        return _isPinSet;
      }
    }

    public ShopperPinSetResponseData(string sResponseXML)
    {
      SetResult(sResponseXML);
    }

    public ShopperPinSetResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      SetResult(sResponseXML);
      _exception = exAtlantis;
    }

    public ShopperPinSetResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      SetResult(sResponseXML);
      _exception = new AtlantisException(oRequestData, oRequestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
    }

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return "<IsPinSet>" + IsPinSet.ToString() + "</IsPinSet>";
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
