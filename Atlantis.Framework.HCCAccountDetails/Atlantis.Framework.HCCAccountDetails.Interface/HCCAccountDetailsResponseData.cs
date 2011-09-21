using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.HCCAccountDetails.Interface
{
  public class HCCAccountDetailsResponseData : IResponseData, ISessionSerializableResponse
  {
    HCCAccountDetailsResponse _response;
    private AtlantisException _exception = null;
    private string _resultXml = string.Empty;
    private bool _success = false;

    public HCCAccountDetailsResponse Response { get { return _response; } }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public HCCAccountDetailsResponseData() { }
    public HCCAccountDetailsResponseData(HCCAccountDetailsResponse hccResponse)
    {
      _response = hccResponse;
      _success = true;
    }

     public HCCAccountDetailsResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public HCCAccountDetailsResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HCCAccountDetailsResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

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
        ser = new DataContractSerializer(typeof(HCCAccountDetailsResponse));
        _response = ser.ReadObject(ms) as HCCAccountDetailsResponse;
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
