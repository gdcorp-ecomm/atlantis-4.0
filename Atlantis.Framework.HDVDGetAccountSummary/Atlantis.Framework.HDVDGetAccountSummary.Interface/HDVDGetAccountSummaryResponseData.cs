using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Atlantis.Framework.HDVD.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;
using Atlantis.Framework.HDVD.Interface.Aries;

namespace Atlantis.Framework.HDVDGetAccountSummary.Interface
{
  [DataContract]
  public class HDVDGetAccountSummaryResponseData : IResponseData, ISessionSerializableResponse
  {
    private AriesAccountSummaryResponse _response;
    private const string STATUS_SUCCESS = "success";

    public  AriesAccountSummaryResponse Response
    {
      get { return _response; }
    }

    private AtlantisException _ex = null;

    [DataMember]
    public bool IsSuccess { 
      get
      {
        bool bSuccess = false;
        if (this._response != null)
        {
          bSuccess = (this._response.StatusCode == 0);
        }

        return bSuccess;
      }
    }

    [DataMember]
    public int ResellerId { get; private set; }


    [DataMember]
    public AriesAccountSummaryInfo AccountSummary { get; private set; }

    public HDVDGetAccountSummaryResponseData(RequestData requestData, Exception ex)
    {
      ResellerId = -1;
      AccountSummary = null;
      _ex = new AtlantisException(requestData, ex.Source + ":HDVDGetAccountSummaryRequest", ex.Message, string.Empty, ex);
    }
    
    public HDVDGetAccountSummaryResponseData(AtlantisException ex)
    {
      ResellerId = -1;
      AccountSummary = null;
      _ex = ex;
    }

    public HDVDGetAccountSummaryResponseData(AriesAccountSummaryResponse response)
    {
      this._response = response;
      this.ResellerId = response.ResellerID;
      this.AccountSummary = response.AccountSummary;
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
      catch (Exception)
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
        ser = new DataContractJsonSerializer(typeof(AriesAccountSummaryResponse));
        ser.WriteObject(ms, this._response);
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
        ser = new DataContractJsonSerializer(typeof(AriesAccountSummaryResponse));
        this._response = ser.ReadObject(ms) as AriesAccountSummaryResponse;
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
