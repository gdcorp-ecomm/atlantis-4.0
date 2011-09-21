using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.HCC.Interface
{
  [DataContract]
  public class HCCSet404ErrorBehaviorResponse : IHCCResponseMessage
  {
    string _responseMessage;
    string _responseStatus;
    int _responseStatusCode;
    string _xml = string.Empty;

    public HCCSet404ErrorBehaviorResponse(string responseMessage, string responseStatus, int responseStatusCode)
    {
      _responseMessage = responseMessage;
      _responseStatus = responseStatus;
      _responseStatusCode = responseStatusCode;
    }

    
    [DataMember]
    public string[] Errors { get; set; }

    public string ToXML()
    {
      if (string.IsNullOrEmpty(_xml))
      {
        XElement response = new XElement("response",
           new XElement("message", _responseMessage),
           new XElement("status", _responseStatus),
           new XElement("statuscode", _responseStatusCode)
        );

        _xml = response.ToString();
      }

      return _xml;
    }
    
    #region Implementation of IHCCResponseMessage

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
