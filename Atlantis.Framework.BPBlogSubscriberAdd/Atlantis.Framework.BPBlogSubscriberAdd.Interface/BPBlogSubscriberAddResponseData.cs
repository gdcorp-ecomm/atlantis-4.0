using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BPBlogSubscriberAdd.Interface
{
  public class BPBlogSubscriberAddResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private bool _isSuccess = false;
    private string _responseText = string.Empty;
    
    public BPBlogSubscriberAddResponseData(string responseText)
    {
      _responseText = responseText;
      _isSuccess = (string.Compare(_responseText, "y", true) == 0);
    }

    public BPBlogSubscriberAddResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public BPBlogSubscriberAddResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "BPBlogSubscriberAddResponseData", ex.Message, ex.StackTrace);
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
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
