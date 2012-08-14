using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Enums;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.QSCGetAccounts.Interface
{
  [DataContract]
  [KnownType(typeof(getAccountResponseDetail))]
  public class QSCGetAccountsResponseData : IResponseData, ISessionSerializableResponse
  {
    private readonly AtlantisException _ex;
    private getAccountResponseDetail response;

    // required for session cache
    public QSCGetAccountsResponseData()
    {
    }

    public QSCGetAccountsResponseData(RequestData request, Exception ex)
    {
      _ex = new AtlantisException(request, ex.Source, ex.Message, ex.StackTrace, ex);
    }

    public QSCGetAccountsResponseData(AtlantisException aex)
    {
      _ex = aex;
    }

    public QSCGetAccountsResponseData(getAccountResponseDetail response)
    {
      this.response = response;
    }

    private QSCStatusCodes responseStatus
    {
      get
      {
        QSCStatusCodes temp;

        if (!Enum.TryParse(response.responseStatus.statusCode.ToString(), out temp))
        {
          temp = QSCStatusCodes.FAILURE;
        }

        return temp;
      }
    }

    public IList<account> AccountList
    {
      get { return response.accounts.ToList(); }
    }

    [DataMember]
    public getAccountResponseDetail Response
    {
      get { return response; }
      set { response = value; }
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
        ser = new DataContractSerializer(typeof(getAccountResponseDetail));
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
        ser = new DataContractSerializer(typeof(getAccountResponseDetail));
        var accountListResponse = ser.ReadObject(ms) as getAccountResponseDetail;
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
