using System;
using System.Text;
using Atlantis.Framework.HDVD.Interface;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDSubmitRebootRequest.Interface
{
  public class HDVDSubmitRebootResponseData :  IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private AriesHostingResponse _response;

    public AriesHostingResponse Response { get { return _response; } }

    public bool IsSuccess
    {
      get
      {
        return (_response.StatusCode == 0);
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

    public HDVDSubmitRebootResponseData(AriesHostingResponse response)
    {
      _response = response;
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sb = new StringBuilder();

      sb.Append("<HDVDSubmitRebootResponseData>");
      sb.AppendFormat("<Status>{0}</Status>", _response.Status);
      sb.AppendFormat("<StatusCode>{0}</StatusCode>", _response.StatusCode);
      sb.AppendFormat("<Message>{0}</Message>", _response.Message);
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
