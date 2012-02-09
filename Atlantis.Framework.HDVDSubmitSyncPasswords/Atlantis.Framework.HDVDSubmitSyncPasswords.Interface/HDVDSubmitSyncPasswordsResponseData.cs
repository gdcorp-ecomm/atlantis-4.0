using System;
using System.Runtime.Serialization;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDSubmitSyncPasswords.Interface
{
  [DataContract]
  public class HDVDSubmitSyncPasswordsResponseData : IResponseData
  {
    private readonly AtlantisException _ex;
    private AriesHostingResponse response;

    public HDVDSubmitSyncPasswordsResponseData(RequestData request, Exception ex)
    {
      _ex = new AtlantisException(request, ex.Source, ex.Message, ex.StackTrace, ex);
    }

    public HDVDSubmitSyncPasswordsResponseData(AtlantisException aex)
    {
      _ex = aex;
    }

    public HDVDSubmitSyncPasswordsResponseData(AriesHostingResponse response)
    {
      this.response = response;
    }

    private bool _isSuccess = false;
    [DataMember]
    public bool IsSuccess
    {
      get
      {
        
        if (this.response != null)
        {
          _isSuccess = (this.response.StatusCode == 0);
        }
        return _isSuccess;
      }
      internal set { _isSuccess = value; }
    }

    private string _status = string.Empty;
    [DataMember]
    public string Status
    {
      get
      {
        
        if (this.response != null)
        {
          _status = this.response.Status;
        }
        return _status;
      }
      internal set { _status = value; }
    }

    private string _message = string.Empty;
    [DataMember]
    public string Message
    {
      get
      {
        if (this.response != null)
        {
          _message = this.response.Message;
        }
        return _message;
      }
      internal set { _message = value; }
    }

    private int _statusCode = -1;
    [DataMember]
    public int StatusCode
    {
      get
      {
        if (this.response != null)
        {
          _statusCode = this.response.StatusCode;
        }
        return _statusCode;
      }
      internal set { _statusCode = value; }
    }
    
    #region Implementation of IResponseData

    public string ToXML()
    {
      string xml;
      try
      {
        var serializer = new DataContractSerializer(this.GetType());
        using (var backing = new System.IO.StringWriter())
        using (var writer = new System.Xml.XmlTextWriter(backing))
        {
          serializer.WriteObject(writer, this);
          xml = backing.ToString();
        }
      }
      catch (Exception exception)
      {
        xml = string.Empty;
      }
      return xml;
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }
}
