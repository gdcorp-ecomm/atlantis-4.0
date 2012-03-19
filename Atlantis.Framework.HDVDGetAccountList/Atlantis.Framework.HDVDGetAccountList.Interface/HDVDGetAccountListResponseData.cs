using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Atlantis.Framework.HDVD.Interface;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.HDVDGetAccountList.Interface
{
  [DataContract]
  [KnownType(typeof(AriesAccountListResponse))]
  [KnownType(typeof(AriesAccountListItem))]
  [KnownType(typeof(AriesHostingResponse))]
  [KnownType(typeof(HostingResponse))]
  public class HDVDGetAccountListResponseData : IResponseData, ISessionSerializableResponse
  {

    private readonly AtlantisException _ex;
    private IList<AriesAccountListItem> _accountList;
    private int _resellerId = -1;
    private int _totalRowCount = -1;
    private HDVD.Interface.Aries.AriesAccountListResponse response;

    //required for session cache
    public HDVDGetAccountListResponseData()
    {
    }

    public HDVDGetAccountListResponseData(RequestData request, Exception ex)
    {
      _ex = new AtlantisException(request, ex.Source, ex.Message, ex.StackTrace, ex);
    }

    public HDVDGetAccountListResponseData(AtlantisException aex)
    {
      _ex = aex;
    }

    public HDVDGetAccountListResponseData(AriesAccountListResponse response)
    {
      this.response = response;
      _resellerId = response.ResellerID;
      _totalRowCount = response.TotalRowCount;
      _accountList = response.AccountList;
    }

    public bool IsSuccess
    {
      get
      {
        bool bSuccess = false;
        if (this.response != null)
        {
          bSuccess = (this.response.StatusCode == 0);
        }

        return bSuccess;

      }
    }

    public IList<AriesAccountListItem> AccountList
    {
      get { return response.AccountList; }
    }

    public int ResellerId
    {
      get { return response.ResellerID; }
    }

    public int TotalRowCount
    {
      get { return response.TotalRowCount; }
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
      DataContractSerializer ser;

      try
      {
        ser = new DataContractSerializer(typeof(AriesAccountListResponse));
        ser.WriteObject(ms, this.response);
        sessionString = Encoding.UTF8.GetString(ms.ToArray());
        ms.Close();
      }
      catch (Exception ex)
      {
        throw;
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
      DataContractSerializer ser;

      try
      {
        ms = new MemoryStream(Encoding.UTF8.GetBytes(sessionData));
        ser = new DataContractSerializer(typeof(AriesAccountListResponse));
        var accountListResponse = ser.ReadObject(ms) as AriesAccountListResponse;
        this.response = accountListResponse;
        ms.Close();
      }
      catch (Exception ex)
      {
        throw;
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
