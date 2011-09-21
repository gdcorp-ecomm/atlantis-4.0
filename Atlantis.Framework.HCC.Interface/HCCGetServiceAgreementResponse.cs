using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.HCC.Interface
{
  [DataContract(Name = "hccresp")]
  public class HCCGetServiceAgreementResponse : IHCCResponseMessage
  {
    [DataMember(Name = "xml")]
    string _xml = string.Empty;

    [DataMember(Name = "msg")]
    string _responseMessage;
    
    [DataMember(Name = "status")]
    string _responseStatus;
    
    [DataMember(Name = "code")]
    int _responseStatusCode;

    public HCCGetServiceAgreementResponse(string responseMessage, string responseStatus, int responseStatusCode)
    {
      _responseMessage = responseMessage;
      _responseStatus = responseStatus;
      _responseStatusCode = responseStatusCode;
    }

    [DataMember(Name = "agrmnt")]
    public string Agreement { get; set; }


    public string ToXML()
    {
      if (string.IsNullOrEmpty(_xml))
      {        

        XElement response = new XElement("response",
           new XElement("message", _responseMessage),
           new XElement("status", _responseStatus),
           new XElement("statuscode", _responseStatusCode),
           new XElement("agreement", Agreement)
        );

        _xml = response.ToString();
      }

      return _xml;
    }

    #region IHCCResponseMessage Interface
    public string GetResponseMessage()
    {
      return _responseMessage;
    }

    public string GetResponseStatus()
    {
      return _responseStatus;
    }

    public int GetResponseStatusCode()
    {
      return _responseStatusCode;
    }
    #endregion
  }
}
