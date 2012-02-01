using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetSupportedOS.Interface
{
  [DataContract]
  public class HDVDGetSupportedOSResponseData : IResponseData
  {

    private const string STATUS_SUCCESS = "success";

    private AtlantisException _aex = null;

    [DataMember]
    public IList<string> SupportedOperatingSystems = new List<string>();

    [DataMember]
    public bool IsSuccess { get; private set; }

    [DataMember]
    public int StatusCode { get; private set; }

    [DataMember]
    public string StatusMessage { get; private set; }

    public HDVDGetSupportedOSResponseData(string status, int statusCode, string message, IList<string> operatingSystems)
    {
      StatusCode = statusCode;
      StatusMessage = message;
      SupportedOperatingSystems = (operatingSystems ?? new List<string>());
      IsSuccess = (status == STATUS_SUCCESS);
    }

    public HDVDGetSupportedOSResponseData(AtlantisException aex)
    {
      _aex = aex;
      IsSuccess = false;
    }

    public HDVDGetSupportedOSResponseData(RequestData requestData, Exception ex)
    {
      _aex = new AtlantisException(requestData, ex.Source, ex.Message, ex.StackTrace, ex);
      IsSuccess = false;
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      string xml = string.Empty;
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
      catch (Exception ex)
      {
        xml = string.Empty;
      }
      return xml;
    }

    public AtlantisException GetException()
    {
      return _aex;
    }

    #endregion
  }
}
