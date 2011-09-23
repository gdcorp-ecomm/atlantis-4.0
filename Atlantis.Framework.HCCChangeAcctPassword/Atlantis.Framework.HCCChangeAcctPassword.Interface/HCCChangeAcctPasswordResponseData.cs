using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.HCCChangeAcctPassword.Interface
{
  public class HCCChangeAcctPasswordResponseData : IResponseData, ISessionSerializableResponse
  {
    HCCAcctPasswordResponse _response;
    AtlantisException _exception = null;
    string _resultXml = string.Empty;

    bool _success = false;

    public bool IsSuccess { get { return _success; } }


    public HCCChangeAcctPasswordResponseData() { }
    public HCCChangeAcctPasswordResponseData(HCCAcctPasswordResponse hccResponse)
    {
      _response = hccResponse;
      _success = hccResponse.GetResponseStatusCode() == 0;
    }

    public HCCChangeAcctPasswordResponseData(string xml)
    {
      _resultXml = xml;
      _success = true;
    }

    public HCCChangeAcctPasswordResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public HCCChangeAcctPasswordResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
        "HCCChangeAcctPasswordResponseData",
        exception.Message,
        string.Empty);
    }

    public string ToXML()
    {
      if (_response != null)
      {
        _resultXml = _response.ToXML();
      }

      return _resultXml;
    }

    public HCCAcctPasswordResponse Response { get { return _response; } }

    public AtlantisException GetException()
    {
      return _exception;
    }


    #region Implementation of ISessionSerializableResponse

    public string SerializeSessionData()
    {
      var sb = new StringBuilder();
      XmlWriter xmlWriter = null;
      xmlWriter = XmlWriter.Create(sb);

      if (IsSuccess)
      {
        var ser = new DataContractSerializer(_response.GetType());
        ser.WriteObject(xmlWriter, _response);
      }

      xmlWriter.Flush();
      xmlWriter.Close();

      return sb.ToString();
    }

    public void DeserializeSessionData(string sessionData)
    {
      var ms = new MemoryStream(Encoding.Unicode.GetBytes(sessionData));
      DataContractSerializer ser;

      try
      {
        ser = new DataContractSerializer(typeof(HCCAccounts));
        _response = ser.ReadObject(ms) as HCCAcctPasswordResponse;
        _success = _response != null;
        ms.Close();
      }
      finally
      {
        ms.Dispose();
      }
    }

    #endregion
  }
}
