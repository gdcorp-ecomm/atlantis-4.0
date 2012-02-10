using System;
using System.Runtime.Serialization;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDUpdFTPBackupInfo.Interface
{
  public class HDVDUpdFTPBackupInfoResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    private AriesHostingResponse _response = null;
    public AriesHostingResponse Response
    {
      get { return _response; }
      internal set { _response = value; }
    }

    public bool IsSuccess
    {
      get { 
        bool bSuccess = false;
        if (Response != null)
        {
          bSuccess = (Response.StatusCode == 0);
        }
        return bSuccess;
      }
    }

    
     public HDVDUpdFTPBackupInfoResponseData(AriesHostingResponse response)
     {
       _response = response;
     }

    public HDVDUpdFTPBackupInfoResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public HDVDUpdFTPBackupInfoResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HDVDUpdFTPBackupInfoResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      string xml = string.Empty;
      try
      {
        var serializer = new DataContractSerializer(this.Response.GetType());
        using (var backing = new System.IO.StringWriter())
        using (var writer = new System.Xml.XmlTextWriter(backing))
        {
          serializer.WriteObject(writer, this.Response);
          xml = backing.ToString();
        }
      }
      catch (Exception ex)
      {
        xml = string.Empty;
      }
      return xml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
