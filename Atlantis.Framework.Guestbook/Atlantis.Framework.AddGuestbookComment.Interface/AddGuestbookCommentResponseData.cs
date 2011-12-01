using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddGuestbookComment.Interface
{
  public class AddGuestbookCommentResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _result;

    public AddGuestbookCommentResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public AddGuestbookCommentResponseData(string result)
    {
      _result = result;
    }

    public string Result
    {
      get { return _result; }
    }

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
