using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetContextHelp.Interface
{
  [DataContract]
  public class HDVDGetContextHelpResponseData : IResponseData
  {
    private readonly AtlantisException _ex;
    private AriesContextHelpResponse response;

    public HDVDGetContextHelpResponseData(RequestData request, Exception ex)
    {
      _ex = new AtlantisException(request, ex.Source, ex.Message, ex.StackTrace, ex);
    }

    public HDVDGetContextHelpResponseData(AtlantisException aex)
    {
      _ex = aex;
    }

    public HDVDGetContextHelpResponseData(AriesContextHelpResponse response)
    {
      this.response = response;
    }

    private string _title = string.Empty;
    [DataMember]
    public string Title
    {
      get
      {
        if (this.response != null && this.response.Title != null)
        {
          _title = this.response.Title;
        }
        return _title;
      }
      internal set { _title = value; }
    }

    private string _description = string.Empty;
    [DataMember]
    public string Description
    {
      get
      {
        if (this.response != null && this.response.Description != null)
        {
          _description = this.response.Description;
        }
        return _description;
      }
      internal set { _description = value; }
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
