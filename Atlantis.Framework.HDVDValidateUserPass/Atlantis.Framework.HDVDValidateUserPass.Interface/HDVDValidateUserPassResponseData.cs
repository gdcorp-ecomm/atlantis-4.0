using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDValidateUserPass.Interface
{
  [DataContract]
  public class HDVDValidateUserPassResponseData : IResponseData
  {
    private readonly AtlantisException _ex;
    private AriesValidationResponse response;

    public HDVDValidateUserPassResponseData(RequestData request, Exception ex)
    {
      _ex = new AtlantisException(request, ex.Source, ex.Message, ex.StackTrace, ex);
    }

    public HDVDValidateUserPassResponseData(AtlantisException aex)
    {
      _ex = aex;
    }

    public HDVDValidateUserPassResponseData(AriesValidationResponse response)
    {
      this.response = response;
    }

    private bool _hasErrors = false;
    [DataMember]
    public bool HasErrors
    {
      get
      {
        if (this.response != null)
        {
          _hasErrors = this.response.HasErrors;
        }
        return _hasErrors;
      } 

      internal set { _hasErrors = value; }
    }

    private List<string> _errors = new List<string>();
    [DataMember]
    public List<string> Errors
    {
      get
      {
        if (this.response != null && this.response.HasErrors)
        {
          _errors = response.Errors.ToList();
        }
        return _errors;
      }

      internal set { _errors = value; }
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
