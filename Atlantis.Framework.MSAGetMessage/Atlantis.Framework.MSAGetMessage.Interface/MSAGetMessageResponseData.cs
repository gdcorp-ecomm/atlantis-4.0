using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MSA.Interface;

namespace Atlantis.Framework.MSAGetMessage.Interface
{
  public class MSAGetMessageResponseData: IResponseData
  {    
    AtlantisException _exception;
    bool _success = true;

    public Message Message { get; set; }

    public MSAGetMessageResponseData()
    {

    }

    public MSAGetMessageResponseData(GetMessageResponse response)
    {
      Message = response.Message;
    }  

    public MSAGetMessageResponseData(AtlantisException ex)
    {
      _exception = ex;
      _success = false;
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sb = new StringBuilder("");
      return sb.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

  }
    #endregion
}
