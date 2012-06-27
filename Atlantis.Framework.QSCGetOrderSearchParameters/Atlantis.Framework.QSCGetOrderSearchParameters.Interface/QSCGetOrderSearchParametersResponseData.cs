using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Enums;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.QSCGetOrderSearchParameters.Interface
{
  [DataContract]
  public class QSCGetOrderSearchParametersResponseData : IResponseData, ISessionSerializableResponse
  {
    private readonly AtlantisException _ex;
    private getOrderSearchParametersResponseDetail response;

    // required for session cache
    public QSCGetOrderSearchParametersResponseData()
    {
    }

    public QSCGetOrderSearchParametersResponseData(RequestData request, Exception ex)
    {
      _ex = new AtlantisException(request,ex.Source, ex.Message, ex.StackTrace, ex);
    }

    public QSCGetOrderSearchParametersResponseData(AtlantisException aex)
    {
      _ex = aex;
    }

    public QSCGetOrderSearchParametersResponseData(getOrderSearchParametersResponseDetail response)
    {
      this.response = response;
    }

    [DataMember]
    public getOrderSearchParametersResponseDetail Response
    {
      get { return response; }
      set { response = value; }
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

    private bool _isSuccess;
    [DataMember]
    public bool IsSuccess
    {
      get
      {
        bool _isSuccess = false;
        if (this.response != null)
        {
          _isSuccess = (responseStatus == QSCStatusCodes.SUCCESS);
        }

        return _isSuccess;
      }
      set { _isSuccess = value; }
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
        ser = new DataContractSerializer(typeof(getOrderSearchParametersResponseDetail));
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
        ser = new DataContractSerializer(typeof(getOrderSearchParametersResponseDetail));
        var optionListResponse = ser.ReadObject(ms) as getOrderSearchParametersResponseDetail;
        this.response = optionListResponse;
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
