using System;
using System.Text;
using Atlantis.Framework.HDVD.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDSubmitRebootRequest.Interface
{
  public class HDVDSubmitRebootResponseData : HDVDHostingResponse, IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
   
    public bool IsSuccess
    {
      get
      {
        return (StatusCode == 0);
      }
    }

    public HDVDSubmitRebootResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public HDVDSubmitRebootResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HDVDSubmitRebootResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    public HDVDSubmitRebootResponseData(string status, string message, int statusCode)
    {
      Status = status;
      Message = message;
      StatusCode = statusCode;
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sb = new StringBuilder();

      sb.Append("<HDVDSubmitRebootResponseData>");
      sb.AppendFormat("<Status>{0}</Status>", Status);
      sb.AppendFormat("<StatusCode>{0}</StatusCode>", StatusCode);
      sb.AppendFormat("<Message>{0}</Message>", Message);
      sb.Append("</HDVDSubmitRebootResponseData>");

      return sb.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
