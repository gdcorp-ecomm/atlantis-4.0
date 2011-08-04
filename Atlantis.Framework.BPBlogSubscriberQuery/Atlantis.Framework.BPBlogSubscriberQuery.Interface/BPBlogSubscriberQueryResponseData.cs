using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BPBlogSubscriberQuery.Interface
{
  public class BPBlogSubscriberQueryResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private bool _isSuccess = false;
    private string _responseText = string.Empty;
    private bool _emailExists = false;

    public BPBlogSubscriberQueryResponseData(string responseText)
    {
      _responseText = responseText;
      _isSuccess = true;
      _emailExists = (string.Compare(_responseText, "y", true) == 0);
    }

    public BPBlogSubscriberQueryResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public BPBlogSubscriberQueryResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "BPBlogSubscriberQueryResponseData", ex.Message, ex.StackTrace);
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public bool EmailExists
    {
      get { return _emailExists; }
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
