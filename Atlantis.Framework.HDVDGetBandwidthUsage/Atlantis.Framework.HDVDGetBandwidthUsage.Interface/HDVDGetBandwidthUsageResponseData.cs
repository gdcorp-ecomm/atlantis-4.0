using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetBandwidthUsage.Interface
{
  public class HDVDGetBandwidthUsageResponseData : IResponseData
  {
    private AtlantisException _exception = null;

    public AriesBandwidthUsageResponse Response { get; private set; }

    public bool IsSuccess
    {
      get
      {
        bool bSuccess = false;
        if (Response != null)
        {
          bSuccess = (Response.StatusCode == 0);
        }
        return bSuccess;
      }
    }
    
    public HDVDGetBandwidthUsageResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public HDVDGetBandwidthUsageResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HDVDGetBandwidthUsageResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    public HDVDGetBandwidthUsageResponseData(AriesBandwidthUsageResponse response)
    {
      Response = response;
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
