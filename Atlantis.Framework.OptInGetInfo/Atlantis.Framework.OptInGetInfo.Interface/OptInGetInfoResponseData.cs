using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OptInGetInfo.Interface
{
  public class OptInGetInfoResponseData : IResponseData 
  {
    private AtlantisException _exception;
    private bool _isSuccess;
    private  List<OptIn.Interface.OptIn> _optIns;

    public OptInGetInfoResponseData(OptInGetInfoRequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "OptInGetInfoResponseData", ex.Message, ex.StackTrace);
    }

    public OptInGetInfoResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public OptInGetInfoResponseData(List<OptIn.Interface.OptIn> optIns)
    {
      _optIns = optIns;
      _isSuccess = true;
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      var sbOut = new StringBuilder();

      sbOut.Append("<OptInGetInfoResponseData>");
      sbOut.Append("<OptIns>");
      foreach (var item in _optIns)
      {
        sbOut.AppendFormat("<OptIn Type=\"{0}\" Description=\"{1}\" OptInId=\"{2}\" Status=\"{3}\" />", item.Type, item.OptInDescription, item.OptInId, item.Status);
      }
      sbOut.Append("<OptIns>");
      sbOut.Append("</OptInGetInfoResponseData>");

      return sbOut.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }
    #endregion

    public List<OptIn.Interface.OptIn> OptIns
    {
      get { return _optIns; }
    }
  }
}
