using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.HCCGetServiceAgreement.Interface
{
  public class HCCGetServiceAgreementResponseData : IResponseData, ISessionSerializableResponse
  {
    HCCGetServiceAgreementResponse _hccResponse;
    private AtlantisException _exception = null;
    private string _resultXml = string.Empty;
    private bool _success = false;

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public HCCGetServiceAgreementResponse Response
    {
      get { return _hccResponse; }
    }

    public HCCGetServiceAgreementResponseData() { }

    public HCCGetServiceAgreementResponseData(HCCGetServiceAgreementResponse hccResponse)
    {
      _hccResponse = hccResponse;
      _success = true;
    }

     public HCCGetServiceAgreementResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public HCCGetServiceAgreementResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HCCGetServiceAgreementResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      if (_hccResponse != null)
      {
        _resultXml = _hccResponse.ToXML();
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
        var ser = new DataContractSerializer(_hccResponse.GetType());
        ser.WriteObject(xmlWriter, _hccResponse);
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
        ser = new DataContractSerializer(typeof(HCCGetServiceAgreementResponse));
        _hccResponse = ser.ReadObject(ms) as HCCGetServiceAgreementResponse;
        _success = _hccResponse != null;
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
