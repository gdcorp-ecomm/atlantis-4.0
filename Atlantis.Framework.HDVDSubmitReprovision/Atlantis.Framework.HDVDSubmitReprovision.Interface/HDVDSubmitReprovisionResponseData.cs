using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDSubmitReprovision.Interface
{
  public class HDVDSubmitReprovisionResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;

    private HDVD.Interface.Aries.AriesHostingResponse response;

    public bool IsSuccess
    {
      get
      {
        bool bSuccess = false;
        if (response != null)
        {
          bSuccess = (response.StatusCode == 0);
        }
        return bSuccess;
      }
    }

    public HDVDSubmitReprovisionResponseData(string xml)
    {

    }

     public HDVDSubmitReprovisionResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public HDVDSubmitReprovisionResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HDVDSubmitReprovisionResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    public HDVDSubmitReprovisionResponseData(HDVD.Interface.Aries.AriesHostingResponse response)
    {
      // TODO: Complete member initialization
      this.response = response;
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
