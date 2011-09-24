using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.HCCSet404ErrorBehavior.Interface
{
  public class HCCSet404ErrorBehaviorResponseData : IResponseData, ISessionSerializableResponse
  {
    HCCSet404ErrorBehaviorResponse _response;
    private AtlantisException _exception = null;
    string _resultXml = string.Empty;

    bool _success = false;

    public bool IsSuccess { get { return _success; } }

    public HCCSet404ErrorBehaviorResponseData() { }
    public HCCSet404ErrorBehaviorResponseData(HCCSet404ErrorBehaviorResponse hccResponse)
    {
      _response = hccResponse;
      _success = hccResponse.GetResponseStatusCode() == 0;
    }

    public HCCSet404ErrorBehaviorResponseData(string xml)
    {
      _resultXml = xml;
      _success = true;
    }

    public HCCSet404ErrorBehaviorResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public HCCSet404ErrorBehaviorResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HCCSet404ErrorBehaviorResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    public HCCSet404ErrorBehaviorResponse Response { get { return _response; } }

    #region Implementation of IResponseData

    public string ToXML()
    {
      if (_response != null)
      {
        _resultXml = _response.ToXML();
      }
      return _resultXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

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
        _response = ser.ReadObject(ms) as HCCSet404ErrorBehaviorResponse;
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
