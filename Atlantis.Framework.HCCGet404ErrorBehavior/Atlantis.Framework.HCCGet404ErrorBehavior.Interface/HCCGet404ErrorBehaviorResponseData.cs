using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.HCCGet404ErrorBehavior.Interface
{
  public class HCCGet404ErrorBehaviorResponseData : IResponseData, ISessionSerializableResponse
  {
    HCCGet404ErrorBehaviorResponse _response;
    private AtlantisException _exception = null;
    private string _resultXml = string.Empty;
    private bool _success = false;

    public HCCGet404ErrorBehaviorResponse Response { get { return _response; } }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public HCCGet404ErrorBehaviorResponseData() {}
    public HCCGet404ErrorBehaviorResponseData(HCCGet404ErrorBehaviorResponse hccResponse)
    {
      _response = hccResponse;
      _success = true;
    }

    public HCCGet404ErrorBehaviorResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public HCCGet404ErrorBehaviorResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HCCGet404ErrorBehaviorResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


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
        ser = new DataContractSerializer(typeof(HCCGet404ErrorBehaviorResponse));
        _response = ser.ReadObject(ms) as HCCGet404ErrorBehaviorResponse;
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
