using System;
using Atlantis.Framework.Interface;
namespace Atlantis.Framework.BPSubscribeRemove.Interface
{
  public class BPSubscribeRemoveResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private bool _isSuccess = false;
    private string _responseText = string.Empty;
    private bool _emailRemoved = false;

    public BPSubscribeRemoveResponseData(string responseText)
    {
      _responseText = responseText;
      _isSuccess = true;
      _emailRemoved = (string.Compare(_responseText, "removed", true) == 0);
    }

    public BPSubscribeRemoveResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public BPSubscribeRemoveResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "BPSubscribeRemoveResponseData", ex.Message, ex.StackTrace);
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public bool EmailRemoved
    {
      get { return _emailRemoved; }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return "<response>" + _responseText + "</response>";
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
