using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;
using Atlantis.Framework.HDVD.Interface;

namespace Atlantis.Framework.HDVDGetAccountSummary.Interface
{
  [DataContract]
  public class HDVDGetAccountSummaryResponseData : IResponseData, ISessionSerializableResponse
  {
    private const string STATUS_SUCCESS = "success";

    private AtlantisException _ex = null;

    [DataMember]
    public bool IsSuccess { get; private set; }

    [DataMember]
    public int ResellerId { get; private set; }

    [DataMember]
    public int StatusCode { get; private set; }

    [DataMember]
    public string StatusMessage { get; private set; }

    [DataMember]
    public HDVDAccountSummaryInfo AccountSummary { get; private set; }

    public HDVDGetAccountSummaryResponseData(RequestData requestData, Exception ex)
    {
      ResellerId = -1;
      StatusCode = -1;
      StatusMessage = string.Empty;
      AccountSummary = null;
      IsSuccess = false;
      _ex = new AtlantisException(requestData, ex.Source + ":HDVDGetAccountSummaryRequest", ex.Message, string.Empty, ex);
    }
    
    public HDVDGetAccountSummaryResponseData(AtlantisException ex)
    {
      ResellerId = -1;
      StatusCode = -1;
      StatusMessage = string.Empty;
      AccountSummary = null;
      IsSuccess = false;
      _ex = ex;
    }

    public HDVDGetAccountSummaryResponseData(string status, int statusCode, string message, HDVDAccountSummaryInfo summary, int resellerId)
    {
      AccountSummary = summary;
      ResellerId = resellerId;
      StatusCode = statusCode;
      StatusMessage = message;
      IsSuccess = (status == STATUS_SUCCESS);
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
      return _ex;
    }

    #endregion

    #region Implementation of ISessionSerializableResponse

    public string SerializeSessionData()
    {
      string sessionString = string.Empty;
      MemoryStream ms = new MemoryStream();
      DataContractJsonSerializer ser;

      try
      {
        ser = new DataContractJsonSerializer(typeof(HDVDAccountSummaryInfo));
        ser.WriteObject(ms, AccountSummary);
        sessionString = Encoding.Default.GetString(ms.ToArray());
        ms.Close();
      }
      finally
      {
        ms.Dispose();
      }

      return sessionString;
    }

    public void DeserializeSessionData(string sessionData)
    {
      MemoryStream ms = null;
      DataContractJsonSerializer ser;

      try
      {
        ms = new MemoryStream(Encoding.Unicode.GetBytes(sessionData));
        ser = new DataContractJsonSerializer(typeof(HDVDAccountSummaryInfo));
        AccountSummary = ser.ReadObject(ms) as HDVDAccountSummaryInfo;
        IsSuccess = true;
        ms.Close();
      }
      finally
      {
        if (ms != null)
        {
          ms.Dispose();
        }
      }
    }

    #endregion
  }
}
