using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetEmailUsage.Interface
{
  [DataContract]
  public class HDVDGetEmailUsageResponseData : IResponseData
  {

        private readonly AtlantisException _ex;
    private AriesEmailUsageResponse response;

    public HDVDGetEmailUsageResponseData(RequestData request, Exception ex)
    {
      _ex = new AtlantisException(request, ex.Source, ex.Message, ex.StackTrace, ex);
    }

    public HDVDGetEmailUsageResponseData(AtlantisException aex)
    {
      _ex = aex;
    }

    public HDVDGetEmailUsageResponseData(AriesEmailUsageResponse response)
    {
      this.response = response;
    }

    private List<AriesEmailRecord> _previousMonthUsage = new List<AriesEmailRecord>(1);
    [DataMember]
    public List<AriesEmailRecord> PreviousMonthUsage
    {
      get
      {
        if (this.response != null && this.response.PreviousMonthUsage != null)
        {
          _previousMonthUsage = this.response.PreviousMonthUsage.ToList();
        }
        return _previousMonthUsage;
      }
      internal set { _previousMonthUsage = value; }
    }

    private List<AriesEmailRecord> _currentMonthUsage = new List<AriesEmailRecord>(1);
    [DataMember]
    public List<AriesEmailRecord> CurrentMonthUsage
    {
      get
      {
        if (this.response != null && this.response.CurrentMonthUsage != null)
        {
          _currentMonthUsage = this.response.CurrentMonthUsage.ToList();
        }
        return _currentMonthUsage;
      }
      internal set { _currentMonthUsage = value; }
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
