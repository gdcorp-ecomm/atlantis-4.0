using System;
using System.Runtime.Serialization;
using Atlantis.Framework.HDVD.Interface;
using Atlantis.Framework.HDVD.Interface.Interfaces;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDRequestAddIP.Interface
{
  [DataContract]
  public class HDVDRequestAddIpResponseData : IResponseData
  {
    private const string STATUS_SUCCESS = "success";

    private AtlantisException _aex;

    [DataMember]
    public HDVDHostingResponse Result { get; set; }

    private bool? _isSuccess = false;
    
    [DataMember] 
    public bool IsSuccess { get
    {
      if (!_isSuccess.HasValue)
      {
        _isSuccess = (Result.Status == STATUS_SUCCESS);
      }
      return _isSuccess.Value;
    }
      private set { _isSuccess = value; }
    }

    public HDVDRequestAddIpResponseData(IHDVDHostingResponse hostingResponse)
    {
      Result = hostingResponse as HDVDHostingResponse;
    }

    public HDVDRequestAddIpResponseData(HDVDRequestAddIpRequestData request, Exception ex)
    {
      _aex = new AtlantisException(request, ex.Source, ex.Message, ex.StackTrace, ex);
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
