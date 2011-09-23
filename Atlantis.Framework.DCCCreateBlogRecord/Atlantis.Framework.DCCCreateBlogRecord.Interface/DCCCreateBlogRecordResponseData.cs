using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCCreateBlogRecord.Interface
{
  public class DCCCreateBlogRecordResponseData : IResponseData
  {
    private readonly AtlantisException _exception;

    public DCCCreateBlogRecordResponseData()
    {
      IsSuccess = true;
    }

    public  DCCCreateBlogRecordResponseData(int errorNum)
    {
      IsSuccess = false;
      ErrorNum = errorNum;
    }

    public DCCCreateBlogRecordResponseData(RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      _exception = new AtlantisException(requestData, ex.Source, ex.Message, ex.StackTrace);
    }

    public bool IsSuccess { get; protected set; }

    public int ErrorNum { get; protected set; }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
