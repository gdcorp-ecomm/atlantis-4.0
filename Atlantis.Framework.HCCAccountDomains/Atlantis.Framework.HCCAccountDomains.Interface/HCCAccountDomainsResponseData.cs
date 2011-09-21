using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.HCCAccountDomains.Interface
{
  public class HCCAccountDomainsResponseData : IResponseData, ISessionSerializableResponse
  {
    HCCDomainMgmtResponse _response;
    AtlantisException _exception = null;
    string _resultXml = string.Empty;

    bool _success = false;

    public bool IsSuccess { get { return _success; } }


    public HCCAccountDomainsResponseData() { }
    public HCCAccountDomainsResponseData(HCCDomainMgmtResponse hccResponse)
    {
      _response = hccResponse;
      _success = hccResponse.GetResponseStatusCode() == 0;
    }

    public HCCAccountDomainsResponseData(string xml)
    {
      _resultXml = xml;
      _success = true;
    }

    public HCCAccountDomainsResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public HCCAccountDomainsResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
        "HCCAccountDomainsResponseData",
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

    public HCCDomainMgmtResponse Response { get { return _response; } }

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
        ser = new DataContractSerializer(typeof(HCCDomainMgmtResponse));
        _response = ser.ReadObject(ms) as HCCDomainMgmtResponse;
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
